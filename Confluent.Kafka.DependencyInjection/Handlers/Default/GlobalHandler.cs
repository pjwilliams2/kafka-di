﻿using System.Collections.Concurrent;
using Confluent.Kafka.DependencyInjection.Logging;
using Microsoft.Extensions.Logging;

namespace Confluent.Kafka.DependencyInjection.Handlers.Default
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated by container")]
    class GlobalHandler : IErrorHandler, ILogHandler
    {
        readonly ConcurrentDictionary<string, ILogger> libLoggers = new();

        readonly ILoggerFactory factory;
        readonly ILogger<GlobalHandler> logger;

        public GlobalHandler(ILoggerFactory factory)
        {
            this.factory = factory;
            logger = factory.CreateLogger<GlobalHandler>();
        }

        public void OnError(IClient client, Error error) =>
            logger.LogKafkaError(client, error);

        public void OnLog(IClient client, LogMessage message)
        {
            if (!libLoggers.TryGetValue(message.Facility, out var logger))
            {
                logger = factory.CreateLogger($"rdkafka|{message.Facility}");
                libLoggers[message.Facility] = logger;
            }

            logger.Log(
                (LogLevel)message.LevelAs(LogLevelType.MicrosoftExtensionsLogging),
                0,
                new KafkaLogState(client, message.Message),
                null,
                (x, y) => x.ToString());
        }
    }
}

﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Confluent.Kafka.DependencyInjection.Logging
{
    /// <summary>
    /// Extensions for logging common Kafka scenarios.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs a Kafka partition assignment.
        /// </summary>
        /// <param name="logger">The extended logger.</param>
        /// <param name="client">The associated Kafka client.</param>
        /// <param name="offsets">The assigned Kafka partitions/offsets.</param>
        public static void LogKafkaAssignment(
            this ILogger logger,
            IClient client,
            IEnumerable<TopicPartitionOffset> offsets)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (offsets is null) throw new ArgumentNullException(nameof(offsets));

            logger.Log(
                LogLevel.Information,
                LogEvents.PartitionsAssigned,
                new OffsetLogValues(client, offsets, "Partitions assigned"),
                null,
                (x, _) => x.ToString());
        }

        /// <summary>
        /// Logs a Kafka partition revocation.
        /// </summary>
        /// <param name="logger">The extended logger.</param>
        /// <param name="client">The associated Kafka client.</param>
        /// <param name="offsets">The revoked Kafka partitions/offsets.</param>
        public static void LogKafkaRevocation(
            this ILogger logger,
            IClient client,
            IEnumerable<TopicPartitionOffset> offsets)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (offsets is null) throw new ArgumentNullException(nameof(offsets));

            logger.Log(
                LogLevel.Information,
                LogEvents.PartitionsRevoked,
                new OffsetLogValues(client, offsets, "Partitions revoked"),
                null,
                (x, _) => x.ToString());
        }

        /// <summary>
        /// Logs a Kafka commit.
        /// </summary>
        /// <param name="logger">The extended logger.</param>
        /// <param name="client">The associated Kafka client.</param>
        /// <param name="offsets">The committed Kafka offsets.</param>
        public static void LogKafkaCommit(
            this ILogger logger,
            IClient client,
            IEnumerable<TopicPartitionOffset> offsets)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (offsets is null) throw new ArgumentNullException(nameof(offsets));

            logger.Log(
                    LogLevel.Information,
                    LogEvents.OffsetsCommitted,
                    new OffsetLogValues(client, offsets, "Offsets committed"),
                    null,
                    (x, _) => x.ToString());
        }

        /// <summary>
        /// Logs a Kafka error.
        /// </summary>
        /// <param name="logger">The extended logger.</param>
        /// <param name="client">The associated Kafka client.</param>
        /// <param name="error">The Kafka error.</param>
        public static void LogKafkaError(this ILogger logger, IClient client, Error error)
        {
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            if (client is null) throw new ArgumentNullException(nameof(client));
            if (error is null) throw new ArgumentNullException(nameof(error));

            logger.Log(
                error.IsFatal ? LogLevel.Critical : LogLevel.Error,
                LogEvents.FromError(error.Code),
                new KafkaLogState(client, error),
                null,
                (x, y) => x.ToString());
        }
    }
}

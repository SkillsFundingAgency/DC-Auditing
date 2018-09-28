using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.Auditing.Persistence.Service.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Queueing.Interface;

namespace ESFA.DC.Auditing.Persistence.Service
{
    public sealed class AuditingPersistenceService<T> : IAuditingPersistenceService<T>
        where T : AuditingDto, new()
    {
        private const string SqlAudit = "INSERT INTO [dbo].[Audit] ([JobId], [DateTimeUtc], [Filename], [Source], [UserId], [Event], [ExtraInfo], [UkPrn]) VALUES (@JobId, @DateTimeUtc, @Filename, @Source, @UserId, @EventType, @ExtraInfo, @UkPrn)";

        private readonly IAuditingPersistenceServiceConfig _config;

        private readonly IQueueSubscriptionService<AuditingDto> _queueSubscriptionService;

        private readonly ILogger _logger;

        public AuditingPersistenceService(IAuditingPersistenceServiceConfig config, IQueueSubscriptionService<AuditingDto> queueSubscriptionService, ILogger logger)
        {
            _config = config;
            _queueSubscriptionService = queueSubscriptionService;
            _logger = logger;
        }

        public void Subscribe()
        {
            _queueSubscriptionService.Subscribe((dto, dict, token) => ProcessMessageAsync((T)dto, dict, token), CancellationToken.None);
        }

        private async Task<IQueueCallbackResult> ProcessMessageAsync(
            T obj,
            IDictionary<string, object> messageProperties,
            CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(obj.UserId))
                {
                    obj.UserId = "System";
                }

                using (SqlConnection connection = new SqlConnection(_config.ConnectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new QueueCallbackResult(false, null);
                    }

                    await connection.ExecuteAsync(
                        SqlAudit,
                        new { obj.JobId, DateTimeUtc = DateTime.UtcNow, obj.Filename, obj.Source, obj.UserId, obj.EventType, obj.ExtraInfo, obj.UkPrn });

                    return new QueueCallbackResult(true, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to process Audit message", ex);
                return new QueueCallbackResult(false, ex);
            }
        }
    }
}

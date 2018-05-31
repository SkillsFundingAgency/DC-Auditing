using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.Auditing.Dto;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Queueing.Interface;

namespace ESFA.DC.Auditing.Persistence.Service
{
    public sealed class AuditingPersistenceService<T> : IAuditingPersistenceService<T>
        where T : AuditingDto, new()
    {
        private const string SqlAudit = "INSERT INTO [dbo].[Audit] ([JobId], [DateTimeUtc], [Filename], [Source], [UserId], [Event], [ExtraInfo], [UkPrn]) VALUES (@JobId, @DateTimeUtc, @Filename, @Source, @UserId, @Event, @ExtraInfo, @UkPrn)";

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
            _queueSubscriptionService.Subscribe((dto, token) => ProcessMessageAsync((T)dto, token));
        }

        public async Task<bool> ProcessMessageAsync(T obj, CancellationToken cancellationToken)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_config.ConnectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return false;
                    }

                    await connection.ExecuteAsync(
                        SqlAudit,
                        new { obj.JobId, DateTimeUtc = DateTime.UtcNow, obj.Filename, obj.Source, obj.UserId, obj.EventType, obj.ExtraInfo, obj.UkPrn });

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to process Audit message", ex);
            }

            return false;
        }
    }
}

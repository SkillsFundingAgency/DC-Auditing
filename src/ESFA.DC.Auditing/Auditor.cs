using System;
using System.Threading.Tasks;
using ESFA.DC.Auditing.Dto;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.JobContext.Interface;
using ESFA.DC.Queueing.Interface;

namespace ESFA.DC.Auditing
{
    public sealed class Auditor : IAuditor
    {
        private readonly IQueuePublishService<AuditingDto> _queuePublishService;

        public Auditor(IQueuePublishService<AuditingDto> queuePublishService)
        {
            _queuePublishService = queuePublishService;
        }

        public async Task AuditStartAsync(IJobContextMessage jobContextMessage)
        {
            if (jobContextMessage.TopicPointer == 0)
            {
                await AuditAsync(
                    jobContextMessage,
                    AuditEventType.JobStarted);
            }

            await AuditAsync(
                jobContextMessage,
                AuditEventType.ServiceStarted);
        }

        public async Task AuditServiceFailAsync(IJobContextMessage jobContextMessage, Exception ex)
        {
            await AuditAsync(
                jobContextMessage,
                AuditEventType.ServiceFailed,
                ex.ToString());
        }

        public async Task AuditServiceFailAsync(IJobContextMessage jobContextMessage, string message)
        {
            await AuditAsync(
                jobContextMessage,
                AuditEventType.ServiceFailed,
                message);
        }

        public async Task AuditJobFailAsync(IJobContextMessage jobContextMessage)
        {
            await AuditAsync(
                jobContextMessage,
                AuditEventType.JobFailed);
        }

        public async Task AuditEndAsync(IJobContextMessage jobContextMessage)
        {
            await AuditAsync(
                jobContextMessage,
                AuditEventType.ServiceFinished);

            if (jobContextMessage.TopicPointer == jobContextMessage.Topics.Count - 1)
            {
                await AuditAsync(
                    jobContextMessage,
                    AuditEventType.JobFinished);
            }
        }

        public async Task AuditAsync(
            IJobContextMessage jobContextMessage,
            AuditEventType eventType,
            string extraInfo = null)
        {
            await AuditAsync(
                jobContextMessage.Topics[jobContextMessage.TopicPointer].SubscriptionName,
                eventType,
                (string)jobContextMessage.KeyValuePairs[JobContextMessageKey.Username],
                jobContextMessage.JobId,
                (string)jobContextMessage.KeyValuePairs[JobContextMessageKey.Filename],
                (string)jobContextMessage.KeyValuePairs[JobContextMessageKey.UkPrn],
                extraInfo);
        }

        public async Task AuditAsync(
            string source,
            AuditEventType eventType,
            string userId,
            long jobId = -1,
            string filename = null,
            string ukPrn = null,
            string extraInfo = null)
        {
            await _queuePublishService.PublishAsync(new AuditingDto(source, (int)eventType, userId, jobId, filename, ukPrn, extraInfo));
        }
    }
}

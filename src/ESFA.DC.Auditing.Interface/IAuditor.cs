using System;
using System.Threading.Tasks;
using ESFA.DC.JobContext.Interface;

namespace ESFA.DC.Auditing.Interface
{
    public interface IAuditor
    {
        Task AuditStartAsync(IJobContextMessage jobContextMessage);

        Task AuditServiceFailAsync(IJobContextMessage jobContextMessage, Exception ex);

        Task AuditServiceFailAsync(IJobContextMessage jobContextMessage, string message);

        Task AuditJobFailAsync(IJobContextMessage jobContextMessage);

        Task AuditEndAsync(IJobContextMessage jobContextMessage);

        Task AuditAsync(IJobContextMessage jobContextMessage, AuditEventType eventType, string extraInfo = null);

        Task AuditAsync(
            string source,
            AuditEventType eventType,
            string userId,
            long jobId = -1,
            string filename = null,
            string ukPrn = null,
            string extraInfo = null);
    }
}

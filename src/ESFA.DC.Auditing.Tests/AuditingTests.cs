using System.Threading.Tasks;
using ESFA.DC.Auditing.Dto;
using ESFA.DC.Auditing.Interface;
using ESFA.DC.Queueing.Interface;
using Moq;
using Xunit;

namespace ESFA.DC.Auditing.Tests
{
    public class AuditingTests
    {
        [Fact]
        public async Task Audit()
        {
            var queuePublishService = new Mock<IQueuePublishService<AuditingDto>>();

            queuePublishService.Setup(x => x.PublishAsync(It.IsAny<AuditingDto>())).Returns(Task.FromResult(1));

            IAuditor auditor = new Auditor(queuePublishService.Object);
            await auditor.AuditAsync(
                "Source",
                AuditEventType.JobStarted,
                "UserId",
                99999,
                "Filename",
                "UkPrn",
                "ExtraInfo");

            queuePublishService.Verify();
        }
    }
}

using ESFA.DC.DatabaseTesting.Model;
using Xunit;

namespace ESFA.DC.Auditing.Database.Tests
{
    public class AuditTesting : IClassFixture<DatabaseConnectionFixture>
    {
        private readonly DatabaseConnectionFixture _fixture;

        public AuditTesting(DatabaseConnectionFixture fixture)
        {
            _fixture = fixture;
        }

        [Trait("Category", "RegressionTest")]
        [Fact]
        public void CheckColumn()
        {
            ExpectedColumn[] expectedColumns =
            {
                ExpectedColumn.CreateBigInt("ID", 1, false),
                ExpectedColumn.CreateBigInt("JobId", 2, false),
                ExpectedColumn.CreateDateTime("DateTimeUtc", 3, false),
                ExpectedColumn.CreateNvarChar("Filename", 4, true),
                ExpectedColumn.CreateNvarChar("Source", 5, false),
                ExpectedColumn.CreateNvarChar("UserId", 6, false),
                ExpectedColumn.CreateInt("Event", 7, false),
                ExpectedColumn.CreateNvarChar("ExtraInfo", 8, true),
                ExpectedColumn.CreateNvarChar("UkPrn", 9, true)
            };
            _fixture.SchemaTests.AssertTableColumnsExist("dbo", "Audit", expectedColumns, true);
        }

        [Trait("Category", "RegressionTest")]
        [Fact]
        public void CheckColumnDoesNotExists()
        {
            ExpectedColumn[] expectedColumns =
            {
                new ExpectedColumn("xxxx", "int", false)
            };
            _fixture.SchemaTests.AssertTableColumnsExist("dbo", "Audit", expectedColumns, false);
        }
    }
}

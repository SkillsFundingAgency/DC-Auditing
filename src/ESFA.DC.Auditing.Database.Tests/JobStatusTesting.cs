using System.Collections.Generic;
using ESFA.DC.DatabaseTesting.Model;
using Xunit;

namespace ESFA.DC.Auditing.Database.Tests
{
    public class JobStatusTesting : IClassFixture<DatabaseConnectionFixture>
    {
        private readonly DatabaseConnectionFixture _fixture;

        public JobStatusTesting(DatabaseConnectionFixture fixture)
        {
            _fixture = fixture;
        }

        [Trait("Category", "RegressionTest")]
        [Fact]
        public void CheckColumn()
        {
            List<ExpectedColumn> expectedColumns = new List<ExpectedColumn>
            {
                ExpectedColumn.CreateBigInt("JobId", 1, false),
                ExpectedColumn.CreateInt("Status", 2, false),
                ExpectedColumn.CreateNvarChar("Created_By", 3, false),
                ExpectedColumn.CreateDateTime("Created_On", 4, false),
                ExpectedColumn.CreateNvarChar("Modified_By", 5, false),
                ExpectedColumn.CreateDateTime("Modified_On", 6, false)
            };
            _fixture.SchemaTests.AssertTableColumnsExist("dbo", "JobStatus", expectedColumns, true);
        }

        [Trait("Category", "RegressionTest")]
        [Fact]
        public void CheckColumnDoesNotExists()
        {
            List<ExpectedColumn> expectedColumns = new List<ExpectedColumn>
            {
                new ExpectedColumn("xxxx", "int", false)
            };
            _fixture.SchemaTests.AssertTableColumnsExist("dbo", "JobStatus", expectedColumns, false);
        }
    }
}

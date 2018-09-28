using ESFA.DC.Auditing.Interface;

namespace ESFA.DC.Auditing.Persistence.Service
{
    public sealed class AudtingPersistenceServiceConfig : IAuditingPersistenceServiceConfig
    {
        public AudtingPersistenceServiceConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}

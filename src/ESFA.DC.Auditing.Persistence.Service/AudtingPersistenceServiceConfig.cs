using ESFA.DC.Auditing.Interface;
using IAuditingPersistenceServiceConfig = ESFA.DC.Auditing.Persistence.Service.Interface.IAuditingPersistenceServiceConfig;

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

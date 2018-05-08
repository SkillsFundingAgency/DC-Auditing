using ESFA.DC.Auditing.Interface;

namespace ESFA.DC.Auditing.Persistence.Service
{
    public sealed class AudtingPersistenceServiceConfig : IAuditingPersistenceServiceConfig
    {
        public string ConnectionString { get; }
    }
}

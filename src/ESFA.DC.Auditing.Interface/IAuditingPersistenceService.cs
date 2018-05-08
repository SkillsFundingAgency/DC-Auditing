using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.Auditing.Interface
{
    public interface IAuditingPersistenceService<T>
    {
        Task<bool> ProcessMessageAsync(T obj, CancellationToken cancellationToken);
    }
}

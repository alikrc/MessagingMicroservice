using System;
using System.Threading;
using System.Threading.Tasks;

namespace Messaging.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
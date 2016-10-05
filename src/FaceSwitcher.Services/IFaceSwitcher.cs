using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FaceSwitcher.Services
{
    public interface IFaceSwitcher
    {
        Task<string> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<Guid> CreateAsync(Stream inputStream, string contentType, CancellationToken cancellationToken);
    }
}

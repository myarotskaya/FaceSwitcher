using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FaceSwitcher.Models;

namespace FaceSwitcher.Services
{
    public interface IFaceSwitcher
    {
        Task<string> GetAsync(Guid id, ImageFormat format, CancellationToken cancellationToken);
        Task<Guid> CreateAsync(Stream inputStream, string contentType, CancellationToken cancellationToken);
    }
}

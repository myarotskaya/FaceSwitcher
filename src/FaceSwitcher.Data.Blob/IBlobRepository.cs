using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FaceSwitcher.Data.Blob
{
    public interface IBlobRepository
    {
        Task<string> GetUrlAsync(string name, CancellationToken cancellationToken);
        Task DownloadAsync(Stream stream, string name, CancellationToken cancellationToken);
        Task UploadAsync(Stream stream, string name, string contentType, CancellationToken cancellationToken);
    }
}

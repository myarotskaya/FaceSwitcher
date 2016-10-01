using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FaceSwitcher.Data.Blob
{
    public interface IBlobRepository
    {
        Task<Stream> GetBlobStreamAsync(string name, CancellationToken cancellationToken);
        Task<string> UploadAsync(Stream stream, string name, CancellationToken cancellationToken);
    }
}

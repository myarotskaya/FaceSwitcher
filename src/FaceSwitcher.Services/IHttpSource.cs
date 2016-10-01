using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FaceSwitcher.Services
{
    public interface IHttpSource
    {
        Task GetStreamAsync(string url, Stream outputStream, CancellationToken cancellationToken);
    }
}

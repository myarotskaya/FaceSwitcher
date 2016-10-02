using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FaceSwitcher.Services
{
    public interface IFaceSwitcher
    {
        Task<string> ProcessAsync(Stream stream, CancellationToken cancellationToken);
    }
}

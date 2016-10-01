using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FaceSwitcher.Services
{
    public class HttpSource : IHttpSource
    {
        public async Task GetStreamAsync(string url, Stream outputStream, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                await stream.CopyToAsync(outputStream);
            }
        }
    }
}

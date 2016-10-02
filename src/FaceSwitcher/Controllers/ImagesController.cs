using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FaceSwitcher.Services;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;

namespace FaceSwitcher.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IFaceSwitcher _faceSwitcher;

        public ImagesController([NotNull] IFaceSwitcher faceSwitcher)
        {
            _faceSwitcher = faceSwitcher;
        }

        [Route("")]
        public async Task<IActionResult> Process(CancellationToken cancellationToken)
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
            {
                throw new ArgumentException();
            }

            string resultUrl;
            using (var stream = file.OpenReadStream())
            {
                resultUrl = await _faceSwitcher.ProcessAsync(stream, cancellationToken);
            }

            return Created(string.Empty, resultUrl);
        }
    }
}

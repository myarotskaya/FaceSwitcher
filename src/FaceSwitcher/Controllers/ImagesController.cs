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

        [HttpPost, Route("")]
        public async Task<IActionResult> Process(CancellationToken cancellationToken)
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
            {
                return BadRequest("The request content is empty.");
            }

            string resultUrl;
            using (var stream = file.OpenReadStream())
            {
                try
                {
                    resultUrl = await _faceSwitcher.ProcessAsync(stream, cancellationToken);
                }
                catch (ArgumentException e)
                {
                    return BadRequest(e.Message);
                }
            }

            return Created(string.Empty, resultUrl);
        }
    }
}

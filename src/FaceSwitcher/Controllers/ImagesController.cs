using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FaceSwitcher.Models;
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

        [HttpGet, Route("{id: guid}", Name = "GetImageById")]
        public async Task<IActionResult> Get(Guid id, [FromQuery] ImageFormat format, CancellationToken cancellationToken)
        {
            string url;
            try
            {
                url = await _faceSwitcher.GetAsync(id, format, cancellationToken);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }

            return Ok(new { url });
        }

        [HttpPost, Route("")]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null)
            {
                return BadRequest("The request content is empty.");
            }
            if (file.ContentType != ContentTypes.Jpg && file.ContentType != ContentTypes.Png)
            {
                return BadRequest("Unsopported content type.");
            }

            Guid id;
            using (var stream = file.OpenReadStream())
            {
                try
                {
                    id = await _faceSwitcher.CreateAsync(stream, file.ContentType, cancellationToken);
                }
                catch (ArgumentException e)
                {
                    return BadRequest(e.Message);
                }
            }

            return CreatedAtRoute("GetImageById", new { id });
        }
    }
}

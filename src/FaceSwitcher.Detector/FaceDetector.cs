using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FaceSwitcher.Models;

using JetBrains.Annotations;

using Microsoft.ProjectOxford.Face;

namespace FaceSwitcher.Detector
{
    public class FaceDetector : IFaceDetector
    {
        private readonly IFaceServiceClient _faceClient;

        public FaceDetector([NotNull] IFaceServiceClient faceClient)
        {
            _faceClient = faceClient;
        }

        public async Task<IEnumerable<FaceModel>> DetectAsync(Stream stream, CancellationToken cancellationToken)
        {
            stream.Seek(0, SeekOrigin.Begin);

            using (var detectStream = new MemoryStream())
            {
                stream.CopyTo(detectStream);
                detectStream.Seek(0, SeekOrigin.Begin);

                var faces = await _faceClient.DetectAsync(detectStream);

                return faces
                    .Select(face => face.FaceRectangle)
                    .Select(x => new FaceModel(x.Top, x.Left, x.Width, x.Height));
            }
        }
    }
}

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
            var faces = await _faceClient.DetectAsync(stream);
            var faceRectangles = faces.Select(face => face.FaceRectangle);

            return faceRectangles
                .Select(x => new FaceModel(x.Top, x.Height, x.Left, x.Width));
        }
    }
}

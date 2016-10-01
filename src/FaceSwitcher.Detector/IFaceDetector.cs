using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FaceSwitcher.Models;

namespace FaceSwitcher.Detector
{
    public interface IFaceDetector
    {
        Task<IEnumerable<FaceModel>> DetectAsync(Stream stream, CancellationToken cancellationToken);
    }
}

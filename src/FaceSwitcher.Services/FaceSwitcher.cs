using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FaceSwitcher.Data.Blob;
using FaceSwitcher.Detector;
using FaceSwitcher.Processor;

using JetBrains.Annotations;

namespace FaceSwitcher.Services
{
    public class FaceSwitcher : IFaceSwitcher
    {
        private const string CatFileName = "cat.jpg";

        private readonly IFaceDetector _faceDetector;
        private readonly IImageProcessor _imageProcessor;
        private readonly IBlobRepository _repository;

        public FaceSwitcher(
            [NotNull] IFaceDetector faceDetector,
            [NotNull] IImageProcessor imageProcessor,
            [NotNull] IBlobRepository repository)
        {
            _faceDetector = faceDetector;
            _imageProcessor = imageProcessor;
            _repository = repository;
        }

        public async Task<string> ProcessAsync(Stream inputStream, CancellationToken cancellationToken)
        {
            var faces = await _faceDetector.DetectAsync(inputStream, cancellationToken);

            using (var outputStream = new MemoryStream())
            {
                var overlayStream = await _repository.GetBlobStreamAsync(CatFileName, cancellationToken);
                _imageProcessor.Overlay(inputStream, overlayStream, outputStream, faces);

                return await _repository.UploadAsync(outputStream, Guid.NewGuid() + ".jpg", cancellationToken);
            }
        }
    }
}

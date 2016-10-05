using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using FaceSwitcher.Data.Blob;
using FaceSwitcher.Detector;
using FaceSwitcher.Models;
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
            if (faceDetector == null)
            {
                throw new ArgumentNullException(nameof(faceDetector));
            }
            if (imageProcessor == null)
            {
                throw new ArgumentNullException(nameof(imageProcessor));
            }
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _faceDetector = faceDetector;
            _imageProcessor = imageProcessor;
            _repository = repository;
        }

        public async Task<string> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _repository.GetUrlAsync(id.ToString(), cancellationToken);
        }

        public async Task<Guid> CreateAsync(Stream inputStream, string contentType, CancellationToken cancellationToken)
        {
            if (inputStream == null || !inputStream.CanRead)
            {
                throw new ArgumentException("Incorrect input stream.");
            }

            var model = new ImageModel(contentType);

            using (var outputStream = new MemoryStream())
            {
                var faces = await _faceDetector.DetectAsync(inputStream, cancellationToken);
                await OverlayAsync(inputStream, outputStream, faces, cancellationToken);
                await _repository.UploadAsync(outputStream, model.FileName, contentType, cancellationToken);
            }

            return model.Id;
        }

        private async Task OverlayAsync(Stream inputStream, Stream outputStream, IEnumerable<FaceModel> faces, CancellationToken cancellationToken)
        {
            using (var overlayStream = new MemoryStream())
            {
                await _repository.DownloadAsync(overlayStream, CatFileName, cancellationToken);
                _imageProcessor.Overlay(inputStream, overlayStream, outputStream, faces);
            }
        }
    }
}

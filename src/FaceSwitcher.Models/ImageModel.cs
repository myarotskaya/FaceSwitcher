using System;

namespace FaceSwitcher.Models
{
    public class ImageModel
    {
        public ImageModel(string contentType)
        {
            if (contentType != ContentTypes.Jpg && contentType != ContentTypes.Png)
            {
                throw new ArgumentException("Unsupported content type.");
            }

            ContentType = contentType;

            Format = ContentType == ContentTypes.Jpg
                ? ImageFormat.Jpg
                : ImageFormat.Png;
        }

        public ImageModel(Guid id, ImageFormat format)
        {
            if (format != ImageFormat.Jpg && format != ImageFormat.Png)
            {
                throw new ArgumentException("Unsopported format.");
            }

            Id = id;
            Format = format;

            ContentType = Format == ImageFormat.Jpg
                ? ContentTypes.Jpg
                : ContentTypes.Png;
        }

        public Guid Id { get; } = Guid.NewGuid();

        private string ContentType { get; }

        private ImageFormat Format { get; }

        public string Url { get; set; }

        public string FileName => Id + Format.ToString("G").ToLower();
    }
}

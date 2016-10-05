using System.Collections.Generic;
using System.Drawing;
using System.IO;

using FaceSwitcher.Models;
using FaceSwitcher.Processor.Extensions;

using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace FaceSwitcher.Processor
{
    public class ImageProcessor : IImageProcessor
    {
        public void Overlay(Stream inputStream, Stream overlayStream, Stream outputStream, IEnumerable<FaceModel> models)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
            overlayStream.Seek(0, SeekOrigin.Begin);
            outputStream.Seek(0, SeekOrigin.Begin);

            var imageBackground = Image.FromStream(inputStream);
            var imageOverlay = Image.FromStream(overlayStream);

            using (var graphics = Graphics.FromImage(imageBackground))
            {
                graphics.DrawImage(imageBackground, new Point(0,0));

                foreach (var model in models)
                {
                    graphics.DrawImageEllipse(imageOverlay, model.Left, model.Top, model.Width, model.Height);
                }
            }

            imageBackground.Save(outputStream, ImageFormat.Jpeg);
        }
    }
}

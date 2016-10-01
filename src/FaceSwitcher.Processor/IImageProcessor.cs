using System.Collections.Generic;
using System.IO;

using FaceSwitcher.Models;

namespace FaceSwitcher.Processor
{
    public interface IImageProcessor
    {
        void Overlay(Stream inputStream, Stream overlayStream, Stream outputStream, IEnumerable<FaceModel> model);
    }
}

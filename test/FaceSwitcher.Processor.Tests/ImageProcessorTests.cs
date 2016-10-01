using System.IO;

using FaceSwitcher.Models;
using FaceSwitcher.Processor.Tests.Extensions;

using Xunit;

namespace FaceSwitcher.Processor.Tests
{
    public class ImageProcessorTests
    {
        [Fact]
        public void OverlayTest()
        {
            var processor = new ImageProcessor();

            using (var stream1 = File.Open("black.bmp", FileMode.Open))
            using (var stream2 = File.Open("white.bmp", FileMode.Open))
            using (var actualResult = new MemoryStream())
            using (var expectedResult = File.Open("black_and_white.bmp", FileMode.Open))
            {
                processor.Overlay(stream1, stream2, actualResult, new FaceModel(25, 25, 50, 50));

                Assert.True(expectedResult.IsEquals(actualResult));
            }
        }
    }
}

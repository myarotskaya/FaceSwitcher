using System.Drawing;
using System.Drawing.Drawing2D;

namespace FaceSwitcher.Processor.Extensions
{
    public static class GraphicsExtensions
    {
        public static void DrawImageEllipse(this Graphics graphics, Image image, int left, int top, int width, int height)
        {
            var path = new GraphicsPath();
            var rectangle = new Rectangle(left, top, width, height);
            path.AddEllipse(rectangle);

            graphics.Clip = new Region(path);

            graphics.DrawImage(image, rectangle);
        }
    }
}

using System.IO;
using System.Linq;

namespace FaceSwitcher.Processor.Tests.Extensions
{
    public static class StreamExtensions
    {
        public static bool IsEquals(this Stream stream1, Stream stream2)
        {
            const int bufferSize = 2048;

            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                var count1 = stream1.Read(buffer1, 0, bufferSize);
                var count2 = stream2.Read(buffer2, 0, bufferSize);

                if (count1 != count2)
                    return false;

                if (count1 == 0)
                    return true;

                if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
                    return false;
            }
        }
    }
}

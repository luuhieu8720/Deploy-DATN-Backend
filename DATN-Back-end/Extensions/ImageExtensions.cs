using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DATN_Back_end.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ImageToByteArray(this Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.SaveAsJpeg(ms);
                return ms.ToArray();
            }
        }
        public static Image EnsureLimitSize(this Image image, int heightLimit, int widthLimit)
        {
            if (image.Width < widthLimit && image.Height < heightLimit)
            {
                return image;
            }

            var scaleWidth = image.Width * 1.0 / widthLimit;
            var scaleHeight = image.Height * 1.0 / heightLimit;

            var scale = Math.Max(scaleWidth, scaleHeight);

            image.Mutate(x => x.Resize((int)(image.Width / scale), (int)(image.Height / scale)));

            return image;
        }
    }
}

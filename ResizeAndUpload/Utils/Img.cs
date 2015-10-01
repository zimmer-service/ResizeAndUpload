using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResizeAndUpload.Utils
{
    public class Img
    {
        public static byte[] ResizeImage(string url, int maxSize)
        {
            byte[] res;
            int refSize;
            double factor;
            int newHeight, newWidth;
            MemoryStream ms = new MemoryStream();
            using (Bitmap img = new Bitmap(url))
            {
                if (img.Height > img.Width)
                    refSize = img.Height;
                else
                    refSize = img.Width;

                factor = maxSize / (double)refSize;
                newHeight = (int)(img.Height * factor);
                newWidth = (int)(img.Width * factor);
                using (Bitmap newImg = ResizeImage(img, newWidth, newHeight))
                {
                    newImg.Save(ms, ImageFormat.Jpeg);
                    res = ms.ToArray();
                }
            }

            return res;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}

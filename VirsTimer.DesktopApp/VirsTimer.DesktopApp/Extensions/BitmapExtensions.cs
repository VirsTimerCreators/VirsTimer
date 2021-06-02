using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VirsTimer.DesktopApp.Extensions
{
    public static class BitmapExtensions
    {
        public static Stream ToStream(this Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
    }
}

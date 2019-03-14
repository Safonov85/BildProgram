using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BildProgram
{
    // Class is Obselete - Use DetectColorPixel instead
    public class DetectAndMarkPixels
    {
        public void DrawRedPixelsOnRegion(BitmapImage bitmap, Image ImageViewWindow,
            Dictionary<PixelPosition, RGBaValue> pixelValue, int strengh)
        {
            // If no image is loaded --- AVOID NULL REFERENCE
            if (ImageViewWindow.Source == null)
            {
                return;
            }

            // Draw Red Line And Circle in the middle of PICTURE

            DrawingVisual drawVis = new DrawingVisual();
            using (DrawingContext dc = drawVis.RenderOpen())
            {
                dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                foreach(var item in pixelValue)
                {
                    if (item.Value.Red > strengh)
                    {
                        //int index = bitmap.PixelWidth * stride + 4 * bitmap.PixelHeight;
                        dc.DrawRectangle(Brushes.Red, null,
                            new Rect(new Point(item.Key.PixelPositionX, item.Key.PixelPositionY),
                                     new Point(item.Key.PixelPositionX + 1, item.Key.PixelPositionY + 1)));
                    }
                }
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawVis);

            ImageViewWindow.Source = rtb;
        }
    }
}

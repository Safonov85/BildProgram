using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BildProgram
{
    public class DetectColorPixel
    {
        public void DrawPixelsOnRegion(BitmapImage bitmap, Image ImageViewWindow,
            Dictionary<PixelPosition, RGBaValue> pixelValue, int strengh, string color, bool redMark, float blue)
        {
            // If no image is loaded --- AVOID NULL REFERENCE
            if (ImageViewWindow.Source == null)
            {
                return;
            }

            DrawingVisual drawVis = new DrawingVisual();

            if(redMark == true)
            {
                using (DrawingContext dc = drawVis.RenderOpen())
                {
                    dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                    foreach (var item in pixelValue)
                    {
                        if (item.Value.Green > item.Value.Red && item.Value.Green > item.Value.Blue
                            && item.Value.Blue < strengh && item.Value.Red < strengh)
                        {
                            dc.DrawRectangle(Brushes.Red, null,
                                new Rect(new Point(item.Key.PixelPositionX, item.Key.PixelPositionY),
                                         new Point(item.Key.PixelPositionX + 1, item.Key.PixelPositionY + 1)));
                        }
                    }
                }
            }
            else
            {
                using (DrawingContext dc = drawVis.RenderOpen())
                {
                    dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));

                    foreach (var item in pixelValue)
                    {
                        if (item.Value.Green > item.Value.Red && item.Value.Green > item.Value.Blue
                            && item.Value.Blue < strengh && item.Value.Red < strengh)
                        {
                            // Turning Detected Pixel to RED
                            byte gtoR = item.Value.Green;
                            byte rToG = item.Value.Red;
                            byte blued = item.Value.Blue;

                            if((gtoR + 20) < 255)
                            {
                                int r = gtoR;
                                r = r + 20;
                                gtoR = (byte)r;
                            }

                            float b = (float)blued * blue;
                            blued = (byte)b;
                            SolidColorBrush colorSwtich = new SolidColorBrush(Color.FromRgb(gtoR, rToG, blued));

                            dc.DrawRectangle(colorSwtich, null,
                                new Rect(new Point(item.Key.PixelPositionX, item.Key.PixelPositionY),
                                         new Point(item.Key.PixelPositionX + 1, item.Key.PixelPositionY + 1)));
                        }

                    }
                }
            }

            RenderTargetBitmap targetBitmap = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            targetBitmap.Render(drawVis);

            ImageViewWindow.Source = targetBitmap;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BildProgram
{
    public class ImagePixelsValue
    {
        public Dictionary<PixelPosition, RGBaValue> GetPixelValue = new Dictionary<PixelPosition, RGBaValue>();

        public void CopyPixelRGBInfo(BitmapImage bitmap)
        {
            if(GetPixelValue.Count != 0)
            {
                GetPixelValue.Clear();
            }
            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixelsRGBA = new byte[size];
            bitmap.CopyPixels(pixelsRGBA, stride, 0);

            //int red = 0;
            int currentPixelX = 0;
            int currentPixelY = 0;
            
            for (int i = 0; i < pixelsRGBA.Length; i += 4)
            {
                GetPixelValue.Add(new PixelPosition(currentPixelX, currentPixelY),
                    new RGBaValue(pixelsRGBA[i], pixelsRGBA[i + 1], pixelsRGBA[i + 2], pixelsRGBA[i + 3]));

                currentPixelX++;

                if (currentPixelX > bitmap.PixelWidth - 1)
                {
                    currentPixelY++;
                    currentPixelX = 0;
                }
            }
            //foreach (var pixel in pixelsRGBA)
            //{
            //    GetPixelValue.Add(new PixelPosition(currentPixelX, currentPixelY), new RGBaValue(pixelsRGBA[i], pixel + 1, pixel+2,pixel+3)
            //    if (currentPixelX > bitmap.PixelWidth - 1)
            //    {
            //        currentPixelY++;
            //        currentPixelX = 0;
            //    }

            //    if (red == 0)
            //        currentPixelX++;

            //    if (red == 3)
            //    {
            //        red = 0;
            //        continue;
            //    }
            //    red++;
            //}
            Debug.WriteLine("done");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Globalization;
using System.Diagnostics;

namespace BildProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage bitmap = new BitmapImage();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files(*.jpg;)|*.jpg;";
            if(dialog.ShowDialog() == true)
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(dialog.FileName);
                bitmap.EndInit();
                //ImageViewWindow.Width = bitmap.Width;
                //ImageViewWindow.Height = bitmap.Height;

                

                ImageViewWindow.Source = bitmap;

                //foreach(var color in bitmap.Palette.Colors)
                //{
                //    Debug.WriteLine(color);
                //}

                //bitmap.CopyPixels()
                //bitmap.UriSource = null;
            }
            
        }

        private void TestingButton()
        {
            // If no image is loaded --- AVOID NULL REFERENCE
            if(ImageViewWindow.Source == null)
            {
                return;
            }
            // make it gray
            FormatConvertedBitmap grayBitmap = new FormatConvertedBitmap();

            grayBitmap.BeginInit();

            grayBitmap.Source = bitmap;

            grayBitmap.DestinationFormat = PixelFormats.Gray8;

            grayBitmap.EndInit();

            ImageViewWindow.Source = grayBitmap;

            //BitmapEncoder encoder = new PngBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(grayBitmap));

            //using (var fileStream = new System.IO.FileStream("myImage.jpg", System.IO.FileMode.Create))
            //{
            //    encoder.Save(fileStream);
            //}

            // remove image
            //ImageViewWindow.Source = null;
        }

        void DrawOnCertainPixel()
        {
            //bitmap.CopyPixels

            DrawingVisual drawVis = new DrawingVisual();
            using (DrawingContext dc = drawVis.RenderOpen())
            {
                dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                dc.DrawLine(new Pen(Brushes.Red, 2), new Point(0, 0), new Point(bitmap.Width, bitmap.Height));
                dc.DrawRectangle(Brushes.Green, null, new Rect(20, 20, 150, 100));
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawVis);

            ImageViewWindow.Source = rtb;
        }

        void DrawNewPixels()
        {
            

            DrawingVisual drawVis = new DrawingVisual();
            using (DrawingContext dc = drawVis.RenderOpen())
            {
                dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                dc.DrawLine(new Pen(Brushes.Red, 2), new Point(0, 0), new Point(bitmap.Width, bitmap.Height));
                dc.DrawRectangle(Brushes.Green, null, new Rect(20, 20, 150, 100));
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawVis);

            
            ImageViewWindow.Source = rtb;
            //bitmap = ImageViewWindow.Source as BitmapImage;

            //BitmapSource bitmapSource = new FormatConvertedBitmap(bitmap, PixelFormats.Pbgra32, null, 0);
            //WriteableBitmap newBitmapPixels = new WriteableBitmap(bitmapSource);

            // bmp is the original BitmapImage
            //var target = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, bitmap.DpiX, bitmap.DpiY, PixelFormats.Pbgra32);
            //var visual = new DrawingVisual();

            //using (var r = visual.RenderOpen())
            //{
            //    r.DrawImage(bitmap, new Rect(0, 0, bitmap.Width, bitmap.Height));
            //    r.DrawLine(new Pen(Brushes.Red, 10.0), new Point(0, 0), new Point(bitmap.Width, bitmap.Height));
            //    //r.DrawText(new FormattedText(
            //    //    "Hello", CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
            //    //    new Typeface("Segoe UI"), 24.0, Brushes.Black), new Point(100, 10));
            //}

            //target.Render(visual);

            //ImageViewWindow.Source = bitmap;

            // SAVE IMG TO .JPG WOOOOOOOOOOOORKS!!!!!!!!!!!!!!!!!!!!!!!
            //BitmapEncoder encoder = new PngBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(rtb));

            //using (var fileStream = new System.IO.FileStream("myImage.jpg", System.IO.FileMode.Create))
            //{
            //    encoder.Save(fileStream);
            //}
            //bitmap.CopyPixels()
            
            //int index = y * stride + 4 * x;

        }

        public BitmapSource ImageSource
        {
            get
            {
                PixelFormat pf = PixelFormats.Bgr32;
                int width = 200;
                int height = 200;
                int rawStride = (width * pf.BitsPerPixel + 7) / 8;
                byte[] rawImage = new byte[rawStride * height];

                Random value = new Random();
                value.NextBytes(rawImage);

                return BitmapSource.Create(width, height, 96, 96, pf, null, rawImage, rawStride);
            }
        }

        void RotateImage()
        {
            TransformedBitmap myRotatedBitmapSource = new TransformedBitmap();

            myRotatedBitmapSource.BeginInit();

            myRotatedBitmapSource.Source = bitmap;

            myRotatedBitmapSource.Transform = new RotateTransform(180);
            myRotatedBitmapSource.EndInit();

            ImageViewWindow.Source = myRotatedBitmapSource;
            //bitmap = (BitmapImage)ImageViewWindow.Source;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            DrawOnePixel(new Point(20, 30), new Point(21, 31));
            //DrawOnCertainPixel();

            // draw pixels on original picture
            //DrawNewPixels();

            // obvious
            //RotateImage();

            // grayscale image
            //TestingButton();
        }

        void DrawOnePixel(Point pt1, Point pt2)
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

                // Drawing a line with PIXELS
                for (int i = 0; i < 30; i++)
                {
                    dc.DrawRectangle(Brushes.Red, null, new Rect(pt1, pt2));
                    pt1.X += 1;
                    pt1.Y += 1;
                    pt2.X += 1;
                    pt2.Y += 1;
                }

                dc.DrawEllipse(Brushes.Green, new Pen(Brushes.HotPink, 3), new Point(bitmap.PixelWidth/2, bitmap.PixelHeight/2), 60, 60);

            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(bitmap.PixelWidth, bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawVis);

            ImageViewWindow.Source = rtb;

            int stride = bitmap.PixelWidth * 4;
            int size = bitmap.PixelHeight * stride;
            byte[] pixels = new byte[size];
            bitmap.CopyPixels(pixels, stride, 0);

            foreach (var pixel in pixels)
            {
                Debug.WriteLine(pixel);
            }
        }

        private void SavePicButton_Click(object sender, RoutedEventArgs e)
        {

        }

        void SaveCurrentPicture(RenderTargetBitmap rtb)
        {
            //SAVE IMG TO.JPG WOOOOOOOOOOOORKS!!!!!!!!!!!!!!!!!!!!!!!
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fileStream = new System.IO.FileStream("myImage.jpg", System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        //public void ToGrayScale(Bitmap Bmp)
        //{
        //    int rgb;
        //    Color c;

        //    for (int y = 0; y < Bmp.Height; y++)
        //    {
        //        for (int x = 0; x < Bmp.Width; x++)
        //        {
        //            c = Bmp.GetPixel(x, y);
        //            rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
        //            Bmp.SetPixel(x, y, Color.FromArgb(255, (byte)rgb, (byte)rgb, (byte)rgb));
        //        }
        //    }
        //}
    }
}

﻿using System;
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
        ImagePixelsValue imagePixelValue = new ImagePixelsValue();
        DetectColorPixel detectColorPix = new DetectColorPixel();
        bool checkIfSliderIsWorking = false;

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

                ImageViewWindow.Source = bitmap;

                imagePixelValue.CopyPixelRGBInfo(bitmap);
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
            
        }

        void DrawOnCertainPixel()
        {

            DrawingVisual drawVis = new DrawingVisual();
            using (DrawingContext dc = drawVis.RenderOpen())
            {
                dc.DrawImage(bitmap, new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight));
                dc.DrawLine(new Pen(Brushes.Red, 2), new Point(0, 0), new Point(bitmap.Width, bitmap.Height));
                dc.DrawRectangle(Brushes.Green, null, new Rect(20, 20, 150, 100));
            }

            RenderTargetBitmap targetBitmap = new RenderTargetBitmap(bitmap.PixelWidth,
                            bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            targetBitmap.Render(drawVis);

            ImageViewWindow.Source = targetBitmap;
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
        }

        void RotateImage()
        {
            TransformedBitmap myRotatedBitmapSource = new TransformedBitmap();

            myRotatedBitmapSource.BeginInit();

            myRotatedBitmapSource.Source = bitmap;

            myRotatedBitmapSource.Transform = new RotateTransform(180);
            myRotatedBitmapSource.EndInit();

            ImageViewWindow.Source = myRotatedBitmapSource;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            
            RedSlider.Value = 0.2;
            detectColorPix.DrawPixelsOnRegion(bitmap, ImageViewWindow, imagePixelValue.GetPixelValue,
                (int)SliderIntencity.Value, "Green", (bool)checkboxTest.IsChecked, (float)RedSlider.Value);
        }

        void DrawRedPixelsOnRegion()
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
                
                int stride = bitmap.PixelWidth * 4;
                int size = bitmap.PixelHeight * stride;
                byte[] pixels = new byte[size];
                bitmap.CopyPixels(pixels, stride, 0);

                int red = 0;
                int currentPixelX = 0;
                int currentPixelY = 0;
                foreach (var pixel in pixels)
                {
                    // REDIFY all the parts that are DARK
                    if (red == 0 && pixel < 50)
                    {
                        dc.DrawRectangle(Brushes.Red, null,
                            new Rect(new Point(currentPixelX, currentPixelY),
                            new Point(currentPixelX + 1, currentPixelY + 1)));
                    }

                    
                    if (currentPixelX > bitmap.PixelWidth - 1)
                    {
                        currentPixelY++;
                        currentPixelX = 0;
                    }

                    if(red == 0)
                        currentPixelX++;

                    if (red == 3)
                    {
                        red = 0;
                        continue;
                    }
                    red++;
                }
                
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(bitmap.PixelWidth,
                            bitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(drawVis);

            ImageViewWindow.Source = rtb;

            
        }

        private void SavePicButton_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap rtb = (RenderTargetBitmap)ImageViewWindow.Source;
            SaveCurrentPicture(rtb);
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

        private void SliderIntencity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (checkIfSliderIsWorking == false)
            {
                return;
            }
            
            detectColorPix.DrawPixelsOnRegion(bitmap, ImageViewWindow, imagePixelValue.GetPixelValue,
                         (int)SliderIntencity.Value, "Green", (bool)checkboxTest.IsChecked, (float)RedSlider.Value);

        }

        void CreateALine()
        {

            byte redCode = 255;
            for (int j = 0; j < 250; j += 1)
            {
                for (int i = 0; i < 250; i++)
                {
                    // Create a Line  
                    Line line = new Line();
                    line.X1 = j;
                    line.Y1 = i;
                    line.X2 = j + 1;
                    line.Y2 = i + 1;

                    // Create a red Brush  
                    SolidColorBrush redBrush = new SolidColorBrush();
                    redBrush.Color = Color.FromRgb(redCode, (byte)i, (byte)i);

                    // Set Line's width and color
                    line.StrokeThickness = 1;
                    line.Stroke = redBrush;
                    

                    // Add line to the Grid.
                    GridMain.Children.Add(line);
                    
                }
                redCode--;
            }
        }

        private void AutoGenerateButton_Click(object sender, RoutedEventArgs e)
        {
            DrawingVisual drawVis = new DrawingVisual();

            CreateALine();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            checkIfSliderIsWorking = true;
        }

        private void RedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (checkIfSliderIsWorking == false)
            {
                return;
            }

            detectColorPix.DrawPixelsOnRegion(bitmap, ImageViewWindow, imagePixelValue.GetPixelValue,
                (int)SliderIntencity.Value, "Green", (bool)checkboxTest.IsChecked, (float)RedSlider.Value);

            BlueLayerLabel.Content = "Blue Layer " + RedSlider.Value.ToString("0.00");
        }
    }
}

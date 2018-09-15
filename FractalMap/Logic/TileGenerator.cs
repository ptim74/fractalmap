using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace FractalMap.Logic
{
    public class TileGenerator
    {
        const int SIZE = 256;
        private BitmapData data;

        public double MinY { get; protected set; }
        public double MinX { get; protected set; }
        public double MaxX { get; protected set; }
        public double MaxY { get; protected set; }

        public List<Color> Palette { get; set; }

        protected readonly IFractal fractal;
        protected readonly double shades;

        public TileGenerator(IFractal fractal, int shades, int z, long x, long y)
        {
            this.fractal = fractal;
            this.shades = shades;

            long n = (long)Math.Pow(2, z);
            double xScale = (fractal.MaxX - fractal.MinX) / n;
            double yScale = (fractal.MaxY - fractal.MinY) / n;

            MinX = fractal.MinX + xScale * x;
            MaxX = fractal.MinX + xScale * (x + 1);

            MinY = fractal.MinY + yScale * y;
            MaxY = fractal.MinY + yScale * (y + 1);
        }

        public Stream GetImageStream()
        {
            using (var bitmap = this.DrawBitmap())
            {
                var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
        }

        protected Bitmap DrawBitmap()
        {
            double xScale = (MaxX - MinX) / SIZE;
            double yScale = (MaxY - MinY) / SIZE;

            Bitmap bitmap = new Bitmap(SIZE, SIZE, PixelFormat.Format24bppRgb);
            data = bitmap.LockBits(new Rectangle(0, 0, SIZE, SIZE), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            double curX = MinX;

            for (int x = 0; x < SIZE; x++)
            {
                double curY = MinY;

                for (int y = 0; y < SIZE; y++)
                {
                    var colorValue = fractal.GetColor(curX, curY);
                    colorValue = colorValue / shades;

                    if (colorValue > 0.0)
                    {
                        int factor = (int)((colorValue % 1) * 0xff);
                        int colorId1 = (int)(colorValue % Palette.Count);
                        int colorId2 = colorId1 + 1;
                        if (colorId2 > Palette.Count - 1)
                            colorId2 = 0;

                        SetPixel(x, y, InterpolateColors(Palette[colorId1], Palette[colorId2], factor));
                    }
                    else
                    {
                        SetPixel(x, y, Color.Black);
                    }

                    curY += yScale;
                }

                curX += xScale;
            }

            bitmap.UnlockBits(data);
            return bitmap;
        }

        protected unsafe void SetPixel(int x, int y, Color color)
        {
            byte* imagePointer = (byte*)data.Scan0;
            int offset = (y * data.Stride) + (3 * x);
            byte* px = (imagePointer + offset);
            *(px++) = color.B;
            *(px++) = color.G;
            *px = color.R;
        }

        protected Color InterpolateColors(Color c1, Color c2, int weigth)
        {
            byte red = (byte)(((int)c1.R + ((int)((c2.R - c1.R) * weigth) >> 8)) & 0xff);
            byte green = (byte)(((int)c1.G + ((int)((c2.G - c1.G) * weigth) >> 8)) & 0xff);
            byte blue = (byte)(((int)c1.B + ((int)((c2.B - c1.B) * weigth) >> 8)) & 0xff);

            return Color.FromArgb(red, green, blue);
        }
    }
}

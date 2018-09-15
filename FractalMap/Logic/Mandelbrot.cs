using System;

namespace FractalMap.Logic
{
    public class Mandelbrot : IFractal
    {
        private readonly int iterations;
        private readonly double bailout;
        private readonly bool interpolate;

        protected readonly double log2 = Math.Log(2);

        public double MinY { get; protected set; }
        public double MinX { get; protected set; }
        public double MaxX { get; protected set; }
        public double MaxY { get; protected set; }

        public Mandelbrot(int iterations, bool interpolate, double bailout)
        {
            this.iterations = iterations;
            this.interpolate = interpolate;
            this.bailout = bailout;
            MaxY = 2.0;
            MinY = -2.0;
            MinX = -2.5;
            MaxX = 1.5;
        }

        public double GetColor(double x, double y)
        {
            int loop = 0;
            double zx = 0.0;
            double zy = 0.0;
            double zx2 = 0.0;
            double zy2 = 0.0;
            while (loop++ < iterations)
            {
                zy = 2.0 * zx * zy + y;
                zx = zx2 - zy2 + x;
                zx2 = zx * zx;
                zy2 = zy * zy;
                if (zx2 + zy2 > bailout)
                {
                    if(!interpolate)
                        return loop;

                    double zn = Math.Sqrt(zx2 + zy2);
                    double nu = Math.Log(Math.Log(zn) / log2) / log2;
                    return loop + 1 - nu;
                }
            }
            return 0.0;
        }
    }
}

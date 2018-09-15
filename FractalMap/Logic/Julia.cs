using System;

namespace FractalMap.Logic
{
    public class Julia : IFractal
    {
        private readonly int iterations;
        private readonly double bailout;
        private readonly bool interpolate;
        private readonly double cx;
        private readonly double cy;

        protected readonly double log2 = Math.Log(2);

        public double MinY { get; protected set; }
        public double MinX { get; protected set; }
        public double MaxX { get; protected set; }
        public double MaxY { get; protected set; }

        public Julia(double cx, double cy, int iterations, bool interpolate, double bailout)
        {
            this.cx = cx;
            this.cy = cy;
            this.iterations = iterations;
            this.interpolate = interpolate;
            this.bailout = bailout;
            MaxY = 2.0;
            MinY = -2.0;
            MinX = -2.0;
            MaxX = 2.0;
        }

        public double GetColor(double x, double y)
        {
            int loop = 0;
            double zx = x;
            double zy = y;
            double zx2 = zx * zx;
            double zy2 = zy * zy;
            while (loop++ < iterations)
            {
                zy = 2.0 * zx * zy + cy;
                zx = zx2 - zy2 + cx;
                zx2 = zx * zx;
                zy2 = zy * zy;
                if (zx2 + zy2 > bailout)
                {
                    if (!interpolate)
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

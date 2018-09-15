using System;

namespace FractalMap.Logic
{
    public interface IFractal
    {
        double MinY { get; }
        double MinX { get; }
        double MaxX { get; }
        double MaxY { get; }

        double GetColor(double x, double y);
    }
}

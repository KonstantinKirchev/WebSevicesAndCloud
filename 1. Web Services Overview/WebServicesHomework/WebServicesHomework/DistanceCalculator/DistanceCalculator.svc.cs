using System;

namespace DistanceCalculator
{
    public class DistanceCalculator : ICalculator
    {
        public double CalculateDistance(Point startPoint, Point endPoint)
        {
            return Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Models
{
    public enum SplineType { None, Cubic, Monotone }
    public class Spline
    {
        private int _yPrecision = 4;
        public SplineType splineType { get; set; }
        public double x1 { get; }
        public double y1 { get; }
        public double x2 { get; }
        public double y2 { get; }
        public double delta => (y2 - y1) / (x2 - x1);
        public double h => x2 - x1;

        public double[] derivatives { get; set; }
        public double[] coefficients { get; set; }

        public Spline(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;

            splineType = SplineType.None;
            derivatives = new double[2];
            coefficients = new double[4];
        }
        public double interpolate(double globalX)
        {
            if(globalX < x1 || globalX > x2) throw new ArgumentOutOfRangeException();
            double y;
            double x;
            double a, b, c, d;
            if(splineType == SplineType.Cubic)
            {
                x = globalX;
                a = coefficients[0];
                b = coefficients[1];
                c = coefficients[2];
                d = coefficients[3];
            }
            else if(splineType == SplineType.Monotone)
            {
                x = globalX - x1;
                a = (derivatives[0] + derivatives[1] - 2 * delta) / Math.Pow(h, 2);
                b = (-2 * derivatives[0] - derivatives[2] + 3 * delta) / h;
                c = derivatives[0];
                d = y1;
            }
            else
            {
                throw new NotSupportedException();
            }
            y = a * Math.Pow(x, 3) + b * Math.Pow(x, 2) + c * x + d;
            return y;
        }
        public double interpolateDerivative(double globalX)
        {
            if (globalX < x1 || globalX > x2) throw new ArgumentOutOfRangeException();

            double a, b, c;
            double x;

            if(splineType == SplineType.Cubic)
            {
                x = globalX;
                a = coefficients[0];
                b = coefficients[1];
                c = coefficients[2];
            }
            else if(splineType == SplineType.Monotone)
            {
                x = globalX - x1;
                a = 3 * (derivatives[0] - 2 * delta) / Math.Pow(h, 2);
                b = 2 * (derivatives[0] - derivatives[1] + 3 * delta) / h;
                c = derivatives[0];
            }
            else
            {
                throw new NotSupportedException();
            }
            double dydx = a * Math.Pow(x, 2) + b * x + c;
            return dydx;
        }
        public double invInterpolate(double globalY)
        {
            if (globalY < y1 || globalY > y1) throw new ArgumentOutOfRangeException();

            double s = x1;
            double e = x2;
            double x = (s + e) / 2;
            double guess = Math.Round(interpolate(x), _yPrecision);
            double y = Math.Round(globalY, _yPrecision);

            while(guess != y)
            {
                if(guess > y)
                {
                    s = x;
                }
                else
                {
                    e = x;
                }
                x = (s + e) / 2;
                guess = Math.Round(interpolate(x), _yPrecision);
            }
            return x;
        }
        public double interpolateArea(double globalX)
        {
            if (globalX < x1 || globalX > x2) throw new ArgumentOutOfRangeException();

            double area, xf, xi;
            double a, b, c, d;
            if(splineType == SplineType.Cubic)
            {
                a = coefficients[0];
                b = coefficients[1];
                c = coefficients[2];
                d = coefficients[3];
                xf = globalX;
                xi = x1;
            }
            else if(splineType == SplineType.Monotone)
            {
                a = (derivatives[0] + derivatives[1] - 2 * delta) / Math.Pow(h, 2);
                b = (-2 * derivatives[0] - derivatives[1] + 3 * delta) / h;
                c = derivatives[0];
                d = y1;
                xf = globalX - x1;
                xi = 0;
            }
            else
            {
                throw new NotSupportedException();
            }

            double A = (a / 4) * Math.Pow(xf, 4) + (b / 3) * Math.Pow(xf, 3) + (c / 2) * Math.Pow(xf, 2) + d * xf;
            double B = (a / 4) * Math.Pow(xi, 4) + (b / 3) * Math.Pow(xi, 3) + (c / 2) * Math.Pow(xi, 2) + d * xi;

            area = A - B;
            return area;
        }
        public double totalArea()
        {
            return interpolateArea(x2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Models
{
    public enum SplineType { None, Cubic, Monotone }
    public class Spline
    {
        private int _yPrecision = 4;
        private double _aucX1;
        private double _area;
        public SplineType splineType { get; set; }
        public double x1 { get; }
        public double y1 { get; }
        public double x2 { get; }
        public double y2 { get; }
        public double delta => (y2 - y1) / (x2 - x1);
        public double h => x2 - x1;
        public double aucX1 { get => _aucX1; set { _aucX1 = value; } }

        public double[] derivatives { get; set; }
        public double[] coefficients { get; set; }

        public Spline(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = Math.Round(y1, 4);
            this.x2 = x2;
            this.y2 = Math.Round(y2, 4);

            _area = -1;
            _aucX1 = -1;

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
            if (globalY > y1 || globalY < y2) throw new ArgumentOutOfRangeException();
            if (splineType == SplineType.None) throw new NotSupportedException();

            double a, b, c, d;
            double x;

            if(splineType == SplineType.Cubic)
            {
                a = coefficients[0];
                b = coefficients[1];
                c = coefficients[2];
                d = coefficients[3] - globalY;
            }
            else
            {
                throw new NotSupportedException();
            }

            (double? r1, double? r2, double? r3) = findRoots(a, b, c, d);

            if (isCorrectRoot(r1))
            {
                x = r1.Value;
            }
            else if(isCorrectRoot(r2))
            {
                x = r2.Value;
            }
            else if(isCorrectRoot(r3))
            {
                x = r3.Value;
            }
            else
            {
                throw new Exception("Roots unable to be found");
            }

            if(splineType == SplineType.Monotone)
            {
                x = x - x1;
            }

            x = Math.Round(x, 4);
            return x;
        }
        private Boolean isCorrectRoot(double? rb)
        {
            double r;
            double a = x1;
            double b = x2;

            if(rb.HasValue)
            {
                r = Math.Round(rb.Value, 4);
            }
            else
            {
                return false;
            }

            a = Math.Round(a, 4);
            b = Math.Round(b, 4);

            return (a <= r) && (r <= b);
        }
        private double getArea(double globalX1, double globalX2)
        {
            if (globalX1 < x1 || globalX2 > x2 || globalX1 > globalX2) throw new ArgumentOutOfRangeException();

            double area, xf, xi;
            double a, b, c, d;
            if(splineType == SplineType.Cubic)
            {
                a = coefficients[0];
                b = coefficients[1];
                c = coefficients[2];
                d = coefficients[3];
                xf = globalX2;
                xi = globalX1;
            }
            else if(splineType == SplineType.Monotone)
            {
                a = (derivatives[0] + derivatives[1] - 2 * delta) / Math.Pow(h, 2);
                b = (-2 * derivatives[0] - derivatives[1] + 3 * delta) / h;
                c = derivatives[0];
                d = y1;
                xf = globalX2 - x1;
                xi = globalX1 - x1;
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
        public static (double?, double?, double?) findRoots(double a, double b, double c, double d)
        {
            double? x1 = null;
            double? x2 = null;
            double? x3 = null;

            double ONETHIRD = 1.0 / 3.0;

            double p = -b / (3 * a);
            double q = Math.Pow(p, 3) + (b * c - 3 * a * d) / (6 * Math.Pow(a, 2));
            double r = c / (3 * a);

            double T = Math.Pow(q, 2) + Math.Pow(r - Math.Pow(p, 2), 3);

            double u, v;
            if(T >= 0)
            {
                double qtu = q + Math.Sqrt(T);
                double qtv = q - Math.Sqrt(T);

                u = Math.Pow(Math.Abs(qtu), ONETHIRD);
                v = Math.Pow(Math.Abs(qtv), ONETHIRD);

                u = (qtu < 0)? -u : u;
                v = (qtv < 0)? -v : v;

                x1 = u + v + p;

                if(Math.Abs(T) < 1e-6)
                {
                    x2 = -u + p;
                    x3 = x2;
                }
            }
            else
            {
                double cr = q;
                double ci = Math.Sqrt(-T);

                double theta = Math.Atan2(ci, cr);
                double amp = Math.Sqrt(cr * cr + ci * ci);

                double cubedRtAmp = Math.Pow(Math.Abs(amp), ONETHIRD);
                cubedRtAmp = (amp < 0) ? -cubedRtAmp : cubedRtAmp;

                theta = theta * ONETHIRD;

                x1 = 2 * cubedRtAmp * Math.Cos(theta) + p;
                x2 = 2 * cubedRtAmp * Math.Cos(theta + 2 * Math.PI / 3) + p;
                x3 = 2 * cubedRtAmp * Math.Cos(theta + 4 * Math.PI / 3) + p;
            }

            return (x1, x2, x3);
        }
        public double getTotalArea()
        {
            return (_area == -1) ? getArea(x1, x2) : _area;
        }
        public double getAUCFromX(double globalX)
        {
            if (_aucX1 == -1) throw new Exception("Calling AUC before _aucX1 is computed");
            return _aucX1 + getArea(x1, globalX);
        }
        public double getAUCFromY(double globalY)
        {
            double x = invInterpolate(globalY);
            return getAUCFromX(x);
        }
    }
}

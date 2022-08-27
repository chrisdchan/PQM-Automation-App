using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Models
{
    public enum SplineType { None, Monotone }
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
        public double delta { get; }
        public double h { get; }
        public double aucX1 { get => _aucX1; set { _aucX1 = value; } }
        private (double a, double b, double c, double d) hermite { get; set; }
        public Spline(double x1, double y1, double x2, double y2)
        {
            this.x1 = x1;
            this.y1 = Math.Round(y1, 4);
            this.x2 = x2;
            this.y2 = Math.Round(y2, 4);

            delta = (y2 - y1) / (x2 - x1);
            h = x2 - x1;

            _area = -1;
            _aucX1 = -1;

            splineType = SplineType.None;
        }

        /*
         * Initialization from Structure
         */
        public void setHermiteCoefficients(double d1, double d2)
        {
            double a = (d1 + d2 - 2 * delta) / (h * h);
            double b = (-2 * d1 - d2 + 3 * delta) / h;
            double c = d1;
            double d = y1;

            double i = a;
            double j = -3 * a * x1 + b;
            double k = 3 * a * x1 * x1 - 2 * b * x1 + c;
            double l = -a * Math.Pow(x1, 3) + b * x1 * x1 - c * x1 + d;

            hermite = (i, j, k, l);
            splineType = SplineType.Monotone;
        }
        public double getTotalArea()
        {
            return (_area == -1) ? getArea(x1, x2) : _area;
        }

        /*
         * Interpolation and AUC
         */
        public double interpolate(double globalX)
        {
            if(globalX < x1 || globalX > x2) throw new ArgumentOutOfRangeException();
            double y;
            double a, b, c, d;

            (a, b, c, d) = getCoefficients();
            y = a * Math.Pow(globalX, 3) + b * Math.Pow(globalX, 2) + c * globalX + d;
            return y;
        }
        public double interpolateDerivative(double globalX)
        {
            if (globalX < x1 || globalX > x2) throw new ArgumentOutOfRangeException();

            double a, b, c;
            double x;

            (a, b, c, _) = getCoefficients();

            double dydx = a * Math.Pow(globalX, 2) + b * globalX + c;
            return dydx;
        }
        public double invInterpolate(double globalY)
        {
            if (globalY > y1 || globalY < y2) throw new ArgumentOutOfRangeException();
            if (splineType == SplineType.None) throw new NotSupportedException();

            if (relativeEq(delta, 0)) return x2;

            double a, b, c, d;
            double x;

            (a, b, c, d) = getCoefficients();

            d = d - globalY;

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

            x = Math.Round(x, 4);
            return x;
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

        /*
         * Helper Methods
         */
        private (double, double, double, double) getCoefficients()
        {
            if (splineType == SplineType.Monotone) return hermite;
            else throw new NotSupportedException();
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
        private double getArea(double xi, double xf)
        {
            if (xi < x1 || xf > x2 || xi > xf) throw new ArgumentOutOfRangeException();

            double area;
            double a, b, c, d;
            (a, b, c, d) = getCoefficients();

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

        private static Boolean relativeEq(double a, double b)
        {
            return (Math.Round(a, 5) == Math.Round(b, 5));
        }
    }
}

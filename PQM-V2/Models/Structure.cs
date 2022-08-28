using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PQM_V2.Models
{
    public class Structure
    {
        private string _filePath;
        private string _metric;
        private bool _invalidReadFlag;
        private double _maxX;
        private List<Spline> _splines;

        public SolidColorBrush color { get; set; }
        public string name { get; set; }
        public double maxX => _maxX;
        public string metric => _metric;
        public Structure(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            _filePath = filePath;
            _splines = new List<Spline>();

            setSplines();
            _maxX = _splines[_splines.Count - 1].x2;
            setMetricAndName();

            setHermites();
            setAreas();
        }
        private void setSplines()
        {
            List<double> newX = new List<double>();
            List<double> newY = new List<double>();

            //TODO: parse file here
            using (StreamReader sr = new StreamReader(_filePath))
            {
                string line;
                Regex rxIdX = new Regex("ec.normJ|ec.normE|SAR/"); // Identify X value row
                Regex rxExtractX = new Regex("[<>=]([0-9.]*)"); // Extract X  values
                Regex rxExtractY = new Regex("[0-9.E-]*"); // Extract Y values

                // Search for data line
                while ((line = sr.ReadLine()) != null)
                {
                    if (rxIdX.IsMatch(line)) break;
                }

                MatchCollection matches = rxExtractX.Matches(line);
                foreach (Match match in matches)
                {
                    string value = match.Value.ToString();
                    value = value.Substring(1);
                    if (float.TryParse(value, out float newVal))
                    {
                        newX.Add(newVal);
                    }
                }

                line = sr.ReadLine();
                matches = rxExtractY.Matches(line);
                int numZeros = 0;
                foreach (Match match in matches)
                {
                    string value = match.Value.ToString();

                    if (double.TryParse(value, out double newVal))
                    {
                        if (newVal < 0) newVal = 0;
                        if (newVal == 0) numZeros++;
                        if (numZeros > 2) break;

                        newY.Add(newVal);
                    }
                }
            }

            if (newX.Count == 0 || newY.Count == 0)
            {
                MessageBox.Show("Can not parse file at " + _filePath);
                _invalidReadFlag = true;
                _splines = null;
                return;
            }
            else
            {
                newY.RemoveAt(0);
                newY = normalize(newY);
            }

            if(newX.Count != newY.Count)
            {
                newX = newX.GetRange(0, newY.Count);
            }

            for(int i = 0; i < newX.Count - 1; i++)
            {
                _splines.Add(new Spline(newX[i], newY[i], newX[i + 1], newY[i + 1]));
            }
        }
        private List<double> normalize(List<double> Y)
        {
            double max = Y[0];
            List<double> normalized = new List<double>();

            foreach (double y in Y)
            {
                normalized.Add(y * 100 / max);
            }

            return normalized;
        }
        private void setMetricAndName()
        {
            string fileName = Path.GetFileName(_filePath);
            string[] fileNameParts = fileName.Split(' ');
            _metric = fileNameParts[0];

            string last = fileNameParts.Last();
            if (last == "Raw.csv")
            {
                string myName = "";
                for (int i = 1; i < fileNameParts.Length - 1; i++)
                {
                    myName += fileNameParts[i] + " ";
                }
                name = myName;
            }
            else
            {
                name = last;
            }
        }
        private void setHermites()
        {
            double[,] derivGuesses = new double[_splines.Count, 2];
           
            derivGuesses[0, 0] =  _splines[0].delta * 0.5;

            for(int i = 0; i < _splines.Count-1; i++)
            {
                double d = (_splines[i].delta + _splines[i + 1].delta) * 0.5;
                derivGuesses[i, 1] = d;
                derivGuesses[i + 1, 0] = d;
            }
            derivGuesses[_splines.Count - 1, 1] = _splines[_splines.Count -1].delta * 0.5;

            for(int i = 0; i < _splines.Count; i++)
            {
                double alpha, beta, disc;

                if(relativeEq(_splines[i].delta, 0.0))
                {
                    derivGuesses[i, 0] = 0.0;
                    derivGuesses[i, 1] = 0.0;

                    alpha = 0.0;
                    beta = 0.0;
                }

                alpha = derivGuesses[i, 0] / _splines[i].delta;
                beta = derivGuesses[i, 1] / _splines[i].delta;

                disc = alpha * alpha + beta * beta;
                if(disc > 9.0)
                {
                    derivGuesses[i, 0] = (3.0 * _splines[i].delta * alpha) / Math.Sqrt(disc);
                    derivGuesses[i, 1] = (3.0 * _splines[i].delta * beta) / Math.Sqrt(disc);
                }
            }

            for(int i = 0; i < _splines.Count - 1; i++)
            {
                if (Math.Abs(derivGuesses[i, 1]) < Math.Abs(derivGuesses[i + 1, 0]))
                {
                    derivGuesses[i + 1, 0] = derivGuesses[i, 1];
                }
                else
                {
                    derivGuesses[i, 1] = derivGuesses[i + 1, 0];
                }
            }

            for(int i = 0; i < _splines.Count; i++)
            {
                _splines[i].setHermiteCoefficients(derivGuesses[i, 0], derivGuesses[i, 1]);
            }
        }
        private void setAreas()
        {
            double totalArea = 0;
            foreach(Spline spline in _splines)
            {
                double area = spline.getTotalArea();
                spline.aucX1 = totalArea;
                totalArea += area;
            }
        }
        private int getSplineFromX(double x)
        {
            if (x < 0 || _maxX < x) throw new ArgumentOutOfRangeException();

            int start = 0;
            int end = _splines.Count - 1;
            int spline = (end - start) / 2;

            while(x < _splines[spline].x1 || _splines[spline].x2 < x)
            {
                if(x < _splines[spline].x1)
                {
                    end = spline;
                }
                else
                {
                    start = spline;
                }

                spline = (end + start) / 2;
            }
            return spline;
        }
        private int getSplineFromY(double y)
        {
            if (y < 0 || y > 100) throw new ArgumentOutOfRangeException();
            int start = 0;
            int end = _splines.Count - 1;
            int spline = (end - start) / 2;

            while(y < Math.Round(_splines[spline].y2, 4) || y > Math.Round(_splines[spline].y1, 4))
            {
                if(y < _splines[spline].y2)
                {
                    start = spline;
                }
                else
                {
                    end = spline;
                }
                spline = (end + start) / 2;
            }
            return spline;
        }
        public double interpolate(double x)
        {
            int spline = getSplineFromX(x);
            double y = _splines[spline].interpolate(x);
            return y;
        }
        public double interpolateDerivative(double x)
        {
            int spline = getSplineFromX(x);
            double dy = _splines[spline].interpolateDerivative(x);
            return dy;
        }
        public List<(double, double)> interpolateRange(double xmin, double xmax, int numPoints)
        {
            if (xmax > _maxX) xmax = _maxX;

            List<(double, double)> points = new List<(double, double)>();
            double x = xmin;
            double y;
            double dx = (xmax - xmin) / numPoints;

            int spline = 0;
            while(x < xmax)
            {
                while(_splines[spline].x2 < x) spline++;
                y = _splines[spline].interpolate(x);
                x += dx;
                points.Add((x, y));
            }

            return points;
        }
        public double invInterpolate(double y)
        {
            int spline = getSplineFromY(y);
            double x = _splines[spline].invInterpolate(y);
            return x;
        }
        public double aucFromX(double x)
        {
            int spline = getSplineFromX(x);
            return _splines[spline].getAUCFromX(x);
        }
        public double aucFromY(double y)
        {
            int spline = getSplineFromY(y);
            return _splines[spline].getAUCFromY(y);
        }

        private Boolean relativeEq(double a, double b)
        {
            return Math.Round(a, 5) == Math.Round(b, 5);
        }
    }

}

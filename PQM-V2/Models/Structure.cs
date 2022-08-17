using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.Models
{
    enum SplineType { None, Cubic, Monotone }
    struct Spline
    {
        public SplineType type;
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
            this.type = SplineType.None;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;

            derivatives = new double[2];
            coefficients = new double[4];
        }
    }


    public class Structure
    {
        private string _filePath;
        private string _metric;
        private string _name;
        private bool _invalidReadFlag;

        private List<Spline> _splines;
        public Structure(string filePath)
        {
            _filePath = filePath;
            _splines = new List<Spline>();

            getSplines();
            getMetricAndName();
        }
        private void getSplines()
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
                foreach (Match match in matches)
                {
                    string value = match.Value.ToString();

                    if (double.TryParse(value, out double newVal))
                    {
                        if (newVal < 0) newVal = 0;
                        newY.Add(newVal);
                        if (newVal < 0)
                        {
                            throw new Exception("Negative Value");
                        }
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
        private void getMetricAndName()
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
                _name = myName;
            }
            else
            {
                _name = last;
            }
        }
        private void setCoeffients()
        {
            int n = _splines.Count + 1;
            int u = 4 * n - 4;

            double[,] A = new double[u, u];
            double[] b = new double[u];

            int row = 0;

            for (int i = 0; i < u; i++)
            {
                for (int j = 0; j < u; j++)
                {
                    A[i, j] = 0;
                }
                b[i] = 0;
            }

            for(int i = 0; i < _splines.Count; i++)
            {
                double x1 = _splines[i].x1;
                double x2 = _splines[i].x2;

                A[row, i * 4] = Math.Pow(x1, 3);
                A[row, i * 4 + 1] = Math.Pow(x1, 2);
                A[row, i * 4 + 2] = x1;
                A[row, i * 4 + 3] = 1;
                b[row] = _splines[i].y1;

                A[row + 1, i * 4] = Math.Pow(x2, 3);
                A[row + 1, i * 4 + 1] = Math.Pow(x2, 2);
                A[row + 1, i * 4 + 2] = x2;
                A[row + 1, i * 4 + 3] = 1;
                b[row + 1] = _splines[i].y2;

                row += 2;
            }

            for (int i = 1; i < n - 1; i++)
            {
                double x1 = _splines[i].x1;
                A[row, i * 4 - 4] = 3 * Math.Pow(x1, 2);
                A[row, i * 4 - 3] = 2 * x1;
                A[row, i * 4 - 2] = 1;
                A[row, i * 4] = -3 * Math.Pow(x1, 2);
                A[row, i * 4 + 1] = -2 * x1;
                A[row, i * 4 + 2] = -1;
                b[row] = 0;

                A[row + 1, i * 4 - 4] = 6 * x1;
                A[row + 1, i * 4 - 3] = 2;
                A[row + 1, i * 4] = -6 * x1;
                A[row + 1, i * 4 + 1] = -2;
                b[row + 1] = 0;

                row += 2;
            }

            A[row, 0] = 6 * X[0];
            A[row, 1] = 2;
            b[row] = 0;

            A[row + 1, u - 4] = 6 * X[n - 1];
            A[row + 1, u - 3] = 2;
            b[row + 1] = 0;

            Matrix<double> matA = Matrix<double>.Build.DenseOfArray(A);
            Vector<double> vecB = Vector<double>.Build.DenseOfArray(b);

            Vector<double> coefficients = matA.Solve(vecB);




        }
    }

}

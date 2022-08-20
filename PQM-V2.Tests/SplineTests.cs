using System;
using PQM_V2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PQM_V2.Tests
{
    public class SplineTests
    {
        [Theory]
        [InlineData(1 , -6, -3, -104, 8)]
        [InlineData(1 , 1, 1, -3, 1)]
        [InlineData(-4, 2, -0.7, -10, -1.17168)]
        public void findRoots_findRootsWithOneSolution(
            double a, double b, double c, double d, double expected)
        {
            // Arrange
            (double? r1, double? r2, double? r3) = Spline.findRoots(a, b, c, d);

            // Act
            double? actual = r1.HasValue ? r1.Value : r2.HasValue ? r2.Value : r3;
            actual = actual.HasValue ? Math.Round(actual.Value, 4) : actual;
            expected = Math.Round(expected, 4);

            // Assert
            Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData(1, 12, 42, 36, -6, -4.7321, -1.2679)]
        [InlineData(2, 16, 10, -100, 2, -5, -5)]
        [InlineData(0.5, 0.5, -10.5, -22.5, -3, -3, 5)]
        [InlineData(-4.7, 1.4, 41.5, 20.7, -2.51591, -0.5244, 3.33819)]
        public void findRoots_findRootsWithThreeSolution(
            double a, double b, double c, double d, double e1, double e2, double e3)
        {
            (double? ab1, double? ab2, double? ab3) = Spline.findRoots(a, b, c, d);

            e1 = Math.Round(e1, 4);
            e2 = Math.Round(e2, 4);
            e3 = Math.Round(e3, 4);

            double a1, a2, a3;

            if (!ab1.HasValue) throw new Exception();
            else a1 = Math.Round(ab1.Value, 4);

            if (!ab2.HasValue) throw new Exception();
            else a2 = Math.Round(ab2.Value, 4);

            if (!ab3.HasValue) throw new Exception();
            else a3 = Math.Round(ab3.Value, 4);

            List<double> expected = new List<double> { a1, a2, a3 };
            List<double> actual = new List<double> { e1, e2, e3 };

            expected.Sort();
            actual.Sort();

            Assert.Equal(expected, actual);
        }
    }
}

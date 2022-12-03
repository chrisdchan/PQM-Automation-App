using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PQM_V2.Tools;
using Xunit;

namespace PQM_V2.Tests
{
    public class csvCheckerTests
    {

        [Theory]
        [InlineData(
            "C:\\Users\\chris\\OneDrive\\Documents\\BIDMC\\AA015 BRAIN 08162012 Case 0 DV\\CD Grey Matter Raw.csv",
            true
            )]
        [InlineData(
            "C:\\Users\\chris\\OneDrive\\Documents\\BIDMC\\AA015 BRAIN 08162012 Case 0 DV\\test1.png",
            false
            )]
        public void checkCSVFiles(string filePath, bool expected)
        {
            bool actual = CSVChecker.isValidFile(filePath);
            Assert.Equal(actual, expected);
        }
    }
}

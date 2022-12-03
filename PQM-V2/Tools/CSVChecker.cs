using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PQM_V2.Tools
{
    public class CSVChecker
    {
        public static string isValidFile(string fileName)
        {
            if (isFileLocked(fileName)) return String.Format("{0} is in use or unable to be opened", fileName);
            if (!isCSV(fileName)) return String.Format("{0} does not have .CSV exension", fileName);
            if (!File.Exists(fileName)) return String.Format("{0} does not exist", fileName);
            if (!hasHeaders(fileName)) return String.Format("{0} is not of valid format", fileName);

            return "passed";
        }
        public static string isValidFile(string[] fileNames)
        {
            if (fileNames.Length == 0) return "No Files Selected";

            foreach(string fileName in fileNames)
            {
                string result = isValidFile(fileName);
                if (result != "passed") return result;
            }
            return "passed";
        }

        public static bool isCSV(string fileName)
        {
            return (Path.GetExtension(fileName) == ".csv");
        }
        public static bool hasHeaders(string fileName)
        {
            HashSet<string> testsPassed = new HashSet<string>();
            int expectedPassed = 6;
            string line;
            Regex regexModel = new Regex("Model");
            Regex regexVersion = new Regex("Version");
            Regex regexDate = new Regex("Date");
            Regex regexTable = new Regex("Table");
            Regex regexX = new Regex("ec.normJ|ec.normE|SAR");
            Regex regexY = new Regex("([0-9}/.]*)");

            int matchCountThreshold = 5;

            using (StreamReader sr = new StreamReader(fileName))
            {
                while((line = sr.ReadLine()) != null)
                {
                    if (regexModel.IsMatch(line))
                    {
                        testsPassed.Add("Model");
                        continue;
                    }
                    if (regexVersion.IsMatch(line))
                    {
                        testsPassed.Add("Version");
                        continue;
                    }
                    if (regexDate.IsMatch(line))
                    {
                        testsPassed.Add("Date");
                        continue;
                    }

                    if (regexTable.IsMatch(line))
                    {
                        testsPassed.Add("Table");
                        continue;
                    }
                        
                    MatchCollection matchesX = regexX.Matches(line);
                    if(matchesX.Count > matchCountThreshold)
                    {
                        testsPassed.Add("X");
                        continue;
                    }

                    MatchCollection matchesY = regexY.Matches(line);
                    if(matchesY.Count > matchCountThreshold)
                    {
                        testsPassed.Add("Y");
                        continue;
                    }
                }
            }
            return (expectedPassed == testsPassed.Count);
        }

        public static bool isFileLocked(string filename)
        {
            FileInfo file = new FileInfo(filename);
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}

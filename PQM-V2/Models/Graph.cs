using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PQM_V2.Models
{
    public class Graph
    {
        private string[] _filepaths;
        private string _title;
        private double _xmax;
        private string _yaxisName;
        private string _xaxisName;
        private bool _multipleMetricsFlag;
        public List<Structure> structures { get; set; }
        public Structure selectedStructure { get; set; }
        public string title => _title;
        public string xaxisName => _xaxisName;
        public string yaxisName => _yaxisName;
        public bool multipleMetricsFlag => _multipleMetricsFlag;
        public double xmax => _xmax;

        public Graph(string[] filepaths)
        {
            _filepaths = filepaths;
            structures = new List<Structure>();
            selectedStructure = null;

            _multipleMetricsFlag = false;

            setStructures();
            setXmax();
            setColors();
            setGraphTitles();
        }

        private void setStructures()
        {
            int index = 0;
            foreach(string filepath in _filepaths)
            {
                if(File.Exists(filepath))
                {
                    Structure structure = new Structure(filepath);
                    structures.Add(structure);
                    structure.index = index;
                    index++;
                }
            }
        }
        public void selectStructure(int index)
        {
            if(selectedStructure != null)
            {
                selectedStructure.selected = false;
            }
            selectedStructure = structures[index];
            selectedStructure.selected = true;
        }
        private void setXmax()
        {
            _xmax = double.MinValue;
            foreach(Structure structure in structures)
            {
                if(_xmax < structure.maxX)
                {
                    _xmax = structure.maxX;
                }
            }
        }
        private void setColors()
        {
            int n = structures.Count;
            int dc = 1530 / n;
            int colorId = 0;

            for (int i = 0; i < n; i++)
            {
                structures[i].color = getColorfromId(colorId);
                colorId += dc;
            }
        }
        private static SolidColorBrush getColorfromId(int id)
        {
            int group = id / 255; // integer division

            int offset = id % 255;

            int R = 0, G = 0, B = 0;

            switch (group)
            {
                case 0:
                    R = 255;
                    G = offset;
                    break;
                case 1:
                    R = 255 - offset;
                    G = 255;
                    break;
                case 2:
                    G = 255;
                    B = offset;
                    break;
                case 3:
                    G = 255 - offset;
                    B = 255;
                    break;
                case 4:
                    R = offset;
                    B = 255;
                    break;
                case 5:
                    R = 255;
                    B = offset;
                    break;
                default:
                    throw new Exception("Invalid group");
            }

            return new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));

        }
        private void setGraphTitles()
        {
            string metric = null;
            foreach(Structure structure in structures)
            {
                if(metric == null)
                {
                    metric = structure.metric;
                }
                else if(metric != structure.metric)
                {
                    _multipleMetricsFlag = true;
                }
            }
            metric = metricToFullName(metric);

            _title = metric;
            _yaxisName = "Normalized Percent Volumne (%)";
            _xaxisName = metric + " (A/m^2)";
        }
        private string metricToFullName(string metric)
        {
            string res;
            if(metric == "CD")
            {
                res = "Current Density";
            }
            else if(metric == "SAR")
            {
                res = "Specific Absorbance Rate";
            }
            else if(metric == "E-field" || metric == "E-Field")
            {
                res = "Electric Field Density";
            }
            else
            {
                throw new ArgumentException("Not a valid metric");
            }

            return res;
        }

        public void isolate(int ind)
        {
            if (ind < 0 || ind > structures.Count) throw new Exception("Index out of range");
            for(int i = 0; i < structures.Count; i++)
            {
                if(i != ind)
                {
                    structures[i].visible = false;
                }
                else
                {
                    structures[i].visible = true;
                }
            }
        }
    }
}

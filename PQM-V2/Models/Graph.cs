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
        public List<Structure> structures { get; set; }

        public Graph(string[] filepaths)
        {
            _filepaths = filepaths;
            structures = new List<Structure>();

            setStructures();
            setColors();
        }

        private void setStructures()
        {
            foreach(string filepath in _filepaths)
            {
                if(File.Exists(filepath))
                    structures.Add(new Structure(filepath));
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



    }
}

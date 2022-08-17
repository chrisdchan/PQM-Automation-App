using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PQM_V2.ViewModels
{
    public class StructuresLegendViewModel
    {
        public IEnumerable<LegendItemViewModel> Structures;

        public StructuresLegendViewModel()
        {

        }
    }

    public class LegendItemViewModel
    {
        public string name { get; set; }
        public SolidColorBrush color { get; set; }
        public bool visible { get; set; }
    }

}

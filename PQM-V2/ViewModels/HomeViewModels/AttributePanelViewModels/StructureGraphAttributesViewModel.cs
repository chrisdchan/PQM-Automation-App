using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.ViewModels.HomeViewModels.AttributePanelViewModels
{
    public class StructureGraphAttributesViewModel : BaseViewModel
    {
        private GraphAttributesStore _graphAttributesStore;
        public double xmin => _graphAttributesStore.xmin;
        public double xmax => _graphAttributesStore.xmax;
        public int pointsPerPlot => _graphAttributesStore.pointsPerPlot;
        public int numXAxisTicks => _graphAttributesStore.numXAxisTicks;
        public int numYAxisTicks => _graphAttributesStore.numYAxisTicks;
        public StructureGraphAttributesViewModel()
        {
            _graphAttributesStore = (Application.Current as App).graphAttributesStore;
        }
    }
}

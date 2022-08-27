using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PQM_V2.Stores
{
    public class GraphAttributesStore
    {
        public event Action plotAttributesChanged;
        public event Action backgroundcolorChanged;
        public event Action axesStructureChanged;
        public event Action graphAttributesChanged;

        private double _xmin;
        private double _xmax;
        private int _pointsPerPlot;
        private int _numXAxisTicks;
        private int _numYAxisTicks;
        private SolidColorBrush _backgroundColor;

        public double xmin
        {
            get => _xmin;
            set { _xmin = value;  onPlotAttributesChanged();}
        }
        public double xmax
        {
            get => _xmax;
            set { _xmax = value; onPlotAttributesChanged(); }
        }
        public int pointsPerPlot
        {
            get => _pointsPerPlot;
            set { _pointsPerPlot = value; onPlotAttributesChanged(); }
        }

        public int numXAxisTicks
        {
            get => _numXAxisTicks;
            set { _numXAxisTicks = value; onAxesStructureChanged(); }
        }
        public int numYAxisTicks
        {
            get => _numYAxisTicks;
            set { _numYAxisTicks = value; onAxesStructureChanged(); }
        }

        public SolidColorBrush backgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; onBackgroundColorChanged(); }
        }

        public GraphAttributesStore()
        {
            _backgroundColor = new SolidColorBrush(Colors.White);
            _xmin = 0;
            _xmax = 40;
            _numXAxisTicks = 5;
            _numYAxisTicks = 5;
            _pointsPerPlot = 100;
        }

        private void onPlotAttributesChanged()
        {
            plotAttributesChanged?.Invoke();
        }

        private void onAxesStructureChanged()
        {
            axesStructureChanged?.Invoke();
        }

        private void onBackgroundColorChanged()
        {
            backgroundcolorChanged?.Invoke();
        }


    }
}

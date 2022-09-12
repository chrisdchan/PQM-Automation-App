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
        public event Action graphAttributesChanged;

        private double _xmin;
        private double _xmax;
        private int _pointsPerPlot;
        private int _numXAxisTicks;
        private int _numYAxisTicks;
        private SolidColorBrush _backgroundColor;
        private SolidColorBrush _axisColor;

        public double xmin
        {
            get => _xmin;
            set { _xmin = value;  }
        }
        public double xmax
        {
            get => _xmax;
            set { _xmax = value;  }
        }
        public int pointsPerPlot
        {
            get => _pointsPerPlot;
            set { _pointsPerPlot = value;  }
        }
        public int numXAxisTicks
        {
            get => _numXAxisTicks;
            set { _numXAxisTicks = value;  }
        }
        public int numYAxisTicks
        {
            get => _numYAxisTicks;
            set { _numYAxisTicks = value;  }
        }

        public SolidColorBrush backgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value;  }
        }
        public SolidColorBrush axisColor
        {
            get => _axisColor;
            set { _axisColor = value;  }
        }

        public GraphAttributesStore()
        {
            _backgroundColor = new SolidColorBrush(Colors.White);
            _axisColor = new SolidColorBrush(Colors.Black);
            _xmin = 0;
            _xmax = 40;
            _numXAxisTicks = 5;
            _numYAxisTicks = 5;
            _pointsPerPlot = 100;
        }
        public void onGraphAttributesChanged()
        {
            graphAttributesChanged?.Invoke();
        }
    }
}

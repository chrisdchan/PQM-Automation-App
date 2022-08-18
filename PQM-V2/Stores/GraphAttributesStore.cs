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
        public event Action domainChanged;
        public event Action backgroundcolorChanged;
        public event Action axesStructureChanged;

        private double _xmin;
        private double _xmax;
        private double _numXAxisTicks;
        private double _numYAxisTicks;
        private SolidColorBrush _backgroundColor;

        public double xmin
        {
            get => _xmin;
            set
            {
                _xmin = value;
                onDomainChange();
            }
        }
        public double xmax
        {
            get => _xmax;
            set
            {
                _xmax = value;
                onDomainChange();
            }
        }

        public double numXAxisTicks
        {
            get => _numXAxisTicks;
            set
            {
                _numXAxisTicks = value;
                onAxesStructureChanged();
            }
        }
        public double numYAxisTicks
        {
            get => _numYAxisTicks;
            set
            {
                _numYAxisTicks = value;
                onAxesStructureChanged();
            }
        }

        public SolidColorBrush backgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                onBackgroundColorChanged();
            }
        }

        private void onDomainChange()
        {
            domainChanged?.Invoke();
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

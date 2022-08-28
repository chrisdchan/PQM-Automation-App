using PQM_V2.Commands;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PQM_V2.ViewModels.HomeViewModels.AttributePanelViewModels
{
    public class GraphAttributesViewModel : BaseViewModel
    {
        private GraphAttributesStore _graphAttributesStore;
        private GraphStore _graphStore;

        private double _xmin;
        private double _xmax;
        private int _pointsPerPlot;
        private int _numXAxisTicks;
        private int _numYAxisTicks;
        private string _backgroundColor;
        private string _axisColor;


        public double xmin { get => _xmin; 
            set { 
                _xmin = value;
                onPropertyChanged(nameof(xmin));
            } }
        public double xmax { get => _xmax; 
            set { _xmax = value;
                onPropertyChanged(nameof(xmax));
            } } 
        public int pointsPerPlot { get => _pointsPerPlot; 
            set { _pointsPerPlot = value;
                onPropertyChanged(nameof(pointsPerPlot));
            } }
        public int numXAxisTicks { get => _numXAxisTicks; 
            set { _numXAxisTicks = value;
                onPropertyChanged(nameof(numXAxisTicks));
            } }
        public int numYAxisTicks { get => _numYAxisTicks; 
            set { _numYAxisTicks = value;
                onPropertyChanged(nameof(numYAxisTicks));
            } }

        public string backgroundColor { get => _backgroundColor; 
            set { _backgroundColor = value;
                onPropertyChanged(nameof(backgroundColor));
            } }
        public string axisColor { get => _axisColor; 
            set { _axisColor = value;
                onPropertyChanged(nameof(axisColor));
            } }

        public string backgroundColorString { get; set; }

        public RelayCommand updateDomainCommand { get; private set; }
        public RelayCommand updateGraphAttributesCommand { get; private set; }
        public RelayCommand updateStyleCommand { get; private set; }

        public GraphAttributesViewModel()
        {
            _graphAttributesStore = (Application.Current as App).graphAttributesStore;
            _graphStore = (Application.Current as App).graphStore;

            xmin = _graphAttributesStore.xmin;
            xmax = _graphAttributesStore.xmax;
            pointsPerPlot = _graphAttributesStore.pointsPerPlot;
            numXAxisTicks = _graphAttributesStore.numXAxisTicks;
            numYAxisTicks = _graphAttributesStore.numYAxisTicks;
            backgroundColor = _graphAttributesStore.backgroundColor.ToString();
            axisColor = _graphAttributesStore.axisColor.ToString();

            updateDomainCommand = new RelayCommand(updateDomain);
            updateGraphAttributesCommand = new RelayCommand(updateGraphAttributes);
            updateStyleCommand = new RelayCommand(updateStyle);
        }
        private void updateDomain(object _)
        {
            if(_xmin < 0 || _graphStore.graph.xmax < _xmax)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                _graphAttributesStore.xmin = _xmin;
                _graphAttributesStore.xmax = _xmax;
                _graphAttributesStore.onGraphAttributesChanged();
            }
        }

        public void updateGraphAttributes(object _)
        {
            _graphAttributesStore.numXAxisTicks = _numXAxisTicks;
            _graphAttributesStore.numYAxisTicks = _numYAxisTicks;
            _graphAttributesStore.pointsPerPlot = _pointsPerPlot;
            _graphAttributesStore.onGraphAttributesChanged();
        }

        public void updateStyle(object _)
        {
            _graphAttributesStore.backgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_backgroundColor));
            _graphAttributesStore.axisColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_axisColor));
;
            _graphAttributesStore.onGraphAttributesChanged();
        }
    }
}

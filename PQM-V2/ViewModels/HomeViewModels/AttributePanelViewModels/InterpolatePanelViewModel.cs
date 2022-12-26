using PQM_V2.Commands;
using PQM_V2.Models;
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
    public class InterpolatePanelViewModel : BaseViewModel
    {
        private GraphStore _graphStore;
        private CanvasStore _canvasStore;
        private Graph _graph => _graphStore.graph;
        private Structure _selectedStructure;

        private string _interpolateX;
        private double _yFromX;
        private double _dyFromX;
        private double _AUCFromX;

        private string _interpolateY;
        private double _xFromY;
        private double _dyFromY;
        private double _AUCFromY;

        private bool _showInterpolateXError;
        private string _interpolateXError;
        private bool _showInterpolateYError;
        private string _interpolateYError;

        private string _color;
        private double _lineThickness;

        public RelayCommand updateStyleCommand { get; set; }
        public RelayCommand changeSelectedStructureCommand { get; set; }
        public RelayCommand updateProbeCommand { get; set; }

        public Structure selectedStructure
        {
            get => _selectedStructure;
            set { _selectedStructure = value; onPropertyChanged(nameof(selectedStructure)); }
        }

        public string interpolateX
        {
            get => _interpolateX;
            set { _interpolateX = value; setFromX(); }
        }
        public double yFromX
        {
            get => _yFromX;
            set { _yFromX = value; onPropertyChanged(nameof(yFromX)); }
        }
        public double dyFromX
        {
            get => _dyFromX;
            set { _dyFromX = value; onPropertyChanged(nameof(dyFromX)); }
        }
        public double AUCFromX
        {
            get => _AUCFromX;
            set { _AUCFromX = value; onPropertyChanged(nameof(AUCFromX)); }
        }
        public string interpolateY
        {
            get => _interpolateY;
            set { _interpolateY = value; setFromY(); }
        }
        public double xFromY
        {
            get => _xFromY;
            set { _xFromY = value; onPropertyChanged(nameof(xFromY)); }
        }
        public double dyFromY
        {
            get => _dyFromY;
            set { _dyFromY = value; onPropertyChanged(nameof(dyFromY)); }
        }
        public double AUCFromY
        {
            get => _AUCFromY;
            set { _AUCFromY = value; onPropertyChanged(nameof(AUCFromY)); }
        }
        public bool showInterpolateXError
        {
            get => _showInterpolateXError;
            set { _showInterpolateXError = value; onPropertyChanged(nameof(showInterpolateXError)); }
        }
        public bool showInterpolateYError
        {
            get => _showInterpolateYError;
            set { _showInterpolateYError = value; onPropertyChanged(nameof(showInterpolateYError)); }
        }
        public string interpolateXError
        {
            get => _interpolateXError;
            set { _interpolateXError = value; onPropertyChanged(nameof(interpolateXError)); }
        }
        public string interpolateYError
        {
            get => _interpolateYError;
            set { _interpolateYError = value; onPropertyChanged(nameof(interpolateYError)); }
        }
        public string color
        {
            get => _color;
            set { _color = value; onPropertyChanged(nameof(color)); }
        }
        public double lineThickness
        {
            get => _lineThickness;
            set { _lineThickness = value; onPropertyChanged(nameof(lineThickness)); }
        }
        public InterpolatePanelViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;
            _canvasStore = (Application.Current as App).canvasStore;
            _selectedStructure = _graph.selectedStructure;

            _graphStore.selectedStructureChanged += onSelectedStructureChanged;

            updateStyleCommand = new RelayCommand(updateStyle);
            changeSelectedStructureCommand = new RelayCommand(changeSelectedStructure);
            updateProbeCommand = new RelayCommand(updateProbe);
        }
        ~InterpolatePanelViewModel()
        {

        }
        public override void Dispose()
        {
            _graphStore.selectedStructureChanged -= onSelectedStructureChanged;
        }

        private void onSelectedStructureChanged()
        {
            selectedStructure = _graph.selectedStructure;
            lineThickness = selectedStructure.lineThickness;
            color = selectedStructure.color.ToString();
        }

        private void setFromX()
        {
            showInterpolateXError = true;
            yFromX = double.NaN;
            dyFromX = double.NaN;
            AUCFromX = double.NaN;

            double xInput; 

            if(interpolateX == "")
            {
                showInterpolateXError = false;
            }
            else if(!double.TryParse(interpolateX, out xInput))
            {
                interpolateXError = "Must be a number";
            }
            else if (_selectedStructure == null)
            {
                interpolateXError = "No Structure Selected";
            }
            else if (xInput < 0)
            {
                interpolateXError = "Value cannot be less than 0";
            }
            else if (_selectedStructure.maxX < xInput)
            {
                interpolateXError = String.Format("Value cannot be greater than {0}", _selectedStructure.maxX);
            }
            else
            {
                showInterpolateXError = false;
                yFromX = _selectedStructure.interpolate(xInput);
                dyFromX = _selectedStructure.interpolateDerivative(xInput);
                AUCFromX = _selectedStructure.aucFromX(xInput);
            }
        }
        private void setFromY()
        {
            showInterpolateYError = true;
            xFromY = double.NaN;
            dyFromY = double.NaN;
            AUCFromY = double.NaN;

            double yInput; 

            if(interpolateY == "")
            {
                showInterpolateYError = false;
            }
            else if(!double.TryParse(interpolateY, out yInput))
            {
                interpolateYError = "Must be a number";
            }
            else if (_selectedStructure == null)
            {
                interpolateYError = "No Structure Selected";
            }
            else if (yInput < 0)
            {
                interpolateYError = "Value cannot be less than 0";
            }
            else if (yInput > 100)
            {
                interpolateYError = "Value cannot be greater than 100";
            }
            else
            {
                showInterpolateYError = false;
                xFromY = _selectedStructure.invInterpolate(yInput);
                dyFromY = _selectedStructure.interpolateDerivative(xFromY);
                AUCFromY = _selectedStructure.aucFromX(xFromY);
            }
        }
        private void changeSelectedStructure(object param)
        {
            if((string)param == "left")
            {
                _graphStore.graph.newSelectedStructureFromOffset(-1);
            }
            else if((string)param == "right")
            {
                _graphStore.graph.newSelectedStructureFromOffset(1);
            }
            _graphStore.onSelectedStructureChanged();
        }
        private void updateStyle(object _)
        {
            if (_selectedStructure != null)
            {
                selectedStructure.color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                selectedStructure.lineThickness = lineThickness;

                _graphStore.onGraphUpdated();
                onPropertyChanged(nameof(selectedStructure));
            }
        }
        private void updateProbe(object message)
        {
            switch ((string)message)
            {
                case "None":
                    _canvasStore.probeType = CanvasStore.ProbeTypes.none;
                    break;
                case "X":
                    _canvasStore.probeType = CanvasStore.ProbeTypes.x;
                    break;
                case "Y":
                    _canvasStore.probeType = CanvasStore.ProbeTypes.y;
                    break;

            }
        }
    }
}

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
    public class StructureAttributesViewModel : BaseViewModel
    {
        private GraphStore _graphStore;
        private Graph _graph => _graphStore.graph;
        private Structure _selectedStructure;

        private double _interpolateX;
        private double _yFromX;
        private double _dyFromX;
        private double _AUCFromX;

        private double _interpolateY;
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

        public Structure selectedStructure {
            get => _selectedStructure;
            set { _selectedStructure = value; onPropertyChanged(nameof(selectedStructure)); }
        }

        public double interpolateX {
            get => _interpolateX;
            set { _interpolateX = value; setFromX(); }}
        public double yFromX {
            get => _yFromX;
            set { _yFromX = value; onPropertyChanged(nameof(yFromX)); }}
        public double dyFromX {
            get => _dyFromX;
            set { _dyFromX = value; onPropertyChanged(nameof(dyFromX)); }}
        public double AUCFromX{ 
            get => _AUCFromX;
            set { _AUCFromX = value; onPropertyChanged(nameof(AUCFromX)); }}

        public double interpolateY {
            get => _interpolateY;
            set { _interpolateY = value; setFromY(); }}
        public double xFromY {
            get => _xFromY;
            set { _xFromY = value; onPropertyChanged(nameof(xFromY)); }}
        public double dyFromY {
            get => _dyFromY;
            set { _dyFromY = value; onPropertyChanged(nameof(dyFromY)); }}
        public double AUCFromY{ 
            get => _AUCFromY;
            set { _AUCFromY = value; onPropertyChanged(nameof(AUCFromY)); }}
        public bool showInterpolateXError{ 
            get => _showInterpolateXError;
            set { _showInterpolateXError = value; onPropertyChanged(nameof(showInterpolateXError)); }}
        public bool showInterpolateYError{ 
            get => _showInterpolateYError;
            set { _showInterpolateYError = value; onPropertyChanged(nameof(showInterpolateYError)); }}
        public string interpolateXError{ 
            get => _interpolateXError;
            set { _interpolateXError = value; onPropertyChanged(nameof(interpolateXError)); }}
        public string interpolateYError{ 
            get => _interpolateYError;
            set { _interpolateYError = value; onPropertyChanged(nameof(interpolateYError)); }}

        public string color{ 
            get => _color;
            set { _color = value; onPropertyChanged(nameof(color)); }}
        public double lineThickness{ 
            get => _lineThickness;
            set { _lineThickness = value; onPropertyChanged(nameof(lineThickness)); }}

        public StructureAttributesViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;
            _selectedStructure = _graph.selectedStructure;

            _graphStore.selectedStructureChanged += onSelectedStructureChanged;

            updateStyleCommand = new RelayCommand(updateStyle);
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
            if(_selectedStructure == null)
            {
                interpolateXError = "No Structure Selected";
            }
            else if (_interpolateX < 0)
            {
                interpolateXError = "Value cannot be less than 0";
            }
            else if(_selectedStructure.maxX < _interpolateX)
            {
                interpolateXError = String.Format("Value cannot be greater than {0}", _selectedStructure.maxX);
            }
            else
            {
                showInterpolateXError = false;
                yFromX = _selectedStructure.interpolate(_interpolateX);
                dyFromX = _selectedStructure.interpolateDerivative(_interpolateX);
                AUCFromX = _selectedStructure.aucFromX(_interpolateX);
            }
        }
        private void setFromY()
        {
            showInterpolateYError = true;
            xFromY = double.NaN;
            dyFromY = double.NaN;
            AUCFromY = double.NaN;

            if(_selectedStructure == null)
            {
                interpolateYError = "No Structure Selected";
            }
            else if(_interpolateY < 0)
            {
                interpolateYError = "Value cannot be less than 0";
            }
            else if(_interpolateY > 100)
            {
                interpolateYError = "Value cannot be greater than 100";
            }
            else
            {
                showInterpolateYError= false;
                xFromY = _selectedStructure.invInterpolate(_interpolateY);
                dyFromY = _selectedStructure.interpolateDerivative(xFromY);
                AUCFromY = _selectedStructure.aucFromX(xFromY);
            }

        }

        private void updateStyle(object _)
        {
            if(_selectedStructure != null)
            {
                selectedStructure.color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                selectedStructure.lineThickness = lineThickness;

                _graphStore.onGraphUpdated();
                onPropertyChanged(nameof(selectedStructure));
            }
        }
    }
}

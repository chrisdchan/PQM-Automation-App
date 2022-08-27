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
        private SelectedStructureStore _selectedStructureStore;

        private Graph _graph => _graphStore.graph;
        private Structure _currentStructure;

        private double _interpolateX;
        private double _yFromX;
        private double _dyFromX;
        private double _AUCFromX;

        private double _interpolateY;
        private double _xFromY;
        private double _dyFromY;
        private double _AUCFromY;

        private SolidColorBrush _color;
        private double _lineThickness;

        public string structureName => _currentStructure.name;
        public SolidColorBrush structureColor => _currentStructure.color;

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

        public SolidColorBrush color{ 
            get => _color;
            set { _color = value; }}
        public double lineThickness{ 
            get => _lineThickness;
            set { _lineThickness = value; }}
        

        public StructureAttributesViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;
            _selectedStructureStore = (Application.Current as App).selectedStructureStore;

            _currentStructure = _graph.structures[0];
        }

        private void setFromX()
        {
            if (_interpolateX < 0 || _currentStructure.maxX < _interpolateX)
            {
                _interpolateX = 0;
            }
            else
            {
                yFromX = _currentStructure.interpolate(_interpolateX);
                dyFromX = _currentStructure.interpolateDerivative(_interpolateX);
                AUCFromX = _currentStructure.aucFromX(_interpolateX);
            }
        }
        private void setFromY()
        {
            xFromY = _currentStructure.interpolate(_interpolateY);
            dyFromY = _currentStructure.interpolateDerivative(_interpolateY);
            AUCFromY = _currentStructure.aucFromY(_interpolateY);
        }
    }
}

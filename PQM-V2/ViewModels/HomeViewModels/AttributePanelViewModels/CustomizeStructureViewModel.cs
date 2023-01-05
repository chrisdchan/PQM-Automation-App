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
    public class CustomizeStructureViewModel : BaseViewModel
    {
        private GraphCustomizeStore _graphCustomizeStore;
        private GraphStore _graphStore;

        private Structure _selectedStructure;
        private string _colorSelect;

        private int _lineType;
        private double _lineThickness;
        private double _dashLength;
        private double _dotGapLength;
        private double _dotRadius;

        private bool _lineThicknessVisible;
        private bool _dashLengthVisible;
        private bool _dotGapLengthVisible;
        private bool _dotRadiusVisible;

        private string _selectStructureError;
        private bool _showLineTypeError;
        private string _lineTypeError;

        
        public Structure selectedStructure { get => _selectedStructure; set { _selectedStructure = value; onPropertyChanged(nameof(selectedStructure));  }}
        public string colorSelect { get => _colorSelect; set {
                _colorSelect = value;
                selectStructureError = null;
                onPropertyChanged(nameof(colorSelect)); 
            } }
        public int lineType { get => _lineType; set { _lineType = value; onPropertyChanged(nameof(lineType)); } }
        public string selectStructureError { get => _selectStructureError; set { _selectStructureError = value; onPropertyChanged(nameof(selectStructureError));  }}
        public string lineTypeError { get => _lineTypeError; set { _lineTypeError = value; onPropertyChanged(nameof(lineTypeError));  }}
        public bool showLineTypeError { get => _showLineTypeError; set { _showLineTypeError = value; onPropertyChanged(nameof(showLineTypeError));  }}

        public double lineThickness { get => _lineThickness; set { _lineThickness = value; onPropertyChanged(nameof(lineThickness)); }}
        public double dashLength { get => _dashLength; set { _dashLength = value; onPropertyChanged(nameof(dashLength)); } }
        public double dotGapLength { get => _dotGapLength; set { _dotGapLength = value; onPropertyChanged(nameof(dotGapLength)); } }
        public double dotRadius { get => _dotRadius; set { _dotRadius = value; onPropertyChanged(nameof(dotRadius)); } }

        public bool lineThicknessVisible { get => _lineThicknessVisible; set { _lineThicknessVisible = value; onPropertyChanged(nameof(lineThicknessVisible)); }}
        public bool dashLengthVisible { get => _dashLengthVisible; set { _dashLengthVisible = value; onPropertyChanged(nameof(dashLengthVisible)); } }
        public bool dotGapLengthVisible { get => _dotGapLengthVisible; set { _dotGapLengthVisible = value; onPropertyChanged(nameof(dotGapLengthVisible)); } }
        public bool dotRadiusVisible { get => _dotRadiusVisible; set { _dotRadiusVisible = value; onPropertyChanged(nameof(dotRadiusVisible)); } }

        public RelayCommand updateCommand { get; private set; }
        public RelayCommand changeStructureCommand { get; private set; }
        public RelayCommand changeLineTypeInputCommand { get; private set; }

        public CustomizeStructureViewModel()
        {
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;
            _graphStore = (Application.Current as App).graphStore;

            colorSelect = (selectedStructure != null) ? selectedStructure.color.ToString() : null;
            lineType = (selectedStructure == null) ? -1 : (int)selectedStructure.lineType;

            lineThickness = (selectedStructure != null) ? selectedStructure.lineThickness : 0;
            dashLength = (selectedStructure != null) ? selectedStructure.dashLength : 0;
            dotRadius = (selectedStructure != null) ? selectedStructure.dotRadius : 0;
            dotGapLength = (selectedStructure != null) ? selectedStructure.dotGapLength : 0;

            _graphStore.selectedStructureChanged += OnStructureChanged;

            updateCommand = new RelayCommand(update);
            changeStructureCommand = new RelayCommand(changeStructure);
            changeLineTypeInputCommand = new RelayCommand(changeLineTypeInput);
        }

        private void OnStructureChanged()
        {
            selectedStructure = _graphStore.graph.selectedStructure;
            colorSelect = selectedStructure.color.ToString();
            lineThickness = selectedStructure.lineThickness;
            lineType = (int)selectedStructure.lineType;
        }
        private void changeStructure(object param)
        {
            if((string)param == "left")
            {
                _graphStore.graph.newSelectedStructureFromOffset(-1);
                _graphStore.onSelectedStructureChanged();
            }
            else if((string)param == "right")
            {
                _graphStore.graph.newSelectedStructureFromOffset(1);
                _graphStore.onSelectedStructureChanged();
            }
        }
        private void update(object _)
        {
            if (selectedStructure == null)
            {
                selectStructureError = "Please Select a Structure";
            }
            else if(lineType == -1)
            {
                showLineTypeError = true;
                lineTypeError = "Please Select a LineType";
            }
            else
            {
                selectedStructure.color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorSelect));
                selectedStructure.lineThickness = lineThickness;
                selectedStructure.lineType = (LineType)Enum.ToObject(typeof(LineType), lineType);
                selectedStructure.dashLength = _dashLength;
                selectedStructure.dotGapLength = _dotGapLength;
                selectedStructure.dotRadius = _dotRadius;
                _graphStore.onGraphUpdated();
                _graphStore.onSelectedStructureChanged();
            }
        }
        private void changeLineTypeInput(object _)
        {
            setTextBoxes((LineType)Enum.ToObject(typeof(LineType), lineType));
        }
        private void clearTextBoxes()
        {
            lineThicknessVisible = false;
            dashLengthVisible = false;
            dotGapLengthVisible = false;
            dotRadiusVisible = false;
        }

        private void setTextBoxes(LineType lineType)
        {
            clearTextBoxes();
            switch (lineType)
            {
                case LineType.solid:
                    lineThicknessVisible = true;
                    break;
                case LineType.dashed:
                    lineThicknessVisible = true;
                    dashLengthVisible = true;
                    break;
                case LineType.dotted:
                    dotRadiusVisible = true;
                    dotGapLengthVisible = true;
                    break;
            }
        }
    }
}

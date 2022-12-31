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
        private double _lineThickness;
        private int _lineType;

        private bool _showColorError;
        private string _colorError;
        private bool _showLineTypeError;
        private string _lineTypeError;
        
        public Structure selectedStructure { get => _selectedStructure; set { _selectedStructure = value; onPropertyChanged(nameof(selectedStructure));  }}
        public string colorSelect { get => _colorSelect; set {
                _colorSelect = value;
                showColorError = false;
                onPropertyChanged(nameof(colorSelect)); 
            } }
        public double lineThickness { get => _lineThickness; set { _lineThickness = value; onPropertyChanged(nameof(lineThickness)); }}
        public int lineType { get => _lineType; set { _lineType = value; onPropertyChanged(nameof(lineType)); } }
        public string colorError { get => _colorError; set { _colorError = value; onPropertyChanged(nameof(colorError));  }}
        public bool showColorError { get => _showColorError; set { _showColorError = value; onPropertyChanged(nameof(showColorError));  }}
        public string lineTypeError { get => _lineTypeError; set { _lineTypeError = value; onPropertyChanged(nameof(lineTypeError));  }}
        public bool showLineTypeError { get => _showLineTypeError; set { _showLineTypeError = value; onPropertyChanged(nameof(showLineTypeError));  }}

        public RelayCommand updateCommand { get; private set; }
        public RelayCommand changeStructureCommand { get; private set; }

        public CustomizeStructureViewModel()
        {
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;
            _graphStore = (Application.Current as App).graphStore;

            colorSelect = (selectedStructure != null) ? selectedStructure.color.ToString() : null;
            lineThickness = (selectedStructure != null) ? selectedStructure.lineThickness : 0;
            lineType = (selectedStructure == null) ? -1 : (int)selectedStructure.lineType;

            _graphStore.selectedStructureChanged += OnStructureChanged;

            updateCommand = new RelayCommand(update);
            changeStructureCommand = new RelayCommand(changeStructure);
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
                showColorError = true;
                colorError = "Please Select a Structure";
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
                _graphStore.onGraphUpdated();
                _graphStore.onSelectedStructureChanged();
            }
        }
    }
}

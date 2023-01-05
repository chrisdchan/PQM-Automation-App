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
    public class CustomizePanelViewModel : BaseViewModel
    {
        private GraphCustomizeStore _graphCustomizeStore;
        private GraphStore _graphStore;
        public RelayCommand changeSelectedStructureCommand { get; set; }

        private int _numXAxisTicks;
        private int _numYAxisTicks;
        private int _axesThickness;
        private int _numPoints;
        private int _axesTickSize;
        private string _axesError;

        private int _titleSize;
        private int _titleLeftOffset;
        private int _titleTopOffset;
        private bool _titleBold;
        private bool _titleItalic;
        private string _titleError;

        private int _xAxisTitleSize;
        private int _xAxisTitleLeftOffset;
        private int _xAxisTitleTopOffset;
        private bool _xAxisTitleBold;
        private bool _xAxisTitleItalic;
        private string _xAxisTitleError;

        private int _yAxisTitleSize;
        private int _yAxisTitleLeftOffset;
        private int _yAxisTitleTopOffset;
        private bool _yAxisTitleBold;
        private bool _yAxisTitleItalic;
        private string _yAxisTitleError;

        private int _legendSize;
        private string _legendError;

        private string _backgroundColor;
        private string _foregroundColor;

        private bool _showSelectedStructureColorError;
        private string _selectedStructureColorError;

        public int numXAxisTicks { get => _numXAxisTicks; set { _numXAxisTicks = value; onPropertyChanged(nameof(numXAxisTicks)); } }
        public int numYAxisTicks { get => _numYAxisTicks; set { _numYAxisTicks = value; onPropertyChanged(nameof(numYAxisTicks)); } }
        public int axesThickness { get => _axesThickness; set { _axesThickness = value; onPropertyChanged(nameof(axesThickness)); } }
        public int axesTickSize { get => _axesTickSize; set { _axesTickSize = value; onPropertyChanged(nameof(axesTickSize)); } }
        public int numPoints { get => _numPoints; set { _numPoints = value; onPropertyChanged(nameof(numPoints)); } }

        public int titleSize { get => _titleSize; set { _titleSize = value; onPropertyChanged(nameof(titleSize)); } }
        public int titleLeftOffset { get => _titleLeftOffset; set { _titleLeftOffset = value; onPropertyChanged(nameof(titleLeftOffset)); } }
        public int titleTopOffset { get => _titleTopOffset; set { _titleTopOffset = value; onPropertyChanged(nameof(titleTopOffset)); } }
        public bool titleBold { get => _titleBold; set { _titleBold = value; onPropertyChanged(nameof(titleBold)); } }
        public bool titleItalic { get => _titleItalic; set { _titleItalic = value; onPropertyChanged(nameof(titleItalic)); } }

        public int xAxisTitleSize { get => _xAxisTitleSize; set { _xAxisTitleSize = value; onPropertyChanged(nameof(xAxisTitleSize)); } }
        public int xAxisTitleLeftOffset { get => _xAxisTitleLeftOffset; set { _xAxisTitleLeftOffset = value; onPropertyChanged(nameof(xAxisTitleLeftOffset)); } }
        public int xAxisTitleTopOffset { get => _xAxisTitleTopOffset; set { _xAxisTitleTopOffset = value; onPropertyChanged(nameof(xAxisTitleTopOffset)); } }
        public bool xAxisTitleBold { get => _xAxisTitleBold; set { _xAxisTitleBold = value; onPropertyChanged(nameof(xAxisTitleBold)); } }
        public bool xAxisTitleItalic { get => _xAxisTitleItalic; set { _xAxisTitleItalic = value; onPropertyChanged(nameof(xAxisTitleItalic)); } }

        public int yAxisTitleSize { get => _yAxisTitleSize; set { _yAxisTitleSize = value; onPropertyChanged(nameof(yAxisTitleSize)); } }
        public int yAxisTitleLeftOffset { get => _yAxisTitleLeftOffset; set { _yAxisTitleLeftOffset = value; onPropertyChanged(nameof(yAxisTitleLeftOffset)); } }
        public int yAxisTitleTopOffset { get => _yAxisTitleTopOffset; set { _yAxisTitleTopOffset = value; onPropertyChanged(nameof(yAxisTitleTopOffset)); } }
        public bool yAxisTitleBold { get => _yAxisTitleBold; set { _yAxisTitleBold = value; onPropertyChanged(nameof(yAxisTitleBold)); } }
        public bool yAxisTitleItalic { get => _yAxisTitleItalic; set { _yAxisTitleItalic = value; onPropertyChanged(nameof(yAxisTitleItalic)); } }

        public int legendSize { get => _legendSize; set { _legendSize = value; onPropertyChanged(nameof(legendSize)); } }

        public string backgroundColor { get => _backgroundColor; set { _backgroundColor = value; onPropertyChanged(nameof(backgroundColor)); } }
        public string foregroundColor { get => _foregroundColor; set { _foregroundColor = value; onPropertyChanged(nameof(foregroundColor)); } }

        public bool showSelectedStructureColorError { get => _showSelectedStructureColorError; set { _showSelectedStructureColorError = value; onPropertyChanged(nameof(showSelectedStructureColorError)); } }
        public string selectedStructureColorError { get => _selectedStructureColorError; set { _selectedStructureColorError = value; onPropertyChanged(nameof(selectedStructureColorError)); } }

        public string axesError { get => _axesError; set { _axesError = value; onPropertyChanged(nameof(axesError)); } }
        public string titleError { get => _titleError; set { _titleError = value; onPropertyChanged(nameof(titleError)); } }
        public string xAxisTitleError { get => _xAxisTitleError; set { _xAxisTitleError = value; onPropertyChanged(nameof(xAxisTitleError)); } }
        public string yAxisTitleError { get => _yAxisTitleError; set { _yAxisTitleError = value; onPropertyChanged(nameof(yAxisTitleError)); } }
        public string legendError { get => _legendError; set { _legendError = value; onPropertyChanged(nameof(legendError)); } }


        public RelayCommand updateLegendSettingsCommand { get; private set; }
        public RelayCommand updateGraphColorSettingsCommand { get; private set; }
        public RelayCommand updateAxisSettingsCommand { get; private set; }
        public RelayCommand updateXAxisTitleSettingsCommand { get; private set; }
        public RelayCommand updateYAxisTitleSettingsCommand { get; private set; }
        public RelayCommand updateTitleSettingsCommand { get; private set; }

        public CustomizePanelViewModel()
        {
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;
            _graphStore = (Application.Current as App).graphStore;

            numXAxisTicks = _graphCustomizeStore.numXAxisTicks;
            numYAxisTicks = _graphCustomizeStore.numYAxisTicks;
            axesThickness = _graphCustomizeStore.axesThickness;
            numPoints = _graphCustomizeStore.numPoints;
            axesTickSize = _graphCustomizeStore.axesTickSize;

            titleSize = _graphCustomizeStore.titleSize;
            titleLeftOffset = _graphCustomizeStore.titleLeftOffset;
            titleTopOffset = _graphCustomizeStore.titleTopOffset;
            titleBold = _graphCustomizeStore.titleBold;
            titleItalic = _graphCustomizeStore.titleItalic;

            xAxisTitleSize = _graphCustomizeStore.xAxisTitleSize;
            xAxisTitleLeftOffset = _graphCustomizeStore.xAxisTitleLeftOffset;
            xAxisTitleTopOffset = _graphCustomizeStore.xAxisTitleTopOffset;
            xAxisTitleBold = _graphCustomizeStore.xAxisTitleBold;
            xAxisTitleItalic = _graphCustomizeStore.xAxisTitleItalic;

            yAxisTitleSize = _graphCustomizeStore.yAxisTitleSize;
            yAxisTitleLeftOffset = _graphCustomizeStore.yAxisTitleLeftOffset;
            yAxisTitleTopOffset = _graphCustomizeStore.yAxisTitleTopOffset;
            yAxisTitleBold = _graphCustomizeStore.yAxisTitleBold;
            yAxisTitleItalic = _graphCustomizeStore.yAxisTitleItalic;

            legendSize = _graphCustomizeStore.legendSize;

            foregroundColor = _graphCustomizeStore.foregroundColor;
            backgroundColor = _graphCustomizeStore.backgroundColor;

            axesError = null;
            titleError = null;
            xAxisTitleError = null;
            yAxisTitleError = null;
            legendError = null;

            updateAxisSettingsCommand = new RelayCommand(updateAxesSettings);
            updateLegendSettingsCommand = new RelayCommand(updateLegendSettings);
            updateGraphColorSettingsCommand = new RelayCommand(updateGraphColorSettings);
            updateXAxisTitleSettingsCommand = new RelayCommand(updateXAxisTitleSettings);
            updateYAxisTitleSettingsCommand = new RelayCommand(updateYAxisTitleSettings);
            updateTitleSettingsCommand = new RelayCommand(updateTitleSettings);
        }

        private void updateAxesSettings(object _)
        {
            if(numXAxisTicks == 0 ) { axesError = "Axes ticks cannot be 0"; return; }
            if(numYAxisTicks == 0 ) { axesError = "Axes ticks cannot be 0"; return; }
            if(numXAxisTicks < 0 ) { axesError = "Axes ticks cannot be negative"; return; }
            if(numYAxisTicks < 0 ) { axesError = "Axes ticks cannot be negative"; return; }
            if(axesThickness <= 0) { axesError = "Axes thickness must be 1 or greater"; return; }
            if(numPoints <= 1) { axesError = "Number of points must be 2 or greater"; return; }
            if(numPoints > 1000) { axesError = "Too many points will make the app slow!"; return; }

            _graphCustomizeStore.numXAxisTicks = numXAxisTicks;
            _graphCustomizeStore.numYAxisTicks = numYAxisTicks;
            _graphCustomizeStore.axesThickness = axesThickness;
            _graphCustomizeStore.axesTickSize = axesTickSize;
            _graphCustomizeStore.numPoints = numPoints;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }
        private void updateTitleSettings(object _)
        {
            if(titleSize <= 0) { titleError = "Title size must be greater than 0"; return; }

            _graphCustomizeStore.titleSize = _titleSize;
            _graphCustomizeStore.titleLeftOffset = _titleLeftOffset;
            _graphCustomizeStore.titleTopOffset = _titleTopOffset;
            _graphCustomizeStore.titleBold = _titleBold;
            _graphCustomizeStore.titleItalic = _titleItalic;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }
        private void updateXAxisTitleSettings(object _)
        {
            if(xAxisTitleSize <= 0) { xAxisTitleError = "Title size must be greater than 0"; return; }

            _graphCustomizeStore.xAxisTitleSize = _xAxisTitleSize;
            _graphCustomizeStore.xAxisTitleLeftOffset = _xAxisTitleLeftOffset;
            _graphCustomizeStore.xAxisTitleTopOffset = _xAxisTitleTopOffset;
            _graphCustomizeStore.xAxisTitleBold = _xAxisTitleBold;
            _graphCustomizeStore.xAxisTitleItalic = _xAxisTitleItalic;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }
        private void updateYAxisTitleSettings(object _)
        {
            if(yAxisTitleSize <= 0) { yAxisTitleError = "Title size must be greater than 0"; return; }

            _graphCustomizeStore.yAxisTitleSize = _yAxisTitleSize;
            _graphCustomizeStore.yAxisTitleLeftOffset = _yAxisTitleLeftOffset;
            _graphCustomizeStore.yAxisTitleTopOffset = _yAxisTitleTopOffset;
            _graphCustomizeStore.yAxisTitleBold = _yAxisTitleBold;
            _graphCustomizeStore.yAxisTitleItalic = _yAxisTitleItalic;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }
        private void updateLegendSettings(object _)
        {
            if(legendSize <= 0) { legendError = "Legend size must be greater than 0"; return; }

            _graphCustomizeStore.legendSize = _legendSize;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }
        private void updateGraphColorSettings(object _)
        {
            _graphCustomizeStore.backgroundColor = _backgroundColor;
            _graphCustomizeStore.foregroundColor = _foregroundColor;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }
    }
}

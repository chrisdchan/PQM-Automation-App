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
    public class CustomizePanelViewModel : BaseViewModel
    {
        private GraphCustomizeStore _graphCustomizeStore;

        private int _numXAxisTicks;
        private int _numYAxisTicks;
        private int _axesThickness;
        private int _numPoints;
        private int _axesTickFontSize;

        private int _titleFontSize;
        private int _titleLeftOffset;
        private int _titleTopOffset;

        private int _xAxisTitleFontSize;
        private int _xAxisTitleLeftOffset;
        private int _xAxisTitleTopOffset;

        private int _yAxisTitleFontSize;
        private int _yAxisTitleLeftOffset;
        private int _yAxisTitleTopOffset;

        private int _legendFontSize;

        private string _backgroundColor;
        private string _foregroundColor;

        public int numXAxisTicks { get => _numXAxisTicks; set { _numXAxisTicks = value; onPropertyChanged(nameof(numXAxisTicks)); } }
        public int numYAxisTicks { get => _numYAxisTicks; set { _numYAxisTicks = value; onPropertyChanged(nameof(numYAxisTicks)); } }
        public int axesThickness { get => _axesThickness; set { _axesThickness = value; onPropertyChanged(nameof(axesThickness)); } }
        public int axesTickFontSize { get => _axesTickFontSize; set { _axesTickFontSize = value; onPropertyChanged(nameof(axesTickFontSize)); } }
        public int numPoints { get => _numPoints; set { _numPoints = value; onPropertyChanged(nameof(numPoints)); } }

        public int titleFontSize { get => _titleFontSize; set { _titleFontSize = value; onPropertyChanged(nameof(titleFontSize)); } }
        public int titleLeftOffset { get => _titleLeftOffset; set { _titleLeftOffset = value; onPropertyChanged(nameof(titleLeftOffset)); } }
        public int titleTopOffset { get => _titleTopOffset; set { _titleTopOffset = value; onPropertyChanged(nameof(titleTopOffset)); } }

        public int xAxisTitleFontSize { get => _xAxisTitleFontSize; set { _xAxisTitleFontSize = value; onPropertyChanged(nameof(xAxisTitleFontSize)); } }
        public int xAxisTitleLeftOffset { get => _xAxisTitleLeftOffset; set { _xAxisTitleLeftOffset = value; onPropertyChanged(nameof(xAxisTitleLeftOffset)); } }
        public int xAxisTitleTopOffset { get => _xAxisTitleTopOffset; set { _xAxisTitleTopOffset = value; onPropertyChanged(nameof(xAxisTitleTopOffset)); } }

        public int yAxisTitleFontSize { get => _yAxisTitleFontSize; set { _yAxisTitleFontSize = value; onPropertyChanged(nameof(yAxisTitleFontSize)); } }
        public int yAxisTitleLeftOffset { get => _yAxisTitleLeftOffset; set { _yAxisTitleLeftOffset = value; onPropertyChanged(nameof(yAxisTitleLeftOffset)); } }
        public int yAxisTitleTopOffset { get => _yAxisTitleTopOffset; set { _yAxisTitleTopOffset = value; onPropertyChanged(nameof(yAxisTitleTopOffset)); } }

        public int legendFontSize { get => _legendFontSize; set { _legendFontSize = value; onPropertyChanged(nameof(legendFontSize)); } }

        public string backgroundColor { get => _backgroundColor; set { _backgroundColor = value; onPropertyChanged(nameof(backgroundColor)); } }
        public string foregroundColor { get => _foregroundColor; set { _foregroundColor = value; onPropertyChanged(nameof(foregroundColor)); } }

        public RelayCommand updateLegendSettingsCommand { get; private set; }
        public RelayCommand updateGraphColorSettingsCommand { get; private set; }
        public RelayCommand updateAxisSettingsCommand { get; private set; }
        public RelayCommand updateAxisTitleSettingsCommand { get; private set; }
        public RelayCommand updateTitleSettingsCommand { get; private set; }

        public CustomizePanelViewModel()
        {
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;
            numXAxisTicks = _graphCustomizeStore.numXAxisTicks;
            numYAxisTicks = _graphCustomizeStore.numYAxisTicks;
            axesThickness = _graphCustomizeStore.axesThickness;
            numPoints = _graphCustomizeStore.numPoints;
            axesTickFontSize = _graphCustomizeStore.axesTickFontSize;

            titleFontSize = _graphCustomizeStore.titleFontSize;
            titleLeftOffset = _graphCustomizeStore.titleLeftOffset;
            titleTopOffset = _graphCustomizeStore.titleTopOffset;

            xAxisTitleFontSize = _graphCustomizeStore.xAxisTitleFontSize;
            xAxisTitleLeftOffset = _graphCustomizeStore.xAxisTitleLeftOffset;
            xAxisTitleTopOffset = _graphCustomizeStore.xAxisTitleTopOffset;

            yAxisTitleFontSize = _graphCustomizeStore.yAxisTitleFontSize;
            yAxisTitleLeftOffset = _graphCustomizeStore.yAxisTitleLeftOffset;
            yAxisTitleTopOffset = _graphCustomizeStore.yAxisTitleTopOffset;

            legendFontSize = _graphCustomizeStore.legendFontSize;

            foregroundColor = _graphCustomizeStore.foregroundColor;
            backgroundColor = _graphCustomizeStore.backgroundColor;

            updateAxisSettingsCommand = new RelayCommand(updateAxesSettings);
            updateLegendSettingsCommand = new RelayCommand(updateLegendSettings);
            updateGraphColorSettingsCommand = new RelayCommand(updateGraphColorSettings);
            updateAxisTitleSettingsCommand = new RelayCommand(updateAxisTitleSettings);
            updateTitleSettingsCommand = new RelayCommand(updateTitleSettings);
        }


        private void updateAxesSettings(object _)
        {
            _graphCustomizeStore.numXAxisTicks = _numXAxisTicks;
            _graphCustomizeStore.numYAxisTicks = _numYAxisTicks;
            _graphCustomizeStore.axesThickness = _axesThickness;
            _graphCustomizeStore.axesTickFontSize = _axesTickFontSize;
            _graphCustomizeStore.numPoints = _numPoints;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }

        private void updateTitleSettings(object _)
        {
            _graphCustomizeStore.titleFontSize = _titleFontSize;
            _graphCustomizeStore.titleLeftOffset = _titleLeftOffset;
            _graphCustomizeStore.titleTopOffset = _titleTopOffset;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }

        private void updateAxisTitleSettings(object _)
        {
            _graphCustomizeStore.xAxisTitleFontSize = _xAxisTitleFontSize;
            _graphCustomizeStore.xAxisTitleLeftOffset = _xAxisTitleLeftOffset;
            _graphCustomizeStore.xAxisTitleTopOffset = _xAxisTitleTopOffset;
            _graphCustomizeStore.yAxisTitleFontSize = _yAxisTitleFontSize;
            _graphCustomizeStore.yAxisTitleLeftOffset = _yAxisTitleLeftOffset;
            _graphCustomizeStore.yAxisTitleTopOffset = _yAxisTitleTopOffset;
            _graphCustomizeStore.onGraphCustomizeChanged();
        }

        private void updateLegendSettings(object _)
        {
            _graphCustomizeStore.legendFontSize = _legendFontSize;
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

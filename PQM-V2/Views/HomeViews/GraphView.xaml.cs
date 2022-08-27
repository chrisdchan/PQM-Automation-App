using PQM_V2.Models;
using PQM_V2.Stores;
using PQM_V2.ViewModels;
using PQM_V2.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PQM_V2.Views.HomeViews
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        private GraphAttributesStore _graphAttributesStore;
        private GraphStore _graphStore;

        private Canvas _axesCanvas;
        private Canvas _textCanvas;
        private List<Canvas> _structureCanvases;

        private DispatcherTimer _resizeTimer;

        private SolidColorBrush BLACK = new SolidColorBrush(Colors.Black);

        private (double left, double top, double right, double bottom) borders;
        private (Func<double, double> x, Func<double, double> y) map;

        public GraphView()
        {
            InitializeComponent();
            DataContext = new GraphViewModel();

            _graphAttributesStore = (Application.Current as App).graphAttributesStore;
            _graphStore = (Application.Current as App).graphStore;

            _axesCanvas = new Canvas();
            grid.Children.Add(_axesCanvas);
            _textCanvas = new Canvas();
            grid.Children.Add(_textCanvas);
            _structureCanvases = new List<Canvas>();


            _resizeTimer = new DispatcherTimer();
            _resizeTimer.Interval = TimeSpan.FromMilliseconds(100);
            _resizeTimer.IsEnabled = false;
            _resizeTimer.Tick += onResize;

            initStructureCanvases();

            _graphStore.graphChanged += onGraphChanged;
            _graphAttributesStore.plotAttributesChanged += onPlotAttributesChanged;
            //_graphAttributesStore.backgroundcolorChanged += onBackgroundColorChanged;
            //_graphAttributesStore.axesStructureChanged += onAxesStructureChanged;
        }

        /* On Initialization */
        /* --------------------------------------------------------------------------------------------*/
        private void initStructureCanvases()
        {
            ScaleTransform graphLayoutTransform = new ScaleTransform(1, -1);

            for(int i = 0; i < _graphStore.graph.structures.Count; i++)
            {
                Canvas structureCanvas = new Canvas();
                structureCanvas.LayoutTransform = graphLayoutTransform;
                _structureCanvases.Add(structureCanvas);
                grid.Children.Add(structureCanvas);
            }
        }

        /* On Updates */
        /* --------------------------------------------------------------------------------------------*/
        private void applyDomainSizeChangeTransformations()
        {
            double width = grid.ActualWidth;
            double height = grid.ActualHeight;
        }
        private void setAxesCanvas()
        {
            _axesCanvas.Children.Clear();
            _axesCanvas.Background = _graphAttributesStore.backgroundColor;

            double width = grid.ActualWidth;
            double height = grid.ActualHeight;
            double domain = _graphAttributesStore.xmax - _graphAttributesStore.xmin;

            Line xaxis = getLine(0, 0, domain, 0, BLACK);
            Line yaxis = getLine(0, 0, 0, 100, BLACK);

            double xTickLength = 10 * (100 / height);
            double yTickLength = 10 * (domain / width);

            _axesCanvas.Children.Add(xaxis);
            _axesCanvas.Children.Add(yaxis);

            double x = 0;
            double dx = domain / _graphAttributesStore.numXAxisTicks;
            for(int i = 0; i < _graphAttributesStore.numXAxisTicks; i++)
            {
                Line tick = getLine(x, 0, x, -xTickLength, BLACK);
                _axesCanvas.Children.Add(tick);
                x += dx;
            }

            double y = 0;
            double dy = 100 / _graphAttributesStore.numYAxisTicks;
            for(int i = 0; i < _graphAttributesStore.numYAxisTicks; i++)
            {
                Line tick = getLine(0, y, -yTickLength, y, BLACK);
                _axesCanvas.Children.Add(tick);
                y += dy;
            }
        }
        private void setStructureCanvases()
        {
            Graph graph = _graphStore.graph;
            for(int i = 0; i < graph.structures.Count; i++)
            {
                Structure structure = graph.structures[i];
                Canvas canvas = _structureCanvases[i];
                setStructureCanvas(canvas, structure);
            }
        }
        private void setStructureCanvas(Canvas canvas, Structure structure)
        {
            double xmin = _graphAttributesStore.xmin;
            double xmax = _graphAttributesStore.xmax;
            int numPoints = _graphAttributesStore.pointsPerPlot;

            List<(double x, double y)> points = structure.interpolateRange(xmin, xmax, numPoints);

            double x1, x2, y1, y2;

            for(int i = 0; i < numPoints - 1; i++)
            {
                x1 = points[i].x;
                y1 = points[i].y;
                x2 = points[i + 1].x;
                y2 = points[i + 1].y;

                Line line = getLine(x1 - xmin, y1, x2 - xmin, y2, structure.color);
                canvas.Children.Add(line);
            }
        }

        private void clearAllCanvases()
        {
            _axesCanvas.Children.Clear();
            foreach (Canvas canvas in _structureCanvases) canvas.Children.Clear();
        }

        /* Helper Functions */
        /* --------------------------------------------------------------------------------------------*/
        private Line getLine(double x1, double y1, double x2, double y2, SolidColorBrush color)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = getThickness(x1, y1, x2, y2);
            line.Stroke = color;
            return line;
        }
        private double getThickness(double x1, double y1, double x2, double y2)
        {
            double scaleX = grid.ActualWidth / (_graphAttributesStore.xmax - _graphAttributesStore.xmin);
            double scaleY = grid.ActualHeight / 100;

            double x = x2 - x1;
            double y = y2 - y1;

            double length = Math.Pow(x, 2) + Math.Pow(y, 2);
            double z = Math.Sqrt(length);

            double thickness = Math.Abs(z / (x * scaleY - y * scaleX));
            return Math.Min(thickness, 0.5);
        }


        /* Global Event Handlers */
        /* --------------------------------------------------------------------------------------------*/
        private void onGraphChanged()
        {
            clearAllCanvases();
            _structureCanvases.Clear();
            initStructureCanvases();
            setStructureCanvases();
            setAxesCanvas();
        }
        private void onStructureColorChanged()
        {
            setStructureCanvases();
        }
        private void onStructureAdded()
        {
            setStructureCanvases();
        }
        private void onStructureDeleted()
        {
            setStructureCanvases();
        }
        private void onPlotAttributesChanged()
        {
        }

        /* Local Event Handlers */
        /* --------------------------------------------------------------------------------------------*/
        private void onResize(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            borders = (
                left: _axesCanvas.ActualWidth * 0.1,
                top: _axesCanvas.ActualHeight * 0.1,
                right: _axesCanvas.ActualWidth * 0.9,
                bottom: _axesCanvas.ActualHeight * 0.1
                );

            textCanvas.Visibility = Visibility.Visible;
            setStructureCanvases();
            setAxesCanvas();
        }
        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            clearAllCanvases();
            textCanvas.Visibility = Visibility.Hidden;
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }
    }
}



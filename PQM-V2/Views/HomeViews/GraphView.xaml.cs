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
        private Canvas _legendCanvas;
        private List<Canvas> _structureCanvases;

        private DispatcherTimer _resizeTimer;

        private SolidColorBrush BLACK = new SolidColorBrush(Colors.Black);
        private ScaleTransform _graphLayoutTransform;
        private ScaleTransform _textBoxRenderTransform;
        private TransformGroup _yaxisRenderTransform;
        private Style _axisTickTextBoxStyle = new Style(typeof(TextBlock));

        private (double left, double top, double right, double bottom) borders;
        private (double x, double y) ratio;
        private (Func<double, double> x, Func<double, double> y) map;

        public GraphView()
        {
            InitializeComponent();
            DataContext = new GraphViewModel();

            _graphAttributesStore = (Application.Current as App).graphAttributesStore;
            _graphStore = (Application.Current as App).graphStore;

            _resizeTimer = new DispatcherTimer();
            _resizeTimer.Interval = TimeSpan.FromMilliseconds(100);
            _resizeTimer.IsEnabled = false;
            _resizeTimer.Tick += onResizeTimerEnd;

            initMapFunction();
            initTransforms();
            initCanvases();

            _graphStore.graphChanged += onGraphChanged;
            _graphAttributesStore.graphAttributesChanged += onGraphAttributesChanged;
        }

        /* On Initialization */
        /* --------------------------------------------------------------------------------------------*/
        private void initMapFunction()
        {
            map = (
                x: (x) => (x - _graphAttributesStore.xmin) * ratio.x + borders.left,
                y: (y) => y * ratio.y + borders.bottom
                );
        }
        private void initTransforms()
        {
            _graphLayoutTransform = new ScaleTransform(1, -1);
            _textBoxRenderTransform = new ScaleTransform(1, -1);
            _yaxisRenderTransform = new TransformGroup();
            _yaxisRenderTransform.Children.Add(new ScaleTransform(1, -1));
            _yaxisRenderTransform.Children.Add(new RotateTransform(90));

        }
        private void initStructureCanvases()
        {
            _structureCanvases = new List<Canvas>();
            for(int i = 0; i < _graphStore.graph.structures.Count; i++)
            {
                Canvas structureCanvas = new Canvas();
                structureCanvas.LayoutTransform = _graphLayoutTransform;
                _structureCanvases.Add(structureCanvas);
                grid.Children.Add(structureCanvas);
            }
        }
        private void initCanvases()
        {
            _textCanvas = new Canvas();
            _textCanvas.LayoutTransform = _graphLayoutTransform;
            grid.Children.Add(_textCanvas);

            initStructureCanvases();

            _legendCanvas = new Canvas();
            grid.Children.Add(_legendCanvas);

            _axesCanvas = new Canvas();
            Grid.SetZIndex(_axesCanvas, 1);
            _axesCanvas.LayoutTransform = _graphLayoutTransform;
            grid.Children.Add(_axesCanvas);
        }

        /* On Updates */
        /* --------------------------------------------------------------------------------------------*/
        private void setAxesCanvas()
        {
            _axesCanvas.Children.Clear();
            SolidColorBrush color = _graphAttributesStore.axisColor;

            Line xaxis = getLine(borders.left, borders.bottom, borders.right, borders.bottom, color);
            Line yaxis = getLine(borders.left, borders.bottom, borders.left, borders.top, color);

            double xTickLength = 10;
            double yTickLength = 10;

            _axesCanvas.Children.Add(xaxis);
            _axesCanvas.Children.Add(yaxis);

            double x = borders.left;
            double dx = (borders.right - borders.left) / (_graphAttributesStore.numXAxisTicks - 1);
            for(int i = 0; i < _graphAttributesStore.numXAxisTicks; i++)
            {
                Line tick = getLine(x, borders.bottom, x, borders.bottom - xTickLength, color);
                _axesCanvas.Children.Add(tick);
                x += dx;
            }

            double y = borders.bottom;
            double dy = (borders.top - borders.bottom) / (_graphAttributesStore.numYAxisTicks - 1);
            for(int i = 0; i < _graphAttributesStore.numYAxisTicks; i++)
            {
                Line tick = getLine(borders.left, y, borders.left - yTickLength, y, color);
                _axesCanvas.Children.Add(tick);
                y += dy;
            }
        }
        private void setTextCanvas()
        {
            _textCanvas.Children.Clear();
            _textCanvas.Background = _graphAttributesStore.backgroundColor;

            double titleWidth = 300;

            TextBlock title = new TextBlock();
            title.Text = _graphStore.graph.title;
            title.FontSize = 25;
            title.RenderTransform = _textBoxRenderTransform;
            title.Width = titleWidth;
            title.Foreground = _graphAttributesStore.axisColor;
            title.TextAlignment = TextAlignment.Center;
            _textCanvas.Children.Add(title);
            Canvas.SetLeft(title, (borders.right + borders.left) / 2.0 - titleWidth / 2.0);
            Canvas.SetTop(title, borders.top + 50);


            double axisTitleHeight = 20;
            double axisTitleWidth = 300;

            TextBlock xaxis = new TextBlock();
            xaxis.Text = _graphStore.graph.xaxisName;
            xaxis.FontSize = 18;
            xaxis.Width = axisTitleWidth;
            xaxis.RenderTransform = _textBoxRenderTransform;
            xaxis.Foreground = _graphAttributesStore.axisColor;
            xaxis.TextAlignment = TextAlignment.Center;
            _textCanvas.Children.Add(xaxis);
            Canvas.SetLeft(xaxis, (borders.left + borders.right) / 2.0 - axisTitleWidth / 2.0);
            Canvas.SetTop(xaxis, borders.bottom - 55);

            TextBlock yaxis = new TextBlock();
            yaxis.Text = _graphStore.graph.yaxisName;
            yaxis.FontSize = 18;
            yaxis.Width = axisTitleWidth;
            yaxis.RenderTransform = _yaxisRenderTransform;
            yaxis.Foreground = _graphAttributesStore.axisColor;
            yaxis.TextAlignment = TextAlignment.Center;
            _textCanvas.Children.Add(yaxis);
            Canvas.SetLeft(yaxis, borders.left - 100);
            Canvas.SetTop(yaxis, (borders.top + borders.bottom) / 2.0 - axisTitleWidth / 2.0);

            double width = 40;
            double height = 30;
            double fontSize = 12;

            double x = _graphAttributesStore.xmin;
            double dx = (_graphAttributesStore.xmax - _graphAttributesStore.xmin) / (_graphAttributesStore.numXAxisTicks - 1);

            for(int i = 0; i < _graphAttributesStore.numXAxisTicks; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Width = width;
                textBlock.Height = height;
                textBlock.FontSize = fontSize;
                textBlock.Foreground = _graphAttributesStore.axisColor;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.RenderTransform = _textBoxRenderTransform;
                textBlock.Text = Math.Round(x, 2).ToString();

                _textCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, map.x(x) - width / 2.0);
                Canvas.SetTop(textBlock, borders.bottom - height / 2.0);
                x += dx;
            }

            double y = 0;
            double dy = 100.0 / (_graphAttributesStore.numYAxisTicks - 1);

            for(int i = 0; i < _graphAttributesStore.numYAxisTicks; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Style = _axisTickTextBoxStyle;
                textBlock.Width = width;
                textBlock.Height = height;
                textBlock.TextAlignment = TextAlignment.Right;
                textBlock.FontSize = fontSize;
                textBlock.Foreground = _graphAttributesStore.axisColor;
                textBlock.RenderTransform = _textBoxRenderTransform;
                textBlock.Text = Math.Round(y, 2).ToString();

                _textCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, borders.left - width - 15);
                Canvas.SetTop(textBlock, map.y(y) + height * 0.35);
                y += dy;
            }
        }
        private void setLegendCanvas()
        {
            StackPanel stackPanel = new StackPanel();
            foreach(Structure structure in _graphStore.graph.structures)
            {
                StackPanel horizontalSP = new StackPanel();
                horizontalSP.Orientation = Orientation.Horizontal;

                Label label = new Label();
                label.Width = 100;
                label.Height = 25;
                label.Foreground = _graphAttributesStore.axisColor;
                label.Content = structure.name;
                horizontalSP.Children.Add(label);

                Rectangle rectangle = new Rectangle();
                rectangle.Fill = structure.color;
                rectangle.Width = 25;
                rectangle.Height = 15;
                horizontalSP.Children.Add(rectangle);

                stackPanel.Children.Add(horizontalSP);
            }
            _legendCanvas.Children.Add(stackPanel);
            Canvas.SetLeft(stackPanel, (borders.right + grid.ActualWidth) / 2.0 - 125.0 / 2.0);
            Canvas.SetTop(stackPanel, (borders.top - borders.bottom) / 2.0);
        }
        private void setStructureCanvases()
        {
            Graph graph = _graphStore.graph;
            for(int i = 0; i < graph.structures.Count; i++)
            {
                Structure structure = graph.structures[i];
                if(structure.visible)
                {
                    Canvas canvas = _structureCanvases[i];
                    setStructureCanvas(canvas, structure);
                }
            }
        }
        private void setStructureCanvas(Canvas canvas, Structure structure)
        {
            double xmin = _graphAttributesStore.xmin;
            double xmax = _graphAttributesStore.xmax;
            int numPoints = _graphAttributesStore.pointsPerPlot;

            List<(double x, double y)> points = structure.interpolateRange(xmin, xmax, numPoints);

            double x1, x2, y1, y2;
            double mx1, mx2, my1, my2;

            for(int i = 0; i < numPoints - 1; i++)
            {
                x1 = points[i].x;
                y1 = points[i].y;
                x2 = points[i + 1].x;
                y2 = points[i + 1].y;

                mx1 = map.x(x1);
                my1 = map.y(y1);
                mx2 = map.x(x2);
                my2 = map.y(y2);
                

                Line line = getLine(mx1, my1, mx2, my2, structure.color);
                canvas.Children.Add(line);
            }
        }
        private void clearAllCanvases()
        {
            _axesCanvas.Children.Clear();
            _textCanvas.Children.Clear();
            _legendCanvas.Children.Clear();
            foreach (Canvas canvas in _structureCanvases) canvas.Children.Clear();
        }
        private void resetGraphDimensions()
        {
            borders = (
                left: _axesCanvas.ActualWidth * 0.15,
                top: _axesCanvas.ActualHeight * 0.90,
                right: _axesCanvas.ActualWidth * 0.80,
                bottom: _axesCanvas.ActualHeight * 0.15
                );

            ratio = (
                x: (borders.right - borders.left) / (_graphAttributesStore.xmax - _graphAttributesStore.xmin),
                y: (borders.top - borders.bottom) / 100
                );
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
            line.StrokeThickness = 1;
            line.Stroke = color;
            return line;
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
            setLegendCanvas();
            setTextCanvas();
        }
        private void onGraphAttributesChanged()
        {
            resetGraphDimensions();
            clearAllCanvases();
            setStructureCanvases();
            setAxesCanvas();
            setLegendCanvas();
            setTextCanvas();
        }

        /* Local Event Handlers */
        /* --------------------------------------------------------------------------------------------*/
        private void onResizeTimerEnd(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            resetGraphDimensions();

            setStructureCanvases();
            setAxesCanvas();
            setTextCanvas();
            setLegendCanvas();
        }
        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            clearAllCanvases();
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }
    }
}



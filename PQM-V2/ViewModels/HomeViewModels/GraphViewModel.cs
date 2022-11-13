using PQM_V2.Commands;
using PQM_V2.Models;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PQM_V2.ViewModels.HomeViewModels
{
    public class GraphViewModel
    {
        private GraphCustomizeStore _graphCustomizeStore;
        private GraphStore _graphStore;
        private CanvasStore _canvasStore;

        private Canvas _axesCanvas;
        private Canvas _textCanvas;
        private Canvas _legendCanvas;
        private List<Canvas> _structureCanvases;

        private Label _probeLabel;

        private TransformGroup _yaxisRenderTransform;

        private (double left, double top, double right, double bottom) _borders;
        private (double x, double y) _ratio;
        private (Func<double, double> x, Func<double, double> y) _map;
        private (Func<double, double> x, Func<double, double> y) _invMap;

        public RelayCommand resizeEndCommand { get; set; }
        public RelayCommand resizeStartCommand { get; set; }
        public GraphViewModel()
        {
            _graphCustomizeStore = (Application.Current as App).graphCustomizeStore;
            _graphStore = (Application.Current as App).graphStore;
            _canvasStore = (Application.Current as App).canvasStore;

            _yaxisRenderTransform = new TransformGroup();
            _yaxisRenderTransform.Children.Add(new ScaleTransform(1, -1));
            _yaxisRenderTransform.Children.Add(new RotateTransform(90));

            resizeStartCommand = new RelayCommand(resizeStart);
            resizeEndCommand = new RelayCommand(resizeEnd);

            init();

            _graphCustomizeStore.graphCustomizeChanged += update;
            _graphStore.graphUpdated += update;
            _graphStore.graphChanged += init;
        }
        // Main Function call
        private void init()
        {
            _map = (
                x: (x) => (x - _graphCustomizeStore.xmin) * _ratio.x + _borders.left,
                y: (y) => y * _ratio.y + _borders.bottom);

            _invMap = (
                x: (x) => (x - _borders.left) / _ratio.x + _graphCustomizeStore.xmin,
                y: (y) => (y - _borders.bottom) / _ratio.y);

            initCanvases();
            update();
        }
        private void update()
        {
            clearAllCanvases();
            Canvas canvas = _canvasStore.canvas;
             _borders = (
                left: canvas.ActualWidth * 0.15,
                top: canvas.ActualHeight * 0.90,
                right: canvas.ActualWidth * 0.80,
                bottom: canvas.ActualHeight * 0.15 );

            _ratio = (
                x: (_borders.right - _borders.left) / (_graphCustomizeStore.xmax - _graphCustomizeStore.xmin),
                y: (_borders.top - _borders.bottom) / 100);

            setAxesCanvas();
            setTextCanvas();
            setLegendCanvas();
            setStructureCanvases();
        }
        private void initCanvases() // Helper to init
        {
            _canvasStore.canvas.Children.Clear();
            _textCanvas = new Canvas();
            _textCanvas.LayoutTransform = new ScaleTransform(1, -1);
            _canvasStore.canvas.Children.Add(_textCanvas);

            initStructureCanvases();

            _legendCanvas = new Canvas();
            _canvasStore.canvas.Children.Add(_legendCanvas);

            _axesCanvas = new Canvas();
            Grid.SetZIndex(_axesCanvas, 1);
            _axesCanvas.LayoutTransform = new ScaleTransform(1, -1);
            _canvasStore.canvas.Children.Add(_axesCanvas);

            _canvasStore.canvas.MouseDown += updateProbeLabel;
        }
        private void initStructureCanvases() // Helper to init/initCanvases
        {
            _structureCanvases = new List<Canvas>();
            for(int i = 0; i < _graphStore.graph.structures.Count; i++)
            {
                Canvas structureCanvas = new Canvas();
                structureCanvas.LayoutTransform = new ScaleTransform(1, -1);
                _structureCanvases.Add(structureCanvas);
                _canvasStore.canvas.Children.Add(structureCanvas);
            }
        }
        private void setAxesCanvas() // Helper to update
        {
            _axesCanvas.Children.Clear();
            setCanvasHeightWidth(_axesCanvas);
            SolidColorBrush color = toBrush(_graphCustomizeStore.foregroundColor);

            Line xaxis = getLine(_borders.left, _borders.bottom, _borders.right, _borders.bottom, color);
            Line yaxis = getLine(_borders.left, _borders.bottom, _borders.left, _borders.top, color);
            xaxis.StrokeThickness = _graphCustomizeStore.axesThickness;
            yaxis.StrokeThickness = _graphCustomizeStore.axesThickness;

            double xTickLength = 10;
            double yTickLength = 10;

            _axesCanvas.Children.Add(xaxis);
            _axesCanvas.Children.Add(yaxis);

            double x = _borders.left;
            double dx = (_borders.right - _borders.left) / (_graphCustomizeStore.numXAxisTicks - 1);
            for(int i = 0; i < _graphCustomizeStore.numXAxisTicks; i++)
            {
                Line tick = getLine(x, _borders.bottom, x, _borders.bottom - xTickLength, color);
                _axesCanvas.Children.Add(tick);
                x += dx;
            }

            double y = _borders.bottom;
            double dy = (_borders.top - _borders.bottom) / (_graphCustomizeStore.numYAxisTicks - 1);
            for(int i = 0; i < _graphCustomizeStore.numYAxisTicks; i++)
            {
                Line tick = getLine(_borders.left, y, _borders.left - yTickLength, y, color);
                _axesCanvas.Children.Add(tick);
                y += dy;
            }

            _probeLabel = new Label();
            _probeLabel.Content = "X: 20 Y: 9";
            _axesCanvas.Children.Add(_probeLabel);
            Canvas.SetLeft(_probeLabel, 0);
            Canvas.SetTop(_probeLabel, 0);

        }
        private void setTextCanvas() // Helper to update
        {
            _textCanvas.Children.Clear();
            setCanvasHeightWidth(_textCanvas);
            _textCanvas.Background = toBrush(_graphCustomizeStore.backgroundColor);

            double titleWidth = 300;

            TextBlock title = new TextBlock();
            title.Text = _graphStore.graph.title;
            title.FontSize = _graphCustomizeStore.titleFontSize;
            title.RenderTransform = new ScaleTransform(1, -1);
            title.Width = titleWidth;
            title.Foreground = toBrush(_graphCustomizeStore.foregroundColor);
            title.TextAlignment = TextAlignment.Center;
            _textCanvas.Children.Add(title);
            Canvas.SetLeft(title, (_borders.right + _borders.left) / 2.0 - titleWidth / 2.0 + _graphCustomizeStore.titleLeftOffset);
            Canvas.SetTop(title, _borders.top + 50 + _graphCustomizeStore.titleTopOffset);


            double axisTitleHeight = 20;
            double axisTitleWidth = 300;

            TextBlock xaxis = new TextBlock();
            xaxis.Text = _graphStore.graph.xaxisName;
            xaxis.FontSize = _graphCustomizeStore.xAxisTitleFontSize;
            xaxis.Width = axisTitleWidth;
            xaxis.RenderTransform = new ScaleTransform(1, -1);
            xaxis.Foreground = toBrush(_graphCustomizeStore.foregroundColor);
            xaxis.TextAlignment = TextAlignment.Center;
            _textCanvas.Children.Add(xaxis);
            Canvas.SetLeft(xaxis, (_borders.left + _borders.right) / 2.0 - axisTitleWidth / 2.0 + _graphCustomizeStore.xAxisTitleLeftOffset);
            Canvas.SetTop(xaxis, _borders.bottom - 55 + _graphCustomizeStore.xAxisTitleTopOffset);

            TextBlock yaxis = new TextBlock();
            yaxis.Text = _graphStore.graph.yaxisName;
            yaxis.FontSize = _graphCustomizeStore.yAxisTitleFontSize;
            yaxis.Width = axisTitleWidth;
            yaxis.RenderTransform = _yaxisRenderTransform;
            yaxis.Foreground = toBrush(_graphCustomizeStore.foregroundColor);
            yaxis.TextAlignment = TextAlignment.Center;
            _textCanvas.Children.Add(yaxis);
            Canvas.SetLeft(yaxis, _borders.left - 100 + _graphCustomizeStore.yAxisTitleLeftOffset);
            Canvas.SetTop(yaxis, (_borders.top + _borders.bottom) / 2.0 - axisTitleWidth / 2.0 + _graphCustomizeStore.yAxisTitleTopOffset);

            double width = 40;
            double height = 30;
            double fontSize = 12;

            double x = _graphCustomizeStore.xmin;
            double dx = (_graphCustomizeStore.xmax - _graphCustomizeStore.xmin) / (_graphCustomizeStore.numXAxisTicks - 1);

            for(int i = 0; i < _graphCustomizeStore.numXAxisTicks; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Width = width;
                textBlock.Height = height;
                textBlock.FontSize = fontSize;
                textBlock.Foreground = toBrush(_graphCustomizeStore.foregroundColor);
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.RenderTransform = new ScaleTransform(1, -1);
                textBlock.Text = Math.Round(x, 2).ToString();

                _textCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, _map.x(x) - width / 2.0);
                Canvas.SetTop(textBlock, _borders.bottom - height / 2.0);
                x += dx;
            }

            double y = 0;
            double dy = 100.0 / (_graphCustomizeStore.numYAxisTicks - 1);

            for(int i = 0; i < _graphCustomizeStore.numYAxisTicks; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Width = width;
                textBlock.Height = height;
                textBlock.TextAlignment = TextAlignment.Right;
                textBlock.FontSize = fontSize;
                textBlock.Foreground = toBrush(_graphCustomizeStore.foregroundColor);
                textBlock.RenderTransform = new ScaleTransform(1, -1);
                textBlock.Text = Math.Round(y, 2).ToString();

                _textCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, _borders.left - width - 15);
                Canvas.SetTop(textBlock, _map.y(y) + height * 0.35);
                y += dy;
            }
        }
        private void setLegendCanvas() // Helper to update
        {
            _legendCanvas.Children.Clear();
            setCanvasHeightWidth(_legendCanvas);

            double width = calculateLegendLabelWidth();

            StackPanel stackPanel = new StackPanel();
            foreach(Structure structure in _graphStore.graph.structures)
            {
                StackPanel horizontalSP = new StackPanel();
                horizontalSP.Orientation = Orientation.Horizontal;

                Rectangle rectangle = new Rectangle();
                rectangle.Fill = structure.color;
                rectangle.Width = 10;
                rectangle.Height = 15;
                horizontalSP.Children.Add(rectangle);

                Label label = new Label();
                label.Height = 25;
                label.FontSize = _graphCustomizeStore.legendFontSize;
                label.Foreground = toBrush(_graphCustomizeStore.foregroundColor);
                label.Content = structure.name;
                label.Width = width;
                horizontalSP.Children.Add(label);

                stackPanel.Children.Add(horizontalSP);
            }
            _legendCanvas.Children.Add(stackPanel);
            Canvas.SetLeft(stackPanel, (_borders.right + _canvasStore.canvas.ActualWidth) / 2.0 - 125.0 / 2.0);
            Canvas.SetTop(stackPanel, (_borders.top - _borders.bottom) / 2.0);
        }
        private double calculateLegendLabelWidth()
        {
            double minWidth = 0;
            foreach(Structure structure in _graphStore.graph.structures)
            {
                int n = structure.name.Length;
                double width = n * _graphCustomizeStore.legendFontSize * 0.55;
                if(width > minWidth)
                {
                    minWidth = width;
                }
            }
            return minWidth;
        }
        private void setStructureCanvases() // Helper to update
        {
            Graph graph = _graphStore.graph;
            for(int i = 0; i < graph.structures.Count; i++)
            {
                Structure structure = graph.structures[i];
                if(structure.visible && isStructureInDomain(structure))
                {
                    Canvas canvas = _structureCanvases[i];
                    setCanvasHeightWidth(canvas);
                    setStructureCanvas(canvas, structure);
                }
            }
        }
        private bool isStructureInDomain(Structure structure) // Helper to setStructureCanvases
        {
            return _graphCustomizeStore.xmin < structure.maxX;
        }
        private void setStructureCanvas(Canvas canvas, Structure structure)
        {
            canvas.Children.Clear();
            double xmin = _graphCustomizeStore.xmin;
            double xmax = _graphCustomizeStore.xmax;
            int numPoints = _graphCustomizeStore.numPoints;

            List<(double x, double y)> points = structure.interpolateRange(xmin, xmax, numPoints);

            double x1, x2, y1, y2;
            double mx1, mx2, my1, my2;

            for(int i = 0; i < points.Count - 1; i++)
            {
                x1 = points[i].x;
                y1 = points[i].y;
                x2 = points[i + 1].x;
                y2 = points[i + 1].y;

                mx1 = _map.x(x1);
                my1 = _map.y(y1);
                mx2 = _map.x(x2);
                my2 = _map.y(y2);
                

                Line line = getLine(mx1, my1, mx2, my2, structure);
                canvas.Children.Add(line);
            }
        }
        private void clearAllCanvases() // Helper to update
        {
            _textCanvas.Children.Clear();
            _legendCanvas.Children.Clear();
            _axesCanvas.Children.Clear();
            foreach (Canvas canvas in _structureCanvases) canvas.Children.Clear();
        }
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

        private Line getLine(double x1, double y1, double x2, double y2, Structure structure)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = structure.lineThickness;
            line.Stroke = structure.color;
            return line;
        }
        private void setCanvasHeightWidth(Canvas canvas) // Helper to set<Name>Canvas()
        {
            canvas.Height = _canvasStore.canvas.ActualHeight;
            canvas.Width = _canvasStore.canvas.ActualWidth;

        }

        private SolidColorBrush toBrush(string hex_code)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFromString(hex_code);
        }

        // Commands
        private void resizeEnd(object _)
        {
            update();
        }
        private void resizeStart(object _)
        {
            clearAllCanvases();
        }

        private void updateProbeLabel(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(_axesCanvas as IInputElement);
            double x = _invMap.x(p.X);
            double y = _invMap.y(p.Y);

        }
    }
}

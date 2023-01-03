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
    public class GraphViewModel : BaseViewModel
    {
        private GraphCustomizeStore _graphCustomizeStore;
        private GraphStore _graphStore;
        private CanvasStore _canvasStore;

        private Canvas _baseCanvas;
        private Canvas _displayCanvas;
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

        public override void Dispose()
        {
            _graphCustomizeStore.graphCustomizeChanged -= update;
            _graphStore.graphUpdated -= update;
            _graphStore.graphChanged -= init;
        }

        // -------- Event Handlers -----------------------
        // -------- Init New Structures ------------------
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
        private void initCanvases() 
        {
            _canvasStore.canvas.Children.Clear();
            _canvasStore.canvas.LayoutTransform = new ScaleTransform(1, -1);

            _displayCanvas = new Canvas();
            _baseCanvas = new Canvas();


            _canvasStore.canvas.Children.Add(_baseCanvas);
            _canvasStore.displayCanvas = _displayCanvas;
            _canvasStore.canvas.Children.Add(_displayCanvas);
            Grid.SetZIndex(_baseCanvas, 1);

            initStructureCanvases();



            _canvasStore.canvas.PreviewMouseMove += updateProbeLabel;
        }
        private void initStructureCanvases() 
        {
            _structureCanvases = new List<Canvas>();
            for (int i = 0; i < _graphStore.graph.structures.Count; i++)
            {
                Canvas structureCanvas = new Canvas();
                _structureCanvases.Add(structureCanvas);
                _canvasStore.canvas.Children.Add(structureCanvas);
            }
        }

        // --------- Changes to Graph Settings -------------
        private void update()
        {
            Canvas canvas = _canvasStore.canvas;
            _borders = (
               left: canvas.ActualWidth * 0.15,
               top: canvas.ActualHeight * 0.90,
               right: canvas.ActualWidth * 0.80,
               bottom: canvas.ActualHeight * 0.15);

            _ratio = (
                x: (_borders.right - _borders.left) / (_graphCustomizeStore.xmax - _graphCustomizeStore.xmin),
                y: (_borders.top - _borders.bottom) / 100);

            _canvasStore.canvas.Background = stringToBrush(_graphCustomizeStore.backgroundColor);

            setBaseCanvas();
            setDisplayCanvas();
            setStructureCanvases();
        }

        // --------- Graph Resize --------------------------
        private void resizeEnd(object _)
        {
            update();
        }
        private void resizeStart(object _)
        {
            clearAllCanvases();
        }
        private void clearAllCanvases()
        {
            _baseCanvas.Children.Clear();
            _displayCanvas.Children.Clear();
            clearAllStructureCanvases();
        }

        // -------- Base Canvas On Update ------------------
        private void setBaseCanvas()
        {
            _baseCanvas.Children.Clear();
            setCanvasHeightWidth(_baseCanvas);
            setAxesLines();
            setAxesTickLabels();
            setAxesTitles();
            setTitle();
            setLegend();
        }
        private void setAxesLines() 
        {
            SolidColorBrush color = stringToBrush(_graphCustomizeStore.foregroundColor);

            Line xaxis = getLine(_borders.left, _borders.bottom, _borders.right, _borders.bottom, color);
            Line yaxis = getLine(_borders.left, _borders.bottom, _borders.left, _borders.top, color);
            xaxis.StrokeThickness = _graphCustomizeStore.axesThickness;
            yaxis.StrokeThickness = _graphCustomizeStore.axesThickness;

            double xTickLength = 10;
            double yTickLength = 10;

            _baseCanvas.Children.Add(xaxis);
            _baseCanvas.Children.Add(yaxis);


            double x = _borders.left;
            double dx = (_borders.right - _borders.left) / (_graphCustomizeStore.numXAxisTicks - 1);
            for (int i = 0; i < _graphCustomizeStore.numXAxisTicks; i++)
            {
                Line tick = getLine(x, _borders.bottom, x, _borders.bottom - xTickLength, color);
                _baseCanvas.Children.Add(tick);
                x += dx;
            }

            double y = _borders.bottom;
            double dy = (_borders.top - _borders.bottom) / (_graphCustomizeStore.numYAxisTicks - 1);
            for (int i = 0; i < _graphCustomizeStore.numYAxisTicks; i++)
            {
                Line tick = getLine(_borders.left, y, _borders.left - yTickLength, y, color);
                _baseCanvas.Children.Add(tick);
                y += dy;
            }
        }
        private void setAxesTitles()
        {
            double axisTitleHeight = 20;
            double axisTitleWidth = 300;

            TextBlock xaxis = new TextBlock();
            xaxis.Text = _graphStore.graph.xaxisName;
            xaxis.FontWeight = _graphCustomizeStore.xAxisTitleBold ? FontWeights.Bold : FontWeights.Normal;
            xaxis.FontStyle = _graphCustomizeStore.xAxisTitleItalic ? FontStyles.Italic : FontStyles.Normal;
            xaxis.RenderTransform = new ScaleTransform(1, -1);
            xaxis.Foreground = stringToBrush(_graphCustomizeStore.foregroundColor);
            xaxis.TextAlignment = TextAlignment.Center;
            setTitleSize(xaxis, _graphCustomizeStore.xAxisTitleSize);
            _baseCanvas.Children.Add(xaxis);
            Canvas.SetLeft(xaxis, (_borders.left + _borders.right) / 2.0 - axisTitleWidth / 2.0 + _graphCustomizeStore.xAxisTitleLeftOffset);
            Canvas.SetTop(xaxis, _borders.bottom - 55 + _graphCustomizeStore.xAxisTitleTopOffset);

            TextBlock yaxis = new TextBlock();
            yaxis.Text = _graphStore.graph.yaxisName;
            yaxis.FontWeight = _graphCustomizeStore.yAxisTitleBold ? FontWeights.Bold : FontWeights.Normal;
            yaxis.FontStyle = _graphCustomizeStore.yAxisTitleItalic ? FontStyles.Italic : FontStyles.Normal;
            yaxis.RenderTransform = _yaxisRenderTransform;
            yaxis.Foreground = stringToBrush(_graphCustomizeStore.foregroundColor);
            yaxis.TextAlignment = TextAlignment.Center;
            setTitleSize(yaxis, _graphCustomizeStore.yAxisTitleSize);
            _baseCanvas.Children.Add(yaxis);
            Canvas.SetLeft(yaxis, _borders.left - 100 + _graphCustomizeStore.yAxisTitleLeftOffset);
            Canvas.SetTop(yaxis, (_borders.top + _borders.bottom) / 2.0 - axisTitleWidth / 2.0 + _graphCustomizeStore.yAxisTitleTopOffset);
        }
        private void setTitle()
        {
            TextBlock title = new TextBlock();
            title.Text = _graphStore.graph.title;
            title.FontWeight = _graphCustomizeStore.titleBold ? FontWeights.Bold : FontWeights.Normal;
            title.FontStyle = _graphCustomizeStore.titleItalic ? FontStyles.Italic : FontStyles.Normal;
            title.RenderTransform = new ScaleTransform(1, -1);
            title.Foreground = stringToBrush(_graphCustomizeStore.foregroundColor);
            title.TextAlignment = TextAlignment.Center;
            setTitleSize(title, _graphCustomizeStore.titleSize);
            _baseCanvas.Children.Add(title);
            Canvas.SetLeft(title, (_borders.right + _borders.left) / 2.0 - title.Width / 2.0 + _graphCustomizeStore.titleLeftOffset);
            Canvas.SetTop(title, _borders.top + 50 + _graphCustomizeStore.titleTopOffset);
        }
        private void setAxesTickLabels()
        {
            double width = 40;
            double height = 30;
            double fontSize = 12;

            double x = _graphCustomizeStore.xmin;
            double dx = (_graphCustomizeStore.xmax - _graphCustomizeStore.xmin) / (_graphCustomizeStore.numXAxisTicks - 1);

            for (int i = 0; i < _graphCustomizeStore.numXAxisTicks; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Width = width;
                textBlock.Height = height;
                textBlock.FontSize = fontSize;
                textBlock.Foreground = stringToBrush(_graphCustomizeStore.foregroundColor);
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.RenderTransform = new ScaleTransform(1, -1);
                textBlock.Text = Math.Round(x, 2).ToString();

                _baseCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, _map.x(x) - width / 2.0);
                Canvas.SetTop(textBlock, _borders.bottom - height / 2.0);
                x += dx;
            }

            double y = 0;
            double dy = 100.0 / (_graphCustomizeStore.numYAxisTicks - 1);

            for (int i = 0; i < _graphCustomizeStore.numYAxisTicks; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Width = width;
                textBlock.Height = height;
                textBlock.TextAlignment = TextAlignment.Right;
                textBlock.FontSize = fontSize;
                textBlock.Foreground = stringToBrush(_graphCustomizeStore.foregroundColor);
                textBlock.RenderTransform = new ScaleTransform(1, -1);
                textBlock.Text = Math.Round(y, 2).ToString();

                _baseCanvas.Children.Add(textBlock);
                Canvas.SetLeft(textBlock, _borders.left - width - 15);
                Canvas.SetTop(textBlock, _map.y(y) + height * 0.35);
                y += dy;
            }
        }
        private void setLegend()
        {
            StackPanel stackPanel = new StackPanel();
            List<Structure> structures = _graphStore.graph.structures;
            double height = 0;

            stackPanel.RenderTransform = new ScaleTransform(1, -1);
            foreach (Structure structure in _graphStore.graph.structures)
            {
                StackPanel horizontalSP = new StackPanel();
                horizontalSP.Orientation = Orientation.Horizontal;

                Rectangle rectangle = new Rectangle();
                rectangle.Fill = structure.color;

                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = _graphCustomizeStore.legendSize;
                textBlock.Foreground = stringToBrush(_graphCustomizeStore.foregroundColor);
                textBlock.Text = structure.name;

                fixLegendBlockHeightAndWidth(textBlock, rectangle);
                height = textBlock.Height;

                horizontalSP.Children.Add(rectangle);
                horizontalSP.Children.Add(textBlock);

                stackPanel.Children.Add(horizontalSP);
            }
            _baseCanvas.Children.Add(stackPanel);
            Canvas.SetLeft(stackPanel, (_borders.right + _canvasStore.canvas.ActualWidth) / 2.0 - 125.0 / 2.0);
            double totalHeight = height * _graphStore.graph.structures.Count;
            Canvas.SetTop(stackPanel, (_borders.top + _borders.bottom + totalHeight) / 2.0);
        }
        private void setTitleSize(TextBlock textBlock, double fontSize)
        {
            int textLength = textBlock.Text.Length;
            double width = fontSize * (48.0 / 72.0) * textLength;
            double height = fontSize * (96.0 / 72.0);

            textBlock.FontSize = fontSize;
            textBlock.Width = width;
            textBlock.Height = height;
        }
        private void fixLegendBlockHeightAndWidth(TextBlock textblock, Rectangle rectangle)
        {
            int textLength = textblock.Text.Length;
            double fontSize = textblock.FontSize;
            double width = fontSize * (48.0 / 72.0) * textLength;
            double height = fontSize * (96.0 / 72.0);

            rectangle.Height = height * 0.9;
            rectangle.Width = height;
            textblock.Width = width;
            textblock.Height = height;
        }

        // ---------- Display Canvas Updates --------------
        private void setDisplayCanvas()
        {
            _displayCanvas.Children.Clear();
            setCanvasHeightWidth(_displayCanvas);
            setProbeLabel("");
        }
        private void setProbeLabel(string txt)
        {
            double width = 200;
            double height = 30;
            _probeLabel = new Label();
            _probeLabel.Height = height;
            _probeLabel.Width = width;
            _probeLabel.RenderTransform = new ScaleTransform(1, -1);
            _probeLabel.Content = txt;
            _displayCanvas.Children.Add(_probeLabel);
            Canvas.SetLeft(_probeLabel, _borders.right - width / 2.0);
            Canvas.SetTop(_probeLabel, _borders.top + 5);
        }
        private void updateProbeLabel(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(_displayCanvas as IInputElement);
            double x = _invMap.x(p.X);
            double y = _invMap.y(p.Y);
            _probeLabel.Content = "";
            if (x < _graphCustomizeStore.xmin || _graphCustomizeStore.xmax < x) return;
            if (y < 0 || y > 100) return;

            Structure selectedStructure = _graphStore.graph.selectedStructure;
            if (selectedStructure != null)
            {
                if (x > selectedStructure.maxX) return;
                if (_canvasStore.probeType == CanvasStore.ProbeTypes.x)
                {
                    double fx = selectedStructure.interpolate(x);
                    updateProbeCanvas(x, fx);
                }
                else if (_canvasStore.probeType == CanvasStore.ProbeTypes.y)
                {
                    double fy = selectedStructure.invInterpolate(y);
                    updateProbeCanvas(fy, y);
                }
                else
                {
                    _displayCanvas.Children.Clear();
                }
            }
        }
        private void updateProbeCanvas(double x, double y)
        {
            setDisplayCanvas();
            setProbeLabel(String.Format("{0}: X = {1}, Y = {2}", _graphStore.graph.selectedStructure.name, Math.Round(x, 2), Math.Round(y, 2)));
            double inner = 2;
            double length = 5;
            double thickness = 1;
            x = _map.x(x);
            y = _map.y(y);
            SolidColorBrush color = new SolidColorBrush(Colors.Black);
            Line top = getLine(x, y + inner, x, y + inner + length, color, thickness);
            Line bottom = getLine(x, y - inner, x, y - inner - length, color, thickness);
            Line right = getLine(x + inner, y, x + inner + length, y, color, thickness);
            Line left = getLine(x - inner, y, x - inner - length, y, color, thickness);
            _displayCanvas.Children.Add(top);
            _displayCanvas.Children.Add(bottom);
            _displayCanvas.Children.Add(right);
            _displayCanvas.Children.Add(left);
        }

        // ---------- Structure Canvases ------------------------------------------------
        private void setStructureCanvases()
        {
            clearAllStructureCanvases();
            Graph graph = _graphStore.graph;
            for (int i = 0; i < graph.structures.Count; i++)
            {
                Structure structure = graph.structures[i];
                if (structure.visible && isStructureInDomain(structure))
                {
                    Canvas canvas = _structureCanvases[i];
                    setCanvasHeightWidth(canvas);
                    setStructureCanvas(canvas, structure);
                }
            }
        }
        private void clearAllStructureCanvases()
        {
            foreach (Canvas canvas in _structureCanvases) canvas.Children.Clear();
        }
        private bool isStructureInDomain(Structure structure)
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

            for (int i = 0; i < points.Count - 1; i++)
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

        // ------------ Helper Methods --------------------------------------------------
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
        private Line getLine(double x1, double y1, double x2, double y2, SolidColorBrush color, double lineThickness)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.StrokeThickness = lineThickness;
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
        private void setCanvasHeightWidth(Canvas canvas) 
        {
            canvas.Height = _canvasStore.canvas.ActualHeight;
            canvas.Width = _canvasStore.canvas.ActualWidth;
        }
        private SolidColorBrush stringToBrush(string hex_code)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFromString(hex_code);
        }

    }

}

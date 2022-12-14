using Microsoft.Win32;
using PQM_V2.Commands;
using PQM_V2.Models;
using PQM_V2.Stores;
using PQM_V2.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PQM_V2.ViewModels.HomeViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly GraphStore _graphStore;
        private readonly CanvasStore _canvasStore;
        private readonly GraphCustomizeStore _graphCustomizeStore;

        private bool _graphVisible;
        private bool _tableVisible;
        private bool _attributesVisible;
        private Layout _layout;
        public Graph graph => _graphStore.graph;
        public RelayCommand navigateStartupCommand { get; private set; }
        public RelayCommand exitApplicationCommand { get; private set; }
        public RelayCommand openFilesCommand { get; private set; }
        public RelayCommand openFolderCommand { get; private set; }
        public RelayCommand exportGraphCommand { get; private set; }
        public RelayCommand exportTableCommand { get; private set; }
        public RelayCommand changeStructureCommand { get; private set; }
        public RelayCommand updateSelectedStructureTypeCommand { get; private set; }
        public bool graphVisible
        {
            get => _graphVisible;
            set { 
                _graphVisible = value; 
                onLayoutUpdated();
                onPropertyChanged(nameof(graphVisible));
            }
        }
        public bool tableVisible
        {
            get => _tableVisible;
            set {
                _tableVisible = value; 
                onLayoutUpdated();
                onPropertyChanged(nameof(tableVisible));
            }
        }
        public bool attributesVisible
        {
            get => _attributesVisible;
            set { 
                _attributesVisible = value; 
                onLayoutUpdated();
                onPropertyChanged(nameof(attributesVisible));
            }
        }

        public Layout layout
        {
            get => _layout;
            set { _layout = value; onPropertyChanged(nameof(layout)); }
        }

        public HomeViewModel()
        {
            _navigationStore = (System.Windows.Application.Current as App).navigationStore;
            _graphStore = (System.Windows.Application.Current as App).graphStore;
            _canvasStore = (System.Windows.Application.Current as App).canvasStore;
            _graphCustomizeStore = (System.Windows.Application.Current as App).graphCustomizeStore;

            _graphVisible = true;
            _tableVisible = true;
            _attributesVisible = true;

            layout = Layouts.layouts[(_graphVisible, _tableVisible, _attributesVisible)];

            navigateStartupCommand = new RelayCommand(navigateStartup);
            exitApplicationCommand = new RelayCommand(exitApplication);
            openFilesCommand = new RelayCommand(openFileDialog);
            openFolderCommand = new RelayCommand(openFolderDialog);
            exportGraphCommand = new RelayCommand(exportGraph);
            exportTableCommand = new RelayCommand(exportTable);
            changeStructureCommand = new RelayCommand(changeStructure);
            updateSelectedStructureTypeCommand = new RelayCommand(updateSelectedStructureType);

            _graphStore.graphChanged += onGraphChanged;
        }

        /*
         * Event Handlers
         */
        private void navigateStartup(object message)
        {
            _navigationStore.OnChangePage(CurrentPage.startup);
        }
        private void onLayoutUpdated()
        {
            if (!Layouts.layouts.ContainsKey((_graphVisible, _tableVisible, _attributesVisible))) throw new NotImplementedException();
            layout = Layouts.layouts[(_graphVisible, _tableVisible, _attributesVisible)];
        }
        private void onGraphChanged()
        {
            onPropertyChanged(nameof(graph));
        }
        private void exitApplication(object message)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void exportGraph(object message)
        {
            Canvas canvas = _canvasStore.canvas;
            canvas.Children.Remove(_canvasStore.displayCanvas);

            Rect rect = new Rect(canvas.RenderSize);
            int dpi = (int)_graphCustomizeStore.dpi;
            int sizeW = (int)(rect.Right * (dpi / 96.0));
            int sizeH = (int)(rect.Bottom * (dpi / 96.0));

            RenderTargetBitmap rtb = new RenderTargetBitmap(sizeW,
             sizeH, dpi, dpi, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);
            //endcode as PNG
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            //save to memory stream
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
            ms.Close();

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.Filter = "png files (*.png)|*.png";
            bool? result = saveFileDialog.ShowDialog();
            if(result.HasValue && result.Value)
            {
                System.IO.File.WriteAllBytes(saveFileDialog.FileName, ms.ToArray());
                System.Diagnostics.Process.Start(saveFileDialog.FileName);
            }
            else
            {
                (System.Windows.Application.Current as App).displayMessage("Error saving file");
            }

            _canvasStore.canvas.Children.Add(_canvasStore.displayCanvas);
            Canvas.SetZIndex(_canvasStore.displayCanvas, 2);
        }
        private void exportTable(object message)
        {
            var saveDialog = new System.Windows.Forms.SaveFileDialog();
            saveDialog.Filter = "CSV Files|*.csv";
            saveDialog.DefaultExt = "csv";

            DialogResult result = saveDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                _graphStore.graph.saveTableToCSV(saveDialog.FileName);
            }
        }
        public void openFileDialog(object message)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.Title = "Select Files";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                string result = CSVChecker.isValidFile(openFileDialog.FileNames);
                if(result == "passed")
                {
                    _graphStore.graph = new Graph(openFileDialog.FileNames);
                }
                else
                {
                    System.Windows.MessageBox.Show(result);
                }
            }
        }
        public void openFolderDialog(object _)
        {
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            DialogResult dialogResult = openFolderDialog.ShowDialog();
            if(dialogResult.ToString() != string.Empty)
            {
                string path = openFolderDialog.SelectedPath.ToString();
                if (path != String.Empty)
                {
                    string message = CSVChecker.isValidFile(Directory.GetFiles(path));
                    if(message == "passed")
                    {
                        _graphStore.graph = new Graph(Directory.GetFiles(path));
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(message);
                    }
                }
            }
        }
        private void changeStructure(object message)
        {
            if(_graphStore.graph.selectedStructure == null)
            {
                _graphStore.graph.selectStructure(0);
            }
            else if((string)message == "left")
            {
                _graphStore.graph.newSelectedStructureFromOffset(-1);
            }
            else if((string)message == "right")
            {
                _graphStore.graph.newSelectedStructureFromOffset(1);
            }
            _graphStore.onSelectedStructureChanged();
        }
        private void updateSelectedStructureType(object message)
        {
            Structure selected = _graphStore.graph.selectedStructure;
            int lineType = int.Parse((string)message);
            lineType -= 1;
            if(selected != null)
            {
                selected.lineType = (LineType)Enum.ToObject(typeof(LineType), lineType);
                _graphStore.onGraphUpdated();
                _graphStore.onSelectedStructureChanged();
            }
        }


    }
}

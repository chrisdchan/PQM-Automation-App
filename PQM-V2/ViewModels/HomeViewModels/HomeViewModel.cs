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
using System.Windows.Media.Imaging;

namespace PQM_V2.ViewModels.HomeViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly GraphStore _graphStore;
        private readonly CanvasStore _canvasStore;

        private bool _graphVisible;
        private bool _tableVisible;
        private bool _attributesVisible;
        private Layout _layout;
        public Graph graph => _graphStore.graph;
        public RelayCommand navigateStartupCommand { get; private set; }
        public RelayCommand exitApplicationCommand { get; private set; }
        public RelayCommand openFilesCommand { get; private set; }
        public RelayCommand exportGraphCommand { get; private set; }
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

            _graphVisible = true;
            _tableVisible = true;
            _attributesVisible = true;

            layout = Layouts.layouts[(_graphVisible, _tableVisible, _attributesVisible)];

            navigateStartupCommand = new RelayCommand(navigateStartup);
            exitApplicationCommand = new RelayCommand(exitApplication);
            openFilesCommand = new RelayCommand(openFileDialog);

            exportGraphCommand = new RelayCommand(exportGraph);

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
            Rect rect = new Rect(canvas.RenderSize);
           RenderTargetBitmap rtb = new RenderTargetBitmap((int)rect.Right,
             (int)rect.Bottom, 96d, 96d, System.Windows.Media.PixelFormats.Default);
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
            }
            else
            {
                (System.Windows.Application.Current as App).displayMessage("Error saving file");
            }
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

    }
}

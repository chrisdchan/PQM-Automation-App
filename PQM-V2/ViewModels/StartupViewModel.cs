using Microsoft.Win32;
using PQM_V2.Commands;
using PQM_V2.Models;
using PQM_V2.Stores;
using PQM_V2.Tools;
using PQM_V2.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace PQM_V2.ViewModels
{
    public class StartupViewModel : BaseViewModel
    {
        public double smallFontSize { get; set; }
        public double mediumFontSize { get; set; }
        public double largeFontSize { get; set; }
        public NavigationStore _navigationStore { get; set; }
        public GraphStore _graphStore { get; set; }
        public RelayCommand openFileDialogCommand { get; private set; }
        public RelayCommand openFolderDialogCommand { get; private set; }
        public RelayCommand navigateHomeCommand { get; private set; }

        public StartupViewModel()
        {
            _navigationStore = (System.Windows.Application.Current as App).navigationStore; 
            _graphStore = (System.Windows.Application.Current as App).graphStore;
            
            smallFontSize = 12;
            mediumFontSize = 18;
            largeFontSize = 40;

            openFileDialogCommand = new RelayCommand(openFileDialog, openFileDialogCanUse);
            openFolderDialogCommand = new RelayCommand(openFolderDialog, openFolderDialogCanUse);
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
                    _navigationStore.selectedViewModel = new HomeViewModel();
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
                        _navigationStore.selectedViewModel = new HomeViewModel();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(message);
                    }
                }
            }
        }

        public bool openFileDialogCanUse(object message) { return (string)message == "OpenFileDialog"; }
        public bool openFolderDialogCanUse(object message) {return (string)message == "OpenFolderDialog";}
    }
}

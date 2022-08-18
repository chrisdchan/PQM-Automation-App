﻿using Microsoft.Win32;
using PQM_V2.Commands;
using PQM_V2.Models;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        public StartupViewModel(NavigationStore navigationStore, GraphStore graphStore)
        {
            smallFontSize = 12;
            mediumFontSize = 18;
            largeFontSize = 40;

            _navigationStore = navigationStore;
            _graphStore = graphStore;

            openFileDialogCommand = new RelayCommand(openFileDialog, openFileDialogCanUse);
            openFolderDialogCommand = new RelayCommand(openFolderDialog, openFolderDialogCanUse);
            navigateHomeCommand = new RelayCommand(navigateHome);
        }

        public void openFileDialog(object message)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.Title = "Select Files";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                _graphStore.graph = new Graph(openFileDialog.FileNames);
                _navigationStore.selectedViewModel = new HomeViewModel(_navigationStore, _graphStore);

            }
        }
        public bool openFileDialogCanUse(object message) { return (string)message == "OpenFileDialog"; }
        public void openFolderDialog(object message)
        {
            MessageBox.Show("Opening Folder Dialog");
        }
        public bool openFolderDialogCanUse(object message) {return (string)message == "OpenFolderDialog";}
        public void navigateHome(object message)
        {
            _navigationStore.selectedViewModel = new HomeViewModel(_navigationStore, _graphStore);
        }
    }
}

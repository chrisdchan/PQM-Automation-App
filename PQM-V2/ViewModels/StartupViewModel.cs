using Microsoft.Win32;
using PQM_V2.Commands;
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
        public RelayCommand OpenFileDialogCommand { get; private set; }
        public RelayCommand OpenFolderDialogCommand { get; private set; }

        public StartupViewModel()
        {
            smallFontSize = 12;
            mediumFontSize = 18; ;
            largeFontSize = 40;

            OpenFileDialogCommand = new RelayCommand(openFileDialog, OpenFileDialogCanUse);
            OpenFolderDialogCommand = new RelayCommand(OpenFolderDialog, OpenFolderDialogCanUse);
        }

        public void openFileDialog(object message)
        {
            MessageBox.Show("Opening File Dialog");
        }
        public bool OpenFileDialogCanUse(object message) { return (string)message == "OpenFileDialog"; }
        public void OpenFolderDialog(object message)
        {
            MessageBox.Show("Opening Folder Dialog");
        }
        public bool OpenFolderDialogCanUse(object message) {return (string)message == "OpenFolderDialog";}

    }
}

using Microsoft.Win32;
using PQM_V2.Commands;
using PQM_V2.Models;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PQM_V2.ViewModels.HomeViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly GraphStore _graphStore;

        private bool _graphVisible;
        private bool _tableVisible;
        private bool _attributesVisible;
        private Layout _layout;
        public Graph graph => _graphStore.graph;
        public RelayCommand navigateStartupCommand { get; private set; }
        public RelayCommand exitApplicationCommand { get; private set; }
        public RelayCommand openFilesCommand { get; private set; }

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
            _navigationStore = (Application.Current as App).navigationStore;
            _graphStore = (Application.Current as App).graphStore;

            _graphVisible = true;
            _tableVisible = true;
            _attributesVisible = true;

            layout = Layouts.layouts[(_graphVisible, _tableVisible, _attributesVisible)];

            navigateStartupCommand = new RelayCommand(navigateStartup);
            exitApplicationCommand = new RelayCommand(exitApplication);
            openFilesCommand = new RelayCommand(openFiles);

            _graphStore.graphChanged += onGraphChanged;
        }

        /*
         * Event Handlers
         */
        private void navigateStartup(object message)
        {
            _navigationStore.selectedViewModel = new StartupViewModel();
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

        /*
         * Helper Methods
         */
        private void openFiles(object message)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.Title = "Select Files";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                _graphStore.graph = new Graph(openFileDialog.FileNames);
            }
        }

    }
}

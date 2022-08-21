using Microsoft.Win32;
using PQM_V2.Commands;
using PQM_V2.Models;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.ViewModels.HomeViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly GraphStore _graphStore;
        private LayoutStore _layoutStore;

        public Graph graph => _graphStore.graph;
        public RelayCommand navigateStartupCommand { get; private set; }
        public RelayCommand exitApplicationCommand { get; private set; }
        public RelayCommand openFilesCommand { get; private set; }

        public viewPosition graphPosition => _layoutStore.graphPosition;
        public viewPosition tablePosition => _layoutStore.tablePosition;
        public viewPosition attributesPosition => _layoutStore.attributesPosition;

        public Visibility test;
        public HomeViewModel()
        {
            _navigationStore = (Application.Current as App).navigationStore;
            _graphStore = (Application.Current as App).graphStore;
            _layoutStore = (Application.Current as App).layoutStore;

            _graphStore.graphChanged += onGraphChanged;
            _layoutStore.layoutChanged += onLayoutChanged;

            test = Visibility.Collapsed;

            navigateStartupCommand = new RelayCommand(navigateStartup);
            exitApplicationCommand = new RelayCommand(exitApplication);

            openFilesCommand = new RelayCommand(openFiles);
        }
        private void onGraphChanged()
        {
            onPropertyChanged(nameof(graph));
        }

        private void onLayoutChanged()
        {

        }
        private void navigateStartup(object message)
        {
            _navigationStore.selectedViewModel = new StartupViewModel();
        }
        private void exitApplication(object message)
        {
            System.Windows.Application.Current.Shutdown();
        }
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

using PQM_V2.Stores;
using PQM_V2.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public GraphAttributesStore graphAttributesStore { get; set; }
        public GraphStore graphStore { get; set; }
        public NavigationStore navigationStore { get; set; }
        public SelectedStructureStore selectedStructureStore { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            navigationStore = new NavigationStore();
            graphStore = new GraphStore();
            graphAttributesStore = new GraphAttributesStore();
            selectedStructureStore = new SelectedStructureStore();

            navigationStore.selectedViewModel = new StartupViewModel();
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}

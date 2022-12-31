using PQM_V2.Stores;
using PQM_V2.ViewModels;
using PQM_V2.ViewModels.HomeViewModels;
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
        public GraphCustomizeStore graphCustomizeStore { get; set; }
        public GraphStore graphStore { get; set; }
        public NavigationStore navigationStore { get; set; }
        public CanvasStore canvasStore { get; set; }
        public BaseViewModel homeViewModel { get; set; }
        public BaseViewModel startupViewModel { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            navigationStore = new NavigationStore();
            graphStore = new GraphStore();
            canvasStore = new CanvasStore();

            graphCustomizeStore = new GraphCustomizeStore();

            startupViewModel = new StartupViewModel();
            homeViewModel = new HomeViewModel();
            navigationStore.OnChangePage(CurrentPage.startup);
            
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();
            base.OnStartup(e);
        }

        public void displayMessage(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}

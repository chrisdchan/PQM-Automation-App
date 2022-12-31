using PQM_V2.Models;
using PQM_V2.Stores;
using PQM_V2.ViewModels;
using PQM_V2.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PQM_V2.Views.HomeViews
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        private DispatcherTimer _resizeTimer;
        private CanvasStore _canvasStore;
        private NavigationStore _navigationStore;
        public GraphView()
        {
            InitializeComponent();
            DataContext = new GraphViewModel();

            _canvasStore = (Application.Current as App).canvasStore;
            _navigationStore = (Application.Current as App).navigationStore;
            grid.Children.Add(_canvasStore.canvas);

            _resizeTimer = new DispatcherTimer();
            _resizeTimer.Interval = TimeSpan.FromMilliseconds(100);
            _resizeTimer.IsEnabled = false;
            _resizeTimer.Tick += onResizeTimerEnd;
        }

        private void disposeGraph()
        {
            _resizeTimer.Tick -= onResizeTimerEnd;
        }

        private void onResizeTimerEnd(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;

            GraphViewModel viewModel = (GraphViewModel)DataContext;
            if(viewModel.resizeEndCommand.CanExecute(null))
            {
                viewModel.resizeEndCommand.Execute(null);
            }
        }
        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();

            GraphViewModel viewModel = (GraphViewModel)DataContext;
            if(viewModel.resizeStartCommand.CanExecute(null))
            {
                viewModel.resizeStartCommand.Execute(null);
            }
        }
    }
}



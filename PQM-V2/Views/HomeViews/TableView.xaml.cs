using PQM_V2.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl
    {
        private DispatcherTimer _resizeTimer;
        public TableView()
        {
            InitializeComponent();
            DataContext = new TableViewModel();

            _resizeTimer = new DispatcherTimer();
            _resizeTimer.Interval = TimeSpan.FromMilliseconds(100);
            _resizeTimer.IsEnabled = false;
            _resizeTimer.Tick += onResize;
        }


        private void onResize(object sender, EventArgs e)
        {
            _resizeTimer.IsEnabled = false;
            grid.Visibility = Visibility.Visible;

            double minStructureWidth = 150;
            double evenWidths = 0.95 * grid.ActualWidth / table.Columns.Count;
            double structureColWidth = evenWidths;

            if(evenWidths < minStructureWidth)
            {
                evenWidths = 0.95 * (grid.ActualWidth - minStructureWidth) / (table.Columns.Count - 1);
                structureColWidth = minStructureWidth;
            }

            foreach(GridViewColumn column in table.Columns)
            {
                if (column.Header.ToString() == "Structure")
                    column.Width = structureColWidth;
                else
                    column.Width = evenWidths;
            }
        }
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            grid.Visibility = Visibility.Hidden;
            _resizeTimer.IsEnabled = true;
            _resizeTimer.Stop();
            _resizeTimer.Start();
        }
    }
}

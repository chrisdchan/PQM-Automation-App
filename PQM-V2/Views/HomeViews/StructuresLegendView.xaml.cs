using PQM_V2.Models;
using PQM_V2.ViewModels;
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

namespace PQM_V2.Views.HomeViews
{
    /// <summary>
    /// Interaction logic for StructuresLegendView.xaml
    /// </summary>
    public partial class StructuresLegendView : UserControl
    {
        public StructuresLegendView()
        {
            InitializeComponent();
            DataContext = new StructuresLegendViewModel();
        }

        private void stackPanel_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            StructuresLegendViewModel viewModel = (StructuresLegendViewModel)DataContext;
            if(viewModel.setStructureIndexCommand.CanExecute(null))
            {
                StackPanel sp = (StackPanel)sender;
                viewModel.setStructureIndexCommand.Execute(sp.Tag);
            }
        }
    }
}

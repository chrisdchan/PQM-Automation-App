using PQM_V2.ViewModels.HomeViewModels.AttributePanelViewModels;
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

namespace PQM_V2.Views.HomeViews.AttributePanelViews
{
    /// <summary>
    /// Interaction logic for CustomizePanelView.xaml
    /// </summary>
    public partial class CustomizePanelView : UserControl
            {
        private Boolean enableExpanding;
        public CustomizePanelView()
        {
            InitializeComponent();
            DataContext = new CustomizePanelViewModel();
            enableExpanding = true;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if(enableExpanding)
            {
                enableExpanding = false;
                closeAllExpanders();
                (sender as Expander).IsExpanded = true;
                enableExpanding = true;
            }
        }

        private void closeAllExpanders()
        {
            foreach(Expander expander in stackPanel.Children)
            {
                expander.IsExpanded = false;
            }
        }
    }

}

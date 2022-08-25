using PQM_V2.Stores;
using PQM_V2.ViewModels;
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

namespace PQM_V2.Views
{
    /// <summary>
    /// Interaction logic for StartupView.xaml
    /// </summary>
    public partial class StartupView : UserControl
    {

        private SolidColorBrush _hoverColor;
        private SolidColorBrush _nonHoverColor;
        public StartupView()
        {
            InitializeComponent();
            _hoverColor = new SolidColorBrush(Color.FromRgb(231, 231, 231));
            _nonHoverColor = new SolidColorBrush(Colors.White);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focusable = true;
            Keyboard.Focus(this);
        }

        private void ImageAwesome_MouseEnter(object sender, MouseEventArgs e)
        {
            FontAwesome.WPF.ImageAwesome image = sender as FontAwesome.WPF.ImageAwesome;
            image.Foreground = _hoverColor;
        }

        private void ImageAwesome_MouseLeave(object sender, MouseEventArgs e)
        {
            FontAwesome.WPF.ImageAwesome image = sender as FontAwesome.WPF.ImageAwesome;
            image.Foreground = _nonHoverColor;
        }
    }
}

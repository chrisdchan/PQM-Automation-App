﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PQM_V2.ViewModels.HomeViewModels.AttributePanelViewModels;
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
    /// Interaction logic for StructureGraphAttributesView.xaml
    /// </summary>
    public partial class StructureGraphAttributesView : UserControl
    {
        public StructureGraphAttributesView()
        {
            InitializeComponent();
            DataContext = new StructureGraphAttributesViewModel();
        }
    }
}

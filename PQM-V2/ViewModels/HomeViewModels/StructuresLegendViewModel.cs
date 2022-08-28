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
using System.Windows.Media;

namespace PQM_V2.ViewModels.HomeViewModels
{
    public class LegendItemViewModel
    {
        public string name { get; set; }
        public SolidColorBrush color { get; set; }
        public Visibility eye { get; set; }
        public Visibility eyeSlash { get; set; }
    }

    public class StructuresLegendViewModel
    {
        private readonly GraphStore _graphStore;

        private readonly ObservableCollection<LegendItemViewModel> _legendItemsList;
        public ObservableCollection<LegendItemViewModel> LegendItemsList => _legendItemsList;

        private RelayCommand changeVisibilityCommand; 

        public StructuresLegendViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;
            _legendItemsList = new ObservableCollection<LegendItemViewModel>();

            loadGraph();

            changeVisibilityCommand = new RelayCommand(changeVisibility);

            _graphStore.graphChanged += loadGraph;
        }

        private void loadGraph()
        {
            _legendItemsList.Clear();
            foreach(Structure structure in _graphStore.graph.structures)
            {
                addStructure(structure);
            }
        }
        private void addStructure(Structure structure)
        {
            LegendItemViewModel itemViewModel = new LegendItemViewModel
            {
                name = structure.name,
                color = structure.color,
                eye = Visibility.Visible,
                eyeSlash = Visibility.Hidden
            };
            _legendItemsList.Add(itemViewModel);
        }

        private void changeVisibility(object message)
        {
            
        }
    }


}

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
    public class StructuresLegendViewModel
    {
        private readonly GraphStore _graphStore;

        private readonly ObservableCollection<Structure> _structureList;
        public ObservableCollection<Structure> structureList => _structureList;

        public RelayCommand changeVisibilityCommand { get; private set; }

        public StructuresLegendViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;
            _structureList = new ObservableCollection<Structure>();

            loadGraph();

            changeVisibilityCommand = new RelayCommand(changeVisibility);

            _graphStore.graphChanged += loadGraph;
        }

        private void loadGraph()
        {
            _structureList.Clear();
            foreach(Structure structure in _graphStore.graph.structures)
            {
                addStructure(structure);
            }
        }
        private void addStructure(Structure structure)
        {
            _structureList.Add(structure);
        }

        private void changeVisibility(object param)
        {

        }

    }


}

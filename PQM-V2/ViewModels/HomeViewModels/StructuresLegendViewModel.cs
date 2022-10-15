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
    public class StructuresLegendViewModel : BaseViewModel
    {
        private readonly GraphStore _graphStore;

        private readonly ObservableCollection<Structure> _structureList;
        public ObservableCollection<Structure> structureList => _structureList;

        public RelayCommand changeVisibilityCommand { get; private set; }
        public RelayCommand isolateCommand { get; private set; }
        public RelayCommand selectStructureCommand { get; private set; }

        public StructuresLegendViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;

            _structureList = new ObservableCollection<Structure>();

            loadGraph();

            changeVisibilityCommand = new RelayCommand(changeVisibility);
            isolateCommand = new RelayCommand(isolate);
            selectStructureCommand = new RelayCommand(selectStructure);

            _graphStore.graphChanged += loadGraph;
            _graphStore.graphUpdated += loadGraph;
            _graphStore.selectedStructureChanged += loadGraph;
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
            if(param != null)
            {
                int index = (int)param;
                Structure structure = _graphStore.graph.structures[index];
                structure.visible = !structure.visible;
                _graphStore.onGraphUpdated();
                onPropertyChanged(nameof(structureList));
            }
        }
        private void isolate(object param)
        {
            if(param != null)
            {
                int index = (int)param;
                _graphStore.graph.isolate(index);
                _graphStore.onGraphUpdated();
                onPropertyChanged(nameof(structureList));
            }
        }
        private void selectStructure(object param)
        {
            if(param != null)
            {
                int index = (int)param;
                _graphStore.graph.selectStructure(index);
                _graphStore.onSelectedStructureChanged();
                onPropertyChanged(nameof(structureList));
            }
        }
    }


}

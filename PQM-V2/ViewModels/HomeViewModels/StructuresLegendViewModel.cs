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
        private int structureIndex;

        public RelayCommand setStructureIndexCommand { get; private set; }
        public RelayCommand changeVisibilityCommand { get; private set; }
        public RelayCommand isolateStructureCommand { get; private set; }
        public RelayCommand selectStructureCommand { get; private set; }
        public RelayCommand deleteStructureCommand { get; private set; }

        public StructuresLegendViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;

            _structureList = new ObservableCollection<Structure>();

            loadGraph();

            changeVisibilityCommand = new RelayCommand(changeVisibility);
            isolateStructureCommand = new RelayCommand(isolateStructure);
            selectStructureCommand = new RelayCommand(selectStructure);
            setStructureIndexCommand = new RelayCommand(setStructureIndex);

            _graphStore.graphChanged += loadGraph;
            _graphStore.graphUpdated += loadGraph;
            _graphStore.selectedStructureChanged += loadGraph;
            structureIndex = 0;
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
            int index = (param == null) ? structureIndex : (int)param;
            Structure structure = _graphStore.graph.structures[index];
            setVisibility(structure, !structure.visible);
        }
        private void setVisibility(Structure structure, bool isVisible)
        {
            structure.visible = isVisible;
            _graphStore.onGraphUpdated();
            onPropertyChanged(nameof(structureList));
        }
        private void isolateStructure(object param)
        {
            int index = (param == null) ? structureIndex : (int)param;
            _graphStore.graph.isolate(index);
            _graphStore.onGraphUpdated();
            onPropertyChanged(nameof(structureList));
        }
        private void selectStructure(object param)
        {
            int index = (param == null) ? structureIndex : (int)param;
            _graphStore.graph.selectStructure(index);
            setVisibility(_graphStore.graph.selectedStructure, true);
            _graphStore.onSelectedStructureChanged();
            onPropertyChanged(nameof(structureList));
        }
        private void setStructureIndex(object param)
        {
            structureIndex = (int)param;
        }
    }


}

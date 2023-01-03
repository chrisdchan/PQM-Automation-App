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
    public class StructureView : BaseViewModel
    {
        public const string AVALIBLE_COLOR = "#000000";
        public const string UNAVALIBLE_COLOR = "#000000";

        private string _showMenuColor;
        private string _hideMenuColor;
        public Structure structure { get; set; }

        public string showMenuColor { get => _showMenuColor; set { _showMenuColor = value; onPropertyChanged(nameof(showMenuColor)); }}
        public string hideMenuColor { get => _hideMenuColor; set { _hideMenuColor = value; onPropertyChanged(nameof(hideMenuColor)); }}

        public StructureView(Structure structure)
        {
            this.structure = structure;
            showMenuColor = UNAVALIBLE_COLOR;
            hideMenuColor = AVALIBLE_COLOR;
        }

    }
    public class StructuresLegendViewModel : BaseViewModel
        {
        private readonly GraphStore _graphStore;

        private readonly ObservableCollection<StructureView> _structureViewList;
        public ObservableCollection<StructureView> structureViewList => _structureViewList;
        private int structureIndex;

        public RelayCommand setStructureIndexCommand { get; private set; }
        public RelayCommand changeVisibilityCommand { get; private set; }
        public RelayCommand isolateStructureCommand { get; private set; }
        public RelayCommand selectStructureCommand { get; private set; }
        public RelayCommand deleteStructureCommand { get; private set; }

        public StructuresLegendViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;

            _structureViewList = new ObservableCollection<StructureView>();

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
            _structureViewList.Clear();
            foreach(Structure structure in _graphStore.graph.structures)
            {
                addStructure(structure);
            }
        }
        private void addStructure(Structure structure)
        {
            _structureViewList.Add(new StructureView(structure));
        }

        private void changeVisibility(object param)
        {
            int index = (param == null) ? structureIndex : (int)param;
            StructureView view = _structureViewList[index];
            setVisibility(view, !view.structure.visible);
        }
        private void setVisibility(StructureView view, bool isVisible)
        {
            view.structure.visible = isVisible;
            view.showMenuColor = (isVisible) ? StructureView.UNAVALIBLE_COLOR : StructureView.AVALIBLE_COLOR;
            view.hideMenuColor = (isVisible) ? StructureView.AVALIBLE_COLOR : StructureView.UNAVALIBLE_COLOR;

            _graphStore.onGraphUpdated();
            onPropertyChanged(nameof(structureViewList));
        }
        private void isolateStructure(object param)
        {
            int index = (param == null) ? structureIndex : (int)param;
            _graphStore.graph.isolate(index);
            _graphStore.onGraphUpdated();
            onPropertyChanged(nameof(structureViewList));
        }
        private void selectStructure(object param)
        {
            int index = (param == null) ? structureIndex : (int)param;
            _graphStore.graph.selectStructure(index);
            _graphStore.onSelectedStructureChanged();
            onPropertyChanged(nameof(structureViewList));
        }
        private void setStructureIndex(object param)
        {
            structureIndex = (int)param;
        }
    }


}

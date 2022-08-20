using PQM_V2.Models;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PQM_V2.ViewModels.HomeViewModels
{
    public class TableItemViewModel
    {
        public string name { get; set; }
        public double area100 { get; set; }
        public double area95{ get; set; }
        public double area90{ get; set; }
        public double area50{ get; set; }
        public double area5{ get; set; }
        public double cc{ get; set; }
    }
    public class TableViewModel : BaseViewModel
    {
        private readonly GraphStore _graphStore;

        private readonly ObservableCollection<TableItemViewModel> _tableRowsList;
        public ObservableCollection<TableItemViewModel> tableRowsList => _tableRowsList;
        public string testBind { get; set; }
        public TableViewModel()
        {
            _graphStore = (Application.Current as App).graphStore;

            _tableRowsList = new ObservableCollection<TableItemViewModel>();

            loadGraph();

            _graphStore.graphChanged += loadGraph;
        }

        private void loadGraph()
        {
            foreach(Structure structure in _graphStore.graph.structures)
            {
                addRow(structure);
            }
        }
        private void addRow(Structure structure)
        {
            TableItemViewModel row = new TableItemViewModel
            {
                name = structure.name,
                area100 = Math.Round( structure.aucFromY(100), 2),
                area95 = Math.Round( structure.aucFromY(95), 2),
                area90 = Math.Round( structure.aucFromY(90), 2),
                area50 = Math.Round( structure.aucFromY(50), 2),
                area5 = Math.Round( structure.aucFromY(5), 2),
                cc = 30,
            };
            _tableRowsList.Add(row);
        }

    }
}

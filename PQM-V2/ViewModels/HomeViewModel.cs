using PQM_V2.Commands;
using PQM_V2.Models;
using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly GraphStore _graphStore;

        public Graph graph => _graphStore.graph;
        public RelayCommand navigateStartupCommand { get; private set; }
        public RelayCommand exitApplicationCommand { get; private set; }
        public StructuresLegendViewModel structuresLegendViewModel { get; set; }
        public HomeViewModel(NavigationStore navigationStore, GraphStore graphStore)
        {
            _navigationStore = navigationStore;
            _graphStore = graphStore;


            _graphStore.graphChanged += onGraphChanged;

            structuresLegendViewModel = new StructuresLegendViewModel(graphStore);
            navigateStartupCommand = new RelayCommand(navigateStartup);
            exitApplicationCommand = new RelayCommand(exitApplication);
        }
        private void onGraphChanged()
        {
            onPropertyChanged(nameof(graph));
        }
        private void navigateStartup(object message)
        {
            _navigationStore.selectedViewModel = new StartupViewModel(_navigationStore, _graphStore);
        }
        private void exitApplication(object message)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}

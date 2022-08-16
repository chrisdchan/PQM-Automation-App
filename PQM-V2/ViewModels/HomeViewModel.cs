using PQM_V2.Commands;
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
        public string variable { get; set; }
        public RelayCommand navigateStartupCommand { get; private set; }
        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            variable = "hello";

            navigateStartupCommand = new RelayCommand(navigateStartup);
        }
        private void navigateStartup(object message)
        {
            _navigationStore.selectedViewModel = new StartupViewModel(_navigationStore);
        }
    }
}

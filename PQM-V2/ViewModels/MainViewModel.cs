using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public BaseViewModel selectedViewModel => _navigationStore.selectedViewModel;
        public MainViewModel() 
        {
            _navigationStore = (Application.Current as App).navigationStore;
            _navigationStore.selectedViewModelChanged += onSelectedViewModelChanged;
        }
        private void onSelectedViewModelChanged()
        {
            onPropertyChanged(nameof(selectedViewModel));
        }
    }
}

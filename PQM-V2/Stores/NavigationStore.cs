using PQM_V2.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Stores
{
    public class NavigationStore
    {
        public event Action selectedViewModelChanged;

        private BaseViewModel _selectedViewModel;
        public BaseViewModel selectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnSelectedViewModelChanged();
            }
        }
        private void OnSelectedViewModelChanged()
        {
            selectedViewModelChanged?.Invoke();
        }

    }
}

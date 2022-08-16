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
        public event Action SelectedViewModelChanged;
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnSelectedViewModelChanged();
            }
        }

        private void OnSelectedViewModelChanged()
        {
            SelectedViewModelChanged?.Invoke();
        }

    }
}

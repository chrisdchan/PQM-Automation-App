using PQM_V2.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.Stores
{
    public enum CurrentPage { home, startup };
    public class NavigationStore
    {
        public event Action selectedViewModelChanged;

        private BaseViewModel _selectedViewModel;
        public BaseViewModel selectedViewModel
        {
            get => _selectedViewModel;
            private set
            {
                _selectedViewModel = value;
                OnSelectedViewModelChanged();
            }
        }

        public void OnChangePage(CurrentPage page)
        {
            if(page == CurrentPage.home)
            {
                (Application.Current as App).canvasStore.canvas = new System.Windows.Controls.Canvas();
                selectedViewModel = (Application.Current as App).homeViewModel;
            }
            else if(page == CurrentPage.startup)
            {
                selectedViewModel = (Application.Current as App).startupViewModel;
            }
        }

        private void OnSelectedViewModelChanged()
        {
            selectedViewModelChanged?.Invoke();
        }


    }
}

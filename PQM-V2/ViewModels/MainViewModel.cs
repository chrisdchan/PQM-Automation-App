using PQM_V2.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public BaseViewModel selectedViewModel { get; set; }
        public MainViewModel() 
        {
            selectedViewModel = new StartupViewModel();
        }
    }
}

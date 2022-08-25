using PQM_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Stores
{
    public class SelectedStructureStore
    {
        public event Action selectedStructuresChanged;

        private List<Structure> _selectedStructures;

        public List<Structure> selectedStructures
        {
            get => _selectedStructures;
            set
            {
                _selectedStructures = value;
                onSelectedStructureChanged();
            }
        }

        private void onSelectedStructureChanged()
        {
            selectedStructuresChanged?.Invoke();
        }
    }
}

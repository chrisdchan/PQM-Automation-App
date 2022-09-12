using PQM_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Stores
{
    public class GraphStore
    {
        public event Action graphChanged;
        public event Action graphUpdated;

        private Graph _graph;
        public Graph graph
        {
            get => _graph;
            set
            {
                _graph = value;
                onGraphChanged();
            }
        }
        private void onGraphChanged()
        {
            graphChanged?.Invoke();
        }

        public void onGraphUpdated()
        {
            graphUpdated?.Invoke();
        }

    }
}

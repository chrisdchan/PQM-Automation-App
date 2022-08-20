using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.Stores
{
    public class viewPosition
    {
        public int row { get; set; }
        public int col { get; set; }
        public int rowSpan { get; set; }
        public int colSpan { get; set; }
        public bool visibility { get; set; }
    }
    public class LayoutStore
    {
        public event Action layoutChanged;

        private viewPosition _graphPosition;
        private viewPosition _tablePosition;
        private viewPosition _attributesPoistion;
        public viewPosition graphPosition { 
            get => _graphPosition;
            set { _graphPosition = value; onLayoutChanged(); } 
        }
        public viewPosition tablePosition { 
            get => _tablePosition;
            set { _tablePosition = value; onLayoutChanged(); } 
        }
        public viewPosition attributesPosition { 
            get => _attributesPoistion;
            set { _attributesPoistion = value; onLayoutChanged(); } 
        }

        public LayoutStore()
        {
            _graphPosition = new viewPosition()
            {
                row = 2,
                col = 2,
                colSpan = 1,
                rowSpan = 1,
                visibility = true
            };
            _tablePosition = new viewPosition()
            {
                row = 2,
                col = 1,
                colSpan = 1,
                rowSpan = 1,
                visibility = true
            };
            _attributesPoistion = new viewPosition()
            {
                row = 1,
                col = 2,
                colSpan = 1,
                rowSpan = 2,
                visibility = true
            };
        }

        private void onLayoutChanged()
        {
            layoutChanged?.Invoke();
        }

    }
}

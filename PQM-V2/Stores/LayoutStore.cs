using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.Stores
{
    public class viewPosition
    {
        public event Action positionChanged;
        private int _row;
        private int _column;
        private int _rowSpan;
        private int _colSpan;
        private bool _visibility;
        public int row { get => _row; set { _row = value; onPositionChanged(); } }
        public int column { get => _column; set { _column = value; onPositionChanged(); } }
        public int rowSpan { get => _rowSpan; set { _rowSpan = value; onPositionChanged(); } }
        public int colSpan { get => _colSpan; set { _colSpan = value; onPositionChanged(); } }
        public bool visibility { get => _visibility; set { _visibility = value; onPositionChanged(); } }

        private void onPositionChanged()
        {
            positionChanged?.Invoke();
        }

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
                column = 2,
                colSpan = 1,
                rowSpan = 1,
                visibility = true
            };
            _tablePosition = new viewPosition()
            {
                row = 2,
                column = 1,
                colSpan = 1,
                rowSpan = 1,
                visibility = false
            };
            _attributesPoistion = new viewPosition()
            {
                row = 1,
                column = 2,
                colSpan = 1,
                rowSpan = 2,
                visibility = true
            };

            _graphPosition.positionChanged += onLayoutChanged;
            _tablePosition.positionChanged += onLayoutChanged;
            _attributesPoistion.positionChanged += onLayoutChanged;

        }

        private void onLayoutChanged()
        {
            layoutChanged?.Invoke();
        }

    }
}

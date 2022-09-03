using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PQM_V2.Stores
{
    public class CanvasStore
    {
        public event Action canvasChanged;

        private Canvas _canvas;
        public Canvas canvas
        {
            get => _canvas;
            set => _canvas = value;
        }

        public CanvasStore()
        {
            _canvas = new Canvas();
        }

        public void onCanvasChanged()
        {
            canvasChanged?.Invoke();
        }
    }
}

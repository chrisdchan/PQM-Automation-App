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
        public event Action prepareExport;
        public enum ProbeTypes { none, x, y };

        private Canvas _canvas;
        private Canvas _displayCanvas;
        private ProbeTypes _probeType;
        public Canvas canvas
        {
            get => _canvas;
            set => _canvas = value;
        }
        public Canvas displayCanvas
        {
            get => _displayCanvas;
            set => _displayCanvas = value;
        }
        public ProbeTypes probeType
        {
            get => _probeType;
            set {
                _probeType = value;
                onCanvasChanged();
            }
        }

        public CanvasStore()
        {
            canvas = new Canvas();
            probeType = ProbeTypes.x;
        }

        public void onPrepareExport()
        {
            prepareExport?.Invoke();
        }
        public void onCanvasChanged()
        {
            canvasChanged?.Invoke();
        }
    }
}

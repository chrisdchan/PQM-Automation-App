using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Stores
{
    public class GraphCustomizeStore
    {
        public event Action graphCustomizeChanged;

        public double xmin { get; set; }
        public double xmax { get; set; }
        public double dpi { get; set; }
        public int numPoints { get; set; }

        public int numXAxisTicks {get; set;}
        public int numYAxisTicks {get; set;}
        public int axesThickness {get; set;}
        public int axesTickSize {get; set;}

        public int titleSize {get; set;}
        public int titleLeftOffset {get; set;}
        public int titleTopOffset {get; set;}
        public bool titleBold { get; set; }
        public bool titleItalic { get; set; }

        public int xAxisTitleSize {get; set;}
        public int xAxisTitleLeftOffset {get; set;}
        public int xAxisTitleTopOffset {get; set;}
        public bool xAxisTitleBold { get; set; }
        public bool xAxisTitleItalic { get; set; }

        public int yAxisTitleSize {get; set;}
        public int yAxisTitleLeftOffset {get; set;}
        public int yAxisTitleTopOffset {get; set;}
        public bool yAxisTitleBold { get; set; }
        public bool yAxisTitleItalic { get; set; }

        public string backgroundColor {get; set;}
        public string foregroundColor {get; set;}
        public int legendSize { get; set; }

        public GraphCustomizeStore()
        {
            xmin = 0;
            xmax = 40;
            dpi = 600;
            numPoints = 75;

            numXAxisTicks = 5;
            numYAxisTicks = 5;
            axesThickness = 1;
            axesTickSize = 12;

            titleLeftOffset = 0;
            titleTopOffset = 0;
            titleSize = 16;
            titleBold = true;
            titleItalic = false;

            xAxisTitleLeftOffset = 0;
            xAxisTitleTopOffset = 0;
            xAxisTitleSize = 14;
            xAxisTitleBold = false;
            xAxisTitleItalic = false;

            yAxisTitleLeftOffset = 0;
            yAxisTitleTopOffset = 0;
            yAxisTitleSize = 14;
            yAxisTitleBold = false;
            yAxisTitleItalic = false;

            backgroundColor = "#ffffff";
            foregroundColor = "#000000";

            legendSize = 14;
        }

        public void onGraphCustomizeChanged()
        {
            graphCustomizeChanged?.Invoke();
        }
    }
}

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
        public int numPoints { get; set; }

        public int numXAxisTicks {get; set;}
        public int numYAxisTicks {get; set;}
        public int axesThickness {get; set;}
        public int axesTickSize {get; set;}

        public int titleSize {get; set;}
        public int titleLeftOffset {get; set;}
        public int titleTopOffset {get; set;}

        public int xAxisTitleSize {get; set;}
        public int xAxisTitleLeftOffset {get; set;}
        public int xAxisTitleTopOffset {get; set;}

        public int yAxisTitleSize {get; set;}
        public int yAxisTitleLeftOffset {get; set;}
        public int yAxisTitleTopOffset {get; set;}

        public string backgroundColor {get; set;}
        public string foregroundColor {get; set;}
        public int legendSize { get; set; }

        public GraphCustomizeStore()
        {
            xmin = 0;
            xmax = 40;
            numPoints = 75;

            numXAxisTicks = 5;
            numYAxisTicks = 5;
            axesThickness = 1;
            axesTickSize = 12;

            titleLeftOffset = 0;
            titleTopOffset = 0;
            titleSize = 16;

            yAxisTitleLeftOffset = 0;
            yAxisTitleTopOffset = 0;
            xAxisTitleLeftOffset = 0;
            xAxisTitleTopOffset = 0;
            xAxisTitleSize = 14;
            yAxisTitleSize = 14;

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

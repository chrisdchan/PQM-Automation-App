using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQM_V2.Stores
{
    public enum VerticalAlignmentType
    {
        Top,
        Bottom,
        Center,
        Custom
    }
    public class LegendSettingsStore
    {
        public event Action legendSettingsChanged;
        public double fontSize { get; set; }
        public double horizontalMargin { get; set; }
        public double verticalMargin { get; set; }
        public VerticalAlignmentType verticalAlignmentType { get; set; }
        public double topOffset { get; set; }
        public LegendSettingsStore()
        {
            fontSize = 10;
            horizontalMargin = 20;
            verticalMargin = 5;
            verticalAlignmentType = VerticalAlignmentType.Top;
            topOffset = 20;
        }

    }
}

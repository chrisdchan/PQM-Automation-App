using PQM_V2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PQM_V2.Stores
{
    public class Position
    {
        public int row { get; set; }
        public int column { get; set; }
        public int rowSpan { get; set; }
        public int colSpan { get; set; }
        public Position(int row, int column, int rowSpan, int colSpan)
        {
            this.row = row;
            this.column = column;
            this.rowSpan = rowSpan;
            this.colSpan = colSpan;
        }
    }
    public class Positions
    {
        public static Position middle =         new Position(1, 2, 3, 1);
        public static Position middleTop =      new Position(1, 2, 1, 1);
        public static Position middleBottom =   new Position(3, 2, 1, 1);
        public static Position top =            new Position(1, 2, 1, 3);
        public static Position bottom =         new Position(3, 2, 1, 3);
        public static Position right =          new Position(1, 4, 3, 1);
        public static Position whole =          new Position(1, 2, 3, 3);
        public static Position none =           new Position(0, 0, 0, 0);

        public static Position rowMiddle =      new Position(2, 2, 1, 1);
        public static Position rowWhole =       new Position(2, 2, 1, 3);
        public static Position colWhole =       new Position(1, 3, 3, 1);
    }
    public static class Layouts
    {
        public static Dictionary<(bool, bool, bool), Layout> layouts = new Dictionary<(bool graph, bool table, bool attributes), Layout>()
        {
            { (true, true, true), new Layout(Positions.middleTop, Positions.middleBottom, Positions.right, Positions.rowMiddle, Positions.colWhole) },
            { (true, false, false), new Layout(Positions.whole, Positions.none, Positions.none, Positions.none, Positions.none) },
            { (false, true, false), new Layout(Positions.none, Positions.whole, Positions.none, Positions.none, Positions.none) },
            { (false, false, true), new Layout(Positions.none, Positions.none, Positions.whole, Positions.none, Positions.none) },
            { (true, true, false), new Layout(Positions.top, Positions.bottom, Positions.none, Positions.rowWhole, Positions.none) },
            { (false, true, true), new Layout(Positions.none, Positions.middle, Positions.right, Positions.none, Positions.colWhole) },
            { (true, false, true), new Layout(Positions.middle, Positions.none, Positions.right, Positions.none, Positions.colWhole) },
            { (false, false, false), new Layout(Positions.none, Positions.none, Positions.whole, Positions.none, Positions.none) },
        };
    }
    public class Layout
    {
        public Position graph{ get; set; }
        public Position table{ get; set; }
        public Position attributes{ get; set; }
        public Position rowSplit{ get; set; }
        public Position colSplit{ get; set; }
        public bool rowVisibility { get; private set; }
        public bool colVisibility { get; private set; }
        public Layout(Position graph, Position table, Position attributes, Position rowSplit, Position colSplit)
        {
            this.graph = graph;
            this.table = table;
            this.attributes = attributes;
            this.rowSplit = rowSplit;
            this.colSplit = colSplit;

            rowVisibility = (rowSplit == Positions.none)? false : true;
            colVisibility = (colSplit == Positions.none)? false : true;
        }
    }


}

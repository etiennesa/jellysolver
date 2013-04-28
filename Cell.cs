using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    struct Cell : IEquatable<Cell>
    {
        public CellType Type { get; private set; }
        public Color Color { get; private set; }

        public Cell(CellType type, Color color)
            : this()
        {
            Type = type; Color = color;
        }

        public Cell Clone()
        {
            return new Cell(Type, Color);
        }

        public static Cell WallCell = new Cell(CellType.Wall, Color.None);
        public static Cell EmptyCell = new Cell(CellType.Empty, Color.None);
        public static Cell RedCell = new Cell(CellType.Jelly, Color.Red);
        public static Cell GreenCell = new Cell(CellType.Jelly, Color.Green);
        public static Cell YellowCell = new Cell(CellType.Jelly, Color.Yellow);

        public override bool Equals(object obj)
        {
            return (obj is Cell && Equals((Cell)obj));
        }

        public bool Equals(Cell other)
        {
            return (Type.Equals(other.Type) && Color.Equals(other.Color));
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            if (Equals(WallCell))
                return "w";
            else if (Equals(EmptyCell))
                return "-";
            else if (Equals(RedCell))
                return "r";
            else if (Equals(YellowCell))
                return "y";
            else if (Equals(GreenCell))
                return "g";

            throw new Exception("unknown " + Type.ToString() + " ; " + Color.ToString());
        }
    }

    public class T_Cell
    {

        public void Run()
        {
            GlobalConfig.Writer.Write("Testing T_Cell...");

            // Equals
            Debug.Assert(Cell.WallCell.Equals(Cell.WallCell));
            Debug.Assert(!Cell.WallCell.Equals(Cell.EmptyCell));
            Debug.Assert(!Cell.WallCell.Equals(Cell.RedCell));
            Debug.Assert(Cell.EmptyCell.Equals(Cell.EmptyCell));
            Debug.Assert(Cell.RedCell.Equals(Cell.RedCell));
            Debug.Assert(Cell.YellowCell.Equals(Cell.YellowCell));
            Debug.Assert(!Cell.RedCell.Equals(Cell.YellowCell));

            // Cell Clone()
            Debug.Assert(Cell.WallCell.Clone().Equals(Cell.WallCell));
            Debug.Assert(!Cell.WallCell.Clone().Equals(Cell.EmptyCell));
            Debug.Assert(!Cell.WallCell.Clone().Equals(Cell.RedCell));
            Debug.Assert(Cell.EmptyCell.Clone().Equals(Cell.EmptyCell));
            Debug.Assert(Cell.RedCell.Clone().Equals(Cell.RedCell));
            Debug.Assert(Cell.YellowCell.Clone().Equals(Cell.YellowCell));
            Debug.Assert(!Cell.RedCell.Clone().Equals(Cell.YellowCell));

            // ToString()
            Debug.Assert(Cell.WallCell.ToString().Equals("w"));
            Debug.Assert(Cell.EmptyCell.ToString().Equals("-"));
            Debug.Assert(Cell.RedCell.ToString().Equals("r"));
            Debug.Assert(Cell.YellowCell.ToString().Equals("y"));
            Debug.Assert(Cell.GreenCell.ToString().Equals("g"));

            GlobalConfig.Writer.WriteLine("finished");
        }
    }

    enum Color
    {
        None,
        Black,
        Red,
        Green,
        Yellow,
    }

    enum CellType
    {
        Wall,
        Jelly,
        Empty,
    }

}

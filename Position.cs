using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class Move
    {
        public int I { get; private set; }
        public int J { get; private set; }

        public Move(int i, int j)
        {
            I = i;
            J = j;
        }

        public static Move LeftMove = new Move(-1, 0);
        public static Move DownMove = new Move(0, 1);
        public static Move RightMove = new Move(1, 0);
    }

    struct Position
    {
        public int I { get; private set; }
        public int J { get; private set; }

        public Position(int i, int j)
            : this()
        {
            I = i;
            J = j;
        }

        public bool IsNeighbour(Position position)
        {
            return ((I == position.I && (J == position.J + 1 || J == position.J - 1))
                || (J == position.J && (I == position.I + 1 || I == position.I - 1)));
        }

        public Position GetPosition(Move move)
        {
            return new Position(I + move.I, J + move.J);
        }

        public override bool Equals(object obj)
        {
            return (obj is Position && Equals((Position)obj));
        }

        public bool Equals(Position other)
        {
            return (I == other.I && J == other.J);
        }

        public override int GetHashCode()
        {
            return I.GetHashCode() * 1000 + J.GetHashCode();
        }

        public override string ToString()
        {
            return I + ";" + J;
        }
    }

    public class T_Position
    {
        public void Run()
        {
            GlobalConfig.Writer.Write("Testing T_Position...");

            Position pos1 = new Position(5, 3);
            Position pos2 = new Position(5, 3);
            Position pos3 = new Position(3, 5);
            Position pos4 = new Position(5, 4);
            Position pos5 = new Position(5, 2);
            Position pos6 = new Position(4, 3);
            Position pos7 = new Position(6, 3);

            // IsNeighbour
            Debug.Assert(!pos1.IsNeighbour(pos2));
            Debug.Assert(!pos1.IsNeighbour(pos3));
            Debug.Assert(pos1.IsNeighbour(pos4));
            Debug.Assert(pos1.IsNeighbour(pos5));
            Debug.Assert(pos1.IsNeighbour(pos6));
            Debug.Assert(pos1.IsNeighbour(pos7));

            // Equals
            Debug.Assert(pos1.Equals(pos1));
            Debug.Assert(pos1.Equals(pos2));
            Debug.Assert(!pos1.Equals(pos3));
            Debug.Assert(!pos1.Equals(pos4));
            Debug.Assert(!pos1.Equals(pos6));

            // ToString()
            Debug.Assert(pos1.ToString().Equals("5;3"));

            GlobalConfig.Writer.WriteLine("finished");
        }
    }

}

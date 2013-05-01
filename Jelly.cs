using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class Jelly : IEquatable<Jelly>
    {
        public Color Color { get; private set; }
        public HashSet<Position> Positions { get; private set; }

        public Jelly(Color color, HashSet<Position> positions)
        {
            Color = color;
            Positions = positions;
        }

        public override bool Equals(object obj)
        {
            return (obj is Jelly && Equals((Jelly)obj));
        }

        public bool Equals(Jelly other)
        {
            if (Color != other.Color || Positions.Count != other.Positions.Count)
                return false;

            foreach (Position pos in Positions)
                if (!other.Positions.Contains(pos))
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            // profiler : too long
            // return ToString().GetHashCode();

            int hash = 0;

            hash = (int)Color * 100000;

            foreach (Position pos in Positions)
            {
                hash += pos.I * 1000 + pos.J;
            }

            return hash;
        }

        public override string ToString()
        {
            StringBuilder bld = new StringBuilder();
            bld.Append(Color.ToString() + ";");

            foreach (Position pos in Positions)
                bld.Append("{" + pos.ToString() + "};");

            return bld.ToString();
        }

        public void Move(Move move)
        {
            HashSet<Position> poss = new HashSet<Position>();

            foreach (Position pos in Positions)
                poss.Add(pos.GetPosition(move));

            Positions = poss;
        }

        public bool IsNeighbour(Jelly jelly)
        {
            // For every cell, search if there is a neighbour in the other jelly
            foreach (Position pos in jelly.Positions)
                if (IsNeighbour(pos))
                    return true;

            return false;
        }

        private bool IsNeighbour(Position pos)
        {
            foreach (Position position in Positions)
                if (position.IsNeighbour(pos))
                    return true;

            return false;
        }

        public void Merge(Jelly jelly)
        {
            foreach (Position pos in jelly.Positions)
                Positions.Add(pos);
        }

        public Jelly Clone()
        {
            HashSet<Position> hashPos = new HashSet<Position>(Positions);
            return new Jelly(Color, hashPos);
        }
    }

    class T_Jelly
    {
        public void Run()
        {
            GlobalConfig.Writer.Write("Testing T_Jelly...");

            HashSet<Position> hashPos0 = new HashSet<Position>();
            hashPos0.Add(new Position(3, 5));
            hashPos0.Add(new Position(2, 7));
            Jelly jelly0 = new Jelly(Color.Red, hashPos0);
            HashSet<Position> hashPos1 = new HashSet<Position>();
            hashPos1.Add(new Position(3, 5));
            hashPos1.Add(new Position(2, 7));
            Jelly jelly1 = new Jelly(Color.Red, hashPos1);
            HashSet<Position> hashPos2 = new HashSet<Position>();
            hashPos2.Add(new Position(3, 6));
            hashPos2.Add(new Position(10, 10));
            Jelly jelly2 = new Jelly(Color.Green, hashPos2);
            HashSet<Position> hashPos3 = new HashSet<Position>();
            hashPos3.Add(new Position(10, 10));
            hashPos3.Add(new Position(1, 7));
            Jelly jelly3 = new Jelly(Color.Green, hashPos3);
            HashSet<Position> hashPos4 = new HashSet<Position>();
            hashPos4.Add(new Position(10, 10));
            hashPos4.Add(new Position(10, 10));
            Jelly jelly4 = new Jelly(Color.Green, hashPos4);

            // ToString
            Debug.Assert(jelly1.ToString().Equals("Red;{3;5};{2;7};"));

            // Equals
            Debug.Assert(jelly0.Equals(jelly0));
            Debug.Assert(jelly0.Equals(jelly1));
            Debug.Assert(!jelly0.Equals(jelly2));

            // Move
            jelly1.Move(Move.LeftMove);
            Debug.Assert(jelly1.Positions.Count.Equals(2));
            Debug.Assert(jelly1.Positions.Contains(new Position(2, 5)));
            Debug.Assert(jelly1.Positions.Contains(new Position(1, 7)));
            jelly1.Move(Move.RightMove);
            Debug.Assert(jelly1.Positions.Count.Equals(2));
            Debug.Assert(jelly1.Positions.Contains(new Position(3, 5)));
            Debug.Assert(jelly1.Positions.Contains(new Position(2, 7)));

            // IsNeighbour
            Debug.Assert(!jelly1.IsNeighbour(jelly1));
            Debug.Assert(jelly1.IsNeighbour(jelly2));
            Debug.Assert(jelly1.IsNeighbour(jelly3));
            Debug.Assert(!jelly1.IsNeighbour(jelly4));

            // Merge
            jelly1.Merge(jelly2);
            Debug.Assert(jelly1.Color.Equals(Color.Red));
            Debug.Assert(jelly1.Positions.Count.Equals(4));
            Debug.Assert(jelly1.Positions.Contains(new Position(3, 5)));
            Debug.Assert(jelly1.Positions.Contains(new Position(2, 7)));

            GlobalConfig.Writer.WriteLine("finished");
        }
    }
}

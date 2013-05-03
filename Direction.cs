using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class Direction
    {
        public int I { get; private set; }
        public int J { get; private set; }

        public Direction(int i, int j)
        {
            I = i;
            J = j;
        }

        public static Direction LeftMove = new Direction(-1, 0);
        public static Direction DownMove = new Direction(0, 1);
        public static Direction RightMove = new Direction(1, 0);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class LevelLoader
    {
        private string[] game0 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W W W W W W W W W W W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game1 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . r . . . . W",
            "                           ",
            "W . . . . . . W W . . . . W",
            "                           ",
            "W . . g . . . . . r . y . W",
            "                           ",
            "W W y W W W g . W W W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game2 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . y . . . y . . W",
            "                           ",
            "W . . . r . r . . . r . . W",
            "                           ",
            "W W W W W . W . W . W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game3 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . g y . . W . y . . W",
            "                           ",
            "W W W . W W W r W W W . . W",
            "                           ",
            "W . . . . . . g . . . . . W",
            "                           ",
            "W W W . W W W r W W W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game4 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . r . . . . W",
            "                           ",
            "W . . . . . . . g . . . . W",
            "                           ",
            "W . . . . . . . W . . . . W",
            "                           ",
            "W . g . r . . . . . . . . W",
            "                           ",
            "W . g . r . . . . . . g . W",
            "                           ",
            "W W W . W . . . . . . W W W",
            "                           ",
            "W W W W W . W W W W W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game5 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W r g . . . g g . . . . . W",
            "                           ",
            "W W W . W W W W . W W . . W",
            "                           ",
            "W r g . . . . . . . . . . W",
            "                           ",
            "W W W W W . . W W . . . W W",
            "                           ",
            "W W W W W W . W W . . W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game6 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W W W W W W W . . . . . . W",
            "                           ",
            "W . . . . . . . g . . . . W",
            "                           ",
            "W . . . . . . . W W . . . W",
            "                           ",
            "W . r . . . y . . . . . . W",
            "                           ",
            "W . W . W W W . W . g . . W",
            "                           ",
            "W . . . . . . . . . W . y W",
            "                           ",
            "W . . . . . . . r . W W W W",
            "                           ",
            "W . . . W W W W W W W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game7 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . r . W",
            "                           ",
            "W . . . . . . . . . . W . W",
            "                           ",
            "W . . . . . g . . . g . . W",
            "                           ",
            "W . . . . . W . . r r . . W",
            "                           ",
            "W . . . . . . . . . W . . W",
            "                           ",
            "W . r . . g W . W . W . . W",
            "    -     -                ",
            "W . W . . W W . W . W . . W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game8 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W W W W . W . . W . W W W W",
            "                           ",
            "W W W . . g . . r . . W W W",
            "                           ",
            "W W . . . W . . W . . . W W",
            "          -     -          ",
            "W W . . . r . . g . . . W W",
            "                           ",
            "W W g . . . . . . . . r W W",
            "                           ",
            "W W W g . . . . . . r W W W",
            "                           ",
            "W W W W . . . . . . W W W W",
            "                           ",
            "W W W W W W W W W W W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game9 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . . . . . . . . . r g W",
            "                           ",
            "W . . . . W . . . . . W W W",
            "                           ",
            "W g . . . . . . . . b-b W W",
            "                           ",
            "W W . . . W . . W . W W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        private string[] game10 = new string[]
        {
            "W W W W W W W W W W W W W W",
            "                           ",
            "W . . . g r . . . . . . . W",
            "                           ",
            "W . . . b-b . b . . . . . W",
            "                           ",
            "W . . . . W . W . W W W W W",
            "                           ",
            "W . . . . . . . . . . . . W",
            "                           ",
            "W . . W . . W . . . . . . W",
            "                           ",
            "W . . . . . . . . W . . r W",
            "                        -  ",
            "W W . . . W . . . . . g W W",
            "                      -    ",
            "W . . . . . . . . . . W W W",
            "                           ",
            "W W W W W W W W W W W W W W"
        };

        public List<Game> LoadLevels()
        {
            List<Game> games = new List<Game>();
            int gameNumber = 0;

            foreach (string[] strings in GetLevelStrings())
                games.Add(CreateGameFromString(strings, gameNumber++));

            return games;
        }

        private List<string[]> GetLevelStrings()
        {
            List<string[]> levelStrings = new List<string[]>();

            levelStrings.Add(game0);
            levelStrings.Add(game1);
            levelStrings.Add(game2);
            levelStrings.Add(game3);
            levelStrings.Add(game4);
            levelStrings.Add(game5);
            levelStrings.Add(game6);
            levelStrings.Add(game7);
            levelStrings.Add(game8);
            levelStrings.Add(game9);
            levelStrings.Add(game10);

            return levelStrings;
        }

        public Game CreateGameFromString(string[] level)
        {
            return CreateGameFromString(level, 0);
        }

        public Game CreateGameFromString(string[] level, int gameNumber)
        {
            List<Cell[]> cells = new List<Cell[]>();
            Dictionary<Position, Position> links = new Dictionary<Position, Position>();

            for (int i = 0; i < level.Length; i++)
            {
                GetLineFromString(level[i], cells, links);

                if (++i < level.Length)
                    GetLinkFromString(level[i], links, (i - 1) / 2);
            }

            return new Game(cells.ToArray(), links, gameNumber);
        }

        private void GetLinkFromString(string lineString, Dictionary<Position, Position> links, int lineNumber)
        {
            char[] chars = lineString.ToCharArray();

            for (int i = 0; i < chars.Length; i+=2)
            {
                if (chars[i] != ' ')
                {
                    Position pos1 = new Position(i / 2, lineNumber);
                    Position pos2 = new Position(i / 2, lineNumber + 1);
                    links.Add(pos1, pos2);
                    links.Add(pos2, pos1);
                }
            }
        }

        private void GetLineFromString(string lineString, List<Cell[]> cells, Dictionary<Position, Position> links)
        {
            Cell[] line = new Cell[lineString.Length/2+1];
            char[] chars = lineString.ToLower().ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                line[i / 2] = CreateCellFromChar(chars[i]);

                if (++i < chars.Length && chars[i] != ' ')
                {
                    Position pos1 = new Position(i / 2, cells.Count);
                    Position pos2 = new Position(i / 2 + 1, cells.Count);
                    links.Add(pos1, pos2);
                    links.Add(pos2, pos1);
                }
            }
         
            cells.Add(line);
        }

        private Cell CreateCellFromChar(char c)
        {
            switch (c)
            {
                case 'w':
                    return new Cell(CellType.Wall, Color.None);
                case '.':
                    return new Cell(CellType.Empty, Color.None);
                case 'r':
                    return new Cell(CellType.Jelly, Color.Red);
                case 'g':
                    return new Cell(CellType.Jelly, Color.Green);
                case 'y':
                    return new Cell(CellType.Jelly, Color.Yellow);
                case 'b':
                    return new Cell(CellType.Jelly, Color.Black);
                default:
                    throw new Exception("Invalid " + c);
            }
        }
    }

}

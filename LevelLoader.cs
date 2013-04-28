using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class LevelLoader
    {
        private string[] levels = new string[]
        {
            // Empty
            @"WWWWWWWWWWWWWW;W------------W;W------------W;W------------W;W------------W;W------------W;W------------W;W------------W;WWWWWWWWWWWWWW;WWWWWWWWWWWWWW;",
            //Level 1
            @"WWWWWWWWWWWWWW;W------------W;W------------W;W------------W;W------------W;W-------r----W;W------WW----W;W--g-----r-y-W;WWyWWWg-WWWWWW;WWWWWWWWWWWWWW;",
            //Level 2
            @"WWWWWWWWWWWWWW;W------------W;W------------W;W------------W;W------------W;W------------W;W-----y---y--W;W---r-r---r--W;WWWWW-W-W-WWWW;WWWWWWWWWWWWWW;",
            //Level 3
            @"WWWWWWWWWWWWWW;W------------W;W------------W;W------------W;W------------W;W---gy--W-y--W;WWW-WWWrWWW--W;W------g-----W;WWW-WWWrWWWWWW;WWWWWWWWWWWWWW;",
            //Level 4
            @"WWWWWWWWWWWWWW;W------------W;W-------r----W;W-------g----W;W-------w----W;W-g-r--------W;W-g-r------g-W;WWW-W------WWW;WWWWW-WWWWWWWW;WWWWWWWWWWWWWW;",
            //Level 5
            @"WWWWWWWWWWWWWW;W------------W;W------------W;W------------W;Wrg---gg-----W;WWW-WWWW-WW--W;Wrg----------W;WWWWW--WW---WW;WWWWWW-WW--WWW;WWWWWWWWWWWWWW;"
        };

        public List<Game> LoadLevels()
        {
            List<Game> games = new List<Game>();

            foreach (string[] strings in GetLevelStrings())
                games.Add(CreateGameFromString(strings));

            return games;
        }

        private List<string[]> GetLevelStrings()
        {
            List<string[]> strings = new List<string[]>();

            foreach (string level in levels)
            {
                strings.Add(level.Split(new char[] { ';' }));
            }

            return strings;
        }

        public Game CreateGameFromString(string[] level)
        {
            List<Cell[]> cells = new List<Cell[]>();

            foreach (string line in level)
                cells.Add(CreateLineFromString(line));

            return new Game(cells.ToArray());
        }

        private Cell[] CreateLineFromString(string line)
        {
            Cell[] cells = new Cell[line.Length];
            char[] chars = line.ToLower().ToCharArray();

            for (int i = 0; i < chars.Length; i++)
                cells[i] = CreateCellFromChar(chars[i]);

            return cells;
        }

        private Cell CreateCellFromChar(char c)
        {
            switch (c)
            {
                case 'w':
                    return new Cell(CellType.Wall, Color.None);
                case '-':
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

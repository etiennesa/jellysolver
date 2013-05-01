using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class Game : IEquatable<Game>
    {
        private Cell[][] Cells;
        private Dictionary<Color, List<Jelly>> Jellys;
        public int Id;

        public Game(Cell[][] cells)
            :this(cells, 0)
        {
        }

        public Game(Cell[][] cells, int id)
        {
            Id = id;
            Cells = cells;
            GetJellysAndMerge();
        }

        private void GetJellysAndMerge()
        {
            Jellys = GetJellyCells();
            MergeJellys();
        }

        private Dictionary<Color, List<Jelly>> GetJellyCells()
        {
            Dictionary<Color, List<Jelly>> dico = new Dictionary<Color, List<Jelly>>();

            for (int j = 0; j < Cells.Length; j++)
            {
                for (int i = 0; i < Cells[j].Length; i++)
                {
                    Cell cell = Cells[j][i];
                    List<Jelly> jellys;

                    if (cell.Type == CellType.Jelly)
                    {
                        if (!dico.TryGetValue(cell.Color, out jellys))
                        {
                            jellys = new List<Jelly>();
                            dico[cell.Color] = jellys;
                        }

                        HashSet<Position> hashPos = new HashSet<Position>();
                        hashPos.Add(new Position(i, j));
                        jellys.Add(new Jelly(cell.Color, hashPos));
                    }
                }
            }

            return dico;
        }

        private void MergeJellys()
        {
            foreach (List<Jelly> jellys in Jellys.Values)
                MergeJellys(jellys);
        }

        private void MergeJellys(List<Jelly> jellys)
        {
            bool merged = true;

            while (merged)
            {
                merged = false;

                for (int i = 0; i < jellys.Count; i++)
                {
                    for (int j = i + 1; j < jellys.Count; j++)
                    {
                        if (jellys[i].IsNeighbour(jellys[j]))
                        {
                            jellys[i].Merge(jellys[j]);
                            jellys.RemoveAt(j);
                            merged = true;
                            break;
                        }
                    }
                    if (merged)
                        break;
                }
            }
        }

        public List<Game> GetNextMoves()
        {
            HashSet<Game> games = new HashSet<Game>();
            Game currentGame = null;

            foreach (List<Jelly> jellys in Jellys.Values)
                foreach (Jelly jelly in jellys)
                {
                    if ((currentGame = TryMoveRight(jelly)) != null && !games.Contains(currentGame))
                        games.Add(currentGame);
                    if ((currentGame = TryMoveLeft(jelly)) != null && !games.Contains(currentGame))
                        games.Add(currentGame);
                }

            return games.ToList<Game>();
        }

        private Game TryMoveRight(Jelly jelly)
        {
            return TryMove(jelly, Move.RightMove);
        }

        private Game TryMoveLeft(Jelly jelly)
        {
            return TryMove(jelly, Move.LeftMove);
        }

        private Game TryMove(Jelly jelly, Move moveDirection)
        {
            HashSet<Jelly> jellysToMove = new HashSet<Jelly>();

            if (CanMove(jelly, moveDirection, jellysToMove))
                return GetGameWithJellyMove(jellysToMove, moveDirection);

            return null;
        }

        private Game GetGameWithJellyMove(HashSet<Jelly> jellysToMove, Move moveDirection)
        {
            HashSet<Jelly> jellyClones = new HashSet<Jelly>();
            foreach (Jelly jelly in jellysToMove)
                jellyClones.Add(jelly.Clone());

            Game newGame = this.Clone();
            newGame.MoveJellys(jellyClones, moveDirection);
            return newGame;
        }

        private bool CanMove(Jelly jelly, Move moveDirection, HashSet<Jelly> neighbourJellys)
        {
            neighbourJellys.Add(jelly);
            Dictionary<Position, Cell> neighbourCells = GetNeighbourCells(jelly, moveDirection);

            foreach (KeyValuePair<Position, Cell> kvpCellPosition in neighbourCells)
            {
                switch (kvpCellPosition.Value.Type)
                {
                    case CellType.Wall:
                        return false;
                    case CellType.Jelly:
                        {
                            Jelly neighbourJelly = GetJellyFromPosition(kvpCellPosition.Key);
                            if (!neighbourJellys.Contains(neighbourJelly)
                                && !CanMove(neighbourJelly, moveDirection, neighbourJellys))
                                return false;
                        }
                        break;
                }
            }

            neighbourJellys.Add(jelly);

            return true;
        }

        private Dictionary<Position, Cell> GetNeighbourCells(Jelly jelly, Move moveDirection)
        {
            // Do a yield

            Dictionary<Position, Cell> neighbourCells = new Dictionary<Position, Cell>();

            foreach (Position pos in jelly.Positions)
            {
                Position newPos = pos.GetPosition(moveDirection);
             
                // Check position does not belong to the current Jelly
                if (!jelly.Positions.Contains(newPos))
                    neighbourCells.Add(newPos, GetCell(newPos));
            }

            return neighbourCells;
        }

        private Jelly GetJellyFromPosition(Position position)
        {
            foreach (List<Jelly> jellys in Jellys.Values)
                foreach (Jelly jelly in jellys)
                    if (jelly.Positions.Contains(position))
                        return jelly;

            throw new Exception("Cannot find Jelly at " + position);
        }

        private void MoveJellys(HashSet<Jelly> jellysToMove, Move moveDirection)
        {
            Dictionary<Jelly, Jelly> newOldJellys = new Dictionary<Jelly, Jelly>();

            // Set jellys positions to empty
            foreach (Jelly jelly in jellysToMove)
            {
                SetEmpty(jelly);
                Jelly oldJelly = jelly.Clone();
                jelly.Move(moveDirection);
                newOldJellys.Add(jelly, oldJelly);
            }

            // Set new jelly positions to jelly
            foreach (KeyValuePair<Jelly, Jelly> jellys in newOldJellys)
            {
                SetJelly(jellys.Key);
                UpdateJelly(jellys.Key, jellys.Value);
            }

            // Apply Gravity
            ApplyGravity();

            // Merge jellys
            GetJellysAndMerge();
        }

        private void UpdateJelly(Jelly jelly, Jelly oldJelly)
        {
            Jellys[jelly.Color].Remove(oldJelly);
            Jellys[jelly.Color].Add(jelly);
        }

        private void ApplyGravity()
        {
            bool movedOne = true;

            while (movedOne)
            {
                movedOne = false;

                foreach (List<Jelly> jellys in Jellys.Values)
                {
                    foreach (Jelly jelly in jellys)
                        if (TryMoveDown(jelly))
                        {
                            movedOne = true;
                            break;
                        }
                    if (movedOne)
                        break;
                }
            }
            
            MergeJellys();
        }

        private bool TryMoveDown(Jelly jelly)
        {
            HashSet<Jelly> jellysToMove = new HashSet<Jelly>();

            if (CanMove(jelly, Move.DownMove, jellysToMove))
            {
                MoveDown(jellysToMove);
                return true;
            }
            return false;
        }

        private void MoveDown(HashSet<Jelly> jellysToMove)
        {
            Dictionary<Jelly, Jelly> newOldJellys = new Dictionary<Jelly, Jelly>();

            // Set jellys positions to empty
            foreach (Jelly jelly in jellysToMove)
            {
                SetEmpty(jelly);
                Jelly oldJelly = jelly.Clone();
                jelly.Move(Move.DownMove);
                newOldJellys.Add(jelly, oldJelly);
            }

            // Set new jelly positions to jelly
            foreach (KeyValuePair<Jelly, Jelly> jellys in newOldJellys)
            {
                SetJelly(jellys.Key);
                UpdateJelly(jellys.Key, jellys.Value);
            }

            // Don't apply gravity, don't merge
        }

        private void SetEmpty(Jelly jelly)
        {
            SetCells(jelly, Color.None, CellType.Empty);
        }

        private void SetJelly(Jelly jelly)
        {
            SetCells(jelly, jelly.Color, CellType.Jelly);
        }

        private void SetCells(Jelly jelly, Color color, CellType type)
        {
            foreach (Position pos in jelly.Positions)
                Cells[pos.J][pos.I] = new Cell(type, color);
        }

        private Cell GetCell(Position pos)
        {
            return Cells[pos.J][pos.I];
        }

        private Game Clone()
        {
            Cell[][] cells = new Cell[Cells.Length][];

            for (int j = 0; j < Cells.Length; j++)
            {
                cells[j] = new Cell[Cells[j].Length];

                for (int i = 0; i < Cells[j].Length; i++)
                    cells[j][i] = Cells[j][i].Clone();
            }

            return new Game(cells);
        }

        public bool IsSolved()
        {
            // Check there is only one jelly of each color
            foreach (List<Jelly> jelly in Jellys.Values)
            {
                if (jelly.Count > 1)
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return (obj is Position && this.Equals((Position)obj));
        }

        public bool Equals(Game other)
        {
            if (Cells.Length != other.Cells.Length)
                return false;

            for (int j = 0; j < Cells.Length; j++)
            {
                if (Cells[j].Length != other.Cells[j].Length)
                    return false;

                for (int i = 0; i < Cells[j].Length; i++)
                    if (!Cells[j][i].Equals(other.Cells[j][i]))
                        return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            // too long...
            //return this.ToString().GetHashCode();

            int hash = 0;

            // combine the hash for the first cell of each jelly
            foreach (List<Jelly> jellys in Jellys.Values)
            {
                foreach (Jelly jelly in jellys)
                {
                    hash ^= jelly.GetHashCode();
                }
            }

            return hash;
        }

        public void Print()
        {
            GlobalConfig.Writer.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            StringBuilder bld = new StringBuilder();
            foreach (Cell[] line in Cells)
            {
                foreach (Cell cell in line)
                    bld.Append(cell.ToString());
                bld.AppendLine();
            }
            return bld.ToString();
        }
    }

    public class T_Game
    {
        public void Run()
        {
            GlobalConfig.Writer.Write("Testing T_Position...");

            LevelLoader loader = new LevelLoader();
            Game game1 = loader.CreateGameFromString(new string[]{"www","w-w","www"});
            Game game2 = loader.CreateGameFromString(new string[]{"www","wrw","www"});
            Game game3 = loader.CreateGameFromString(new string[] { "wwwww", "wr-rw", "wwwww" });
            Game game4 = loader.CreateGameFromString(new string[] { "wwwww", "w-rrw", "wwwww" });
            Game game5 = loader.CreateGameFromString(new string[] { "wwwww", "wrr-w", "wwwww" });
            Game game6 = loader.CreateGameFromString(new string[] { "wwwww", "wr--w", "ww--w", "w---w", "wwwww" });
            Game game7 = loader.CreateGameFromString(new string[] { "wwwww", "w---w", "ww--w", "w-r-w", "wwwww" });
            Game game8 = loader.CreateGameFromString(new string[] { "wwww", "wr-w", "wwrw", "wwww" });
            Game game9 = loader.CreateGameFromString(new string[] { "wwww", "w-rw", "wwrw", "wwww" });
            Game game10 = loader.CreateGameFromString(new string[] { "wwww", "w-rgw", "wwwww" });
            Game game11 = loader.CreateGameFromString(new string[] { "wwww", "wrg-w", "wwwww" });

            // bool IsSolved()
            Debug.Assert(game1.IsSolved());
            Debug.Assert(game2.IsSolved());
            Debug.Assert(!game3.IsSolved());

            // List<Game> GetNextMoves()
            List<Game> games = game1.GetNextMoves();
            Debug.Assert(games.Count.Equals(0));
            games = game2.GetNextMoves();
            Debug.Assert(games.Count.Equals(0));
            games = game3.GetNextMoves();
            Debug.Assert(games.Count.Equals(2));
            Debug.Assert(games.Contains(game4));
            Debug.Assert(games.Contains(game5));
            games = game6.GetNextMoves();
            Debug.Assert(games.Count.Equals(1));
            Debug.Assert(games.Contains(game7));
            games = game8.GetNextMoves();
            Debug.Assert(games.Count.Equals(1));
            Debug.Assert(games.Contains(game9));
            games = game10.GetNextMoves();
            Debug.Assert(games.Count.Equals(2));
            Debug.Assert(games.Contains(game11));

            // override bool Equals(object obj)

            // bool Equals(Game other)

            GlobalConfig.Writer.WriteLine("finished");
        }
    }
}

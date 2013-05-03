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
        private Dictionary<Position, Position> Links;

        private string _stringLevel;
        private int _hashCodeLevel;

        public Game(Cell[][] cells, Dictionary<Position, Position> links)
            :this(cells, links, 0)
        {
        }

        public Game(Cell[][] cells, Dictionary<Position, Position> links, int id)
        {
            Id = id;
            Cells = cells;
            Links = links;
            GetJellysAndMerge();
        }

        private void GetJellysAndMerge()
        {
            Jellys = GetJellyCells();
            MergeJellys();
        }

        private void ResetHashAndString()
        {
            _hashCodeLevel = 0;
            _stringLevel = null;
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
            foreach (KeyValuePair<Color, List<Jelly>> kvpJellys in Jellys)
            {
                if (kvpJellys.Key != Color.Black)
                    MergeJellys(kvpJellys.Value);
            }
            ResetHashAndString();
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
            return TryMove(jelly, Direction.RightMove);
        }

        private Game TryMoveLeft(Jelly jelly)
        {
            return TryMove(jelly, Direction.LeftMove);
        }

        private Game TryMove(Jelly jelly, Direction moveDirection)
        {
            HashSet<Jelly> jellysToMove = new HashSet<Jelly>();
            Dictionary<Position, Position> linksToMove = new Dictionary<Position, Position>();

            if (CanMove(jelly, moveDirection, jellysToMove, linksToMove))
                return GetGameWithJellyMove(jellysToMove, moveDirection, linksToMove);

            return null;
        }

        private Game GetGameWithJellyMove(
            HashSet<Jelly> jellysToMove, 
            Direction moveDirection, 
            Dictionary<Position, Position> linksToMove)
        {
            HashSet<Jelly> jellyClones = new HashSet<Jelly>();
            foreach (Jelly jelly in jellysToMove)
                jellyClones.Add(jelly.Clone());

            Dictionary<Position, Position> linksToMoveClone = new Dictionary<Position, Position>();
            foreach (KeyValuePair<Position, Position> kvp in linksToMove)
                linksToMoveClone.Add(kvp.Key.Clone(), kvp.Value.Clone());

            Game newGame = this.Clone();
            newGame.MoveJellys(jellyClones, moveDirection, linksToMoveClone);
            return newGame;
        }

        private bool CanMove(
            Jelly jelly, 
            Direction moveDirection, 
            HashSet<Jelly> markedJellys, 
            Dictionary<Position, Position> linksToMove)
        {
            markedJellys.Add(jelly);

            // Check for jelly links
            foreach (Position pos in jelly.Positions)
            {
                Position linkedPosition;
                if (Links.TryGetValue(pos, out linkedPosition))
                {
                    Cell linkedCell = GetCell(Links[pos]);
                    switch (linkedCell.Type)
                    {
                        case CellType.Wall:
                            return false;
                        case CellType.Jelly:
                            {
                                Jelly neighbourJelly = GetJellyFromPosition(Links[pos]);
                                if (!markedJellys.Contains(neighbourJelly)
                                    && !CanMove(neighbourJelly, moveDirection, markedJellys, linksToMove))
                                    return false;
                            }
                            break;
                        default:
                            throw new Exception("Cannot link between empty cell and a jelly");
                    }
                    if (!linksToMove.ContainsKey(pos))
                        linksToMove.Add(pos, linkedPosition);
                    if (!linksToMove.ContainsKey(linkedPosition))
                        linksToMove.Add(linkedPosition, pos);
                }
            }

            // Check that neighbour cells of the jelly allow the move :
            // either its empty or its a jelly that can move
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
                            if (!markedJellys.Contains(neighbourJelly)
                                && !CanMove(neighbourJelly, moveDirection, markedJellys, linksToMove))
                                return false;
                        }
                        break;
                }
            }

            return true;
        }

        private Dictionary<Position, Cell> GetNeighbourCells(Jelly jelly, Direction moveDirection)
        {
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

        private void MoveJellys(
            HashSet<Jelly> jellysToMove, 
            Direction moveDirection, 
            Dictionary<Position, Position> linksToMove)
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

            // Move links
            foreach (KeyValuePair<Position, Position> kvp in linksToMove)
            {
                Links.Remove(kvp.Key);
            }
            foreach (KeyValuePair<Position, Position> kvp in linksToMove)
            {
                Links.Add(kvp.Key.GetPosition(moveDirection), kvp.Value.GetPosition(moveDirection));
            }

            // Apply Gravity
            ApplyGravity();

            // Merge jellys
            GetJellysAndMerge();

            ResetHashAndString();
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

            ResetHashAndString();
        }

        private bool TryMoveDown(Jelly jelly)
        {
            HashSet<Jelly> jellysToMove = new HashSet<Jelly>();
            Dictionary<Position, Position> linksToMove = new Dictionary<Position, Position>();

            if (CanMove(jelly, Direction.DownMove, jellysToMove, linksToMove))
            {
                MoveDown(jellysToMove, linksToMove);
                return true;
            }
            return false;
        }

        private void MoveDown(HashSet<Jelly> jellysToMove, Dictionary<Position, Position> linksToMove)
        {
            Dictionary<Jelly, Jelly> newOldJellys = new Dictionary<Jelly, Jelly>();

            // Set jellys positions to empty
            foreach (Jelly jelly in jellysToMove)
            {
                SetEmpty(jelly);
                Jelly oldJelly = jelly.Clone();
                jelly.Move(Direction.DownMove);
                newOldJellys.Add(jelly, oldJelly);
            }

            // Set new jelly positions to jelly
            foreach (KeyValuePair<Jelly, Jelly> jellys in newOldJellys)
            {
                SetJelly(jellys.Key);
                UpdateJelly(jellys.Key, jellys.Value);
            }

            // Move links
            foreach (KeyValuePair<Position, Position> kvp in linksToMove)
            {
                Links.Remove(kvp.Key);
                Links.Add(kvp.Key.GetPosition(Direction.DownMove), kvp.Value.GetPosition(Direction.DownMove));
            }

            // Don't apply gravity, don't merge

            ResetHashAndString();
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

        public Game Clone()
        {
            Cell[][] cells = new Cell[Cells.Length][];

            for (int j = 0; j < Cells.Length; j++)
            {
                cells[j] = new Cell[Cells[j].Length];

                for (int i = 0; i < Cells[j].Length; i++)
                    cells[j][i] = Cells[j][i].Clone();
            }

            Dictionary<Position, Position> links = new Dictionary<Position, Position>(Links.Count);

            foreach (KeyValuePair<Position, Position> kvp in Links)
                links.Add(kvp.Key.Clone(), kvp.Value.Clone());

            return new Game(cells, links, Id);
        }

        public bool IsSolved()
        {
            // Check there is only one jelly of each color except black
            foreach (KeyValuePair<Color, List<Jelly>> kvpJelly in Jellys)
            {
                if ((kvpJelly.Key != Color.Black) && (kvpJelly.Value.Count > 1))
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
            return (ToString() == other.ToString());
        }

        public override int GetHashCode()
        {
            if (_hashCodeLevel == 0)
            {
                _hashCodeLevel = SetHashCode();
            }
            return _hashCodeLevel;
        }

        private int SetHashCode()
        {
            int hash = 0;

            // combine the hash for the first cell of each jelly
            foreach (List<Jelly> jellys in Jellys.Values)
            {
                foreach (Jelly jelly in jellys)
                {
                    hash ^= jelly.GetHashCode();
                }
            }

            // Combine the has for links
            foreach (KeyValuePair<Position, Position> kvp in Links)
                hash ^= kvp.Key.GetHashCode() * 27 + kvp.Value.GetHashCode();

            return hash;
        }

        public void Print()
        {
            GlobalConfig.Writer.WriteLine(this.ToString());
        }

        public override string ToString()
        {
            if (_stringLevel == null)
            {
                _stringLevel = SetString();
            }
            return _stringLevel;
        }

        private string SetString()
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
}

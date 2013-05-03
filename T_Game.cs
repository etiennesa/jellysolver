using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JellySolver
{

    public class T_Game
    {
        public void Run()
        {
            GlobalConfig.Writer.Write("Testing T_Game...");

            LevelLoader loader = new LevelLoader();
            Game game1 = loader.CreateGameFromString(new string[] 
            { 
                "w w w", 
                "     ", 
                "w . w", 
                "     ", 
                "w w w" 
            });
            Game game2 = loader.CreateGameFromString(new string[]
            {
                "w w w",
                "     ", 
                "w r w",
                "     ", 
                "w w w"
            });
            Game game3 = loader.CreateGameFromString(new string[] { 
                "w w w w w", 
                "         ", 
                "w r . r w", 
                "         ", 
                "w w w w w" 
            });
            Game game4 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w w", 
                "         ", 
                "w . r r w", 
                "         ", 
                "w w w w w" 
            });
            Game game5 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w w", 
                "         ", 
                "w r r . w", 
                "         ", 
                "w w w w w" 
            });
            Game game6 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w w", 
                "         ", 
                "w r . . w", 
                "         ", 
                "w w . . w", 
                "         ", 
                "w . . . w", 
                "         ", 
                "w w w w w" 
            });
            Game game7 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w w" ,
                "         ", 
                "w . . . w", 
                "         ", 
                "w w . . w", 
                "         ", 
                "w . r . w", 
                "         ", 
                "w w w w w" 
            });
            Game game8 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w", 
                "       ", 
                "w r . w", 
                "       ", 
                "w w r w", 
                "       ", 
                "w w w w" 
            });
            Game game9 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w", 
                "       ", 
                "w . r w", 
                "       ", 
                "w w r w", 
                "       ", 
                "w w w w" 
            });
            Game game10 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w w", 
                "         ", 
                "w . r g w", 
                "         ", 
                "w w w w w" 
            });
            Game game11 = loader.CreateGameFromString(new string[] 
            { 
                "w w w w w", 
                "         ", 
                "w r g . w", 
                "         ", 
                "w w w w w" 
            });
            Game game12 = loader.CreateGameFromString(new string[]
            {
                "W W W W W",
                "         ",
                "W-r . g W",
                "      -  ",
                "W W W W W"
            });
            Game game13 = loader.CreateGameFromString(new string[]
            {
                "W W W W W",
                "         ",
                "W b-b . W",
                "         ",
                "W W W W W"
            });
            Game game14 = loader.CreateGameFromString(new string[]
            {
                "W W W W W",
                "         ",
                "W . b-b W",
                "         ",
                "W W W W W"
            });

            // Clone
            Debug.Assert(game1.Clone().Equals(game1));
            Debug.Assert(game12.Clone().Equals(game12));

            // bool IsSolved()
            Debug.Assert(game1.IsSolved());
            Debug.Assert(game2.IsSolved());
            Debug.Assert(!game3.IsSolved());
            Debug.Assert(game13.IsSolved());

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
            games = game12.GetNextMoves();
            Debug.Assert(games.Count.Equals(0));
            games = game13.GetNextMoves();
            Debug.Assert(games.Count.Equals(1));
            Debug.Assert(games.Contains(game14));

            // override bool Equals(object obj)

            // bool Equals(Game other)

            GlobalConfig.Writer.WriteLine("finished");
        }
    }
}

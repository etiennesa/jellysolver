using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class Solver
    {
        public void Run()
        {
            foreach (Game game in new LevelLoader().LoadLevels())
            {
                Solve(game);
            }
        }

        public bool Solve(Game game)
        {
            Game currentGame = null;
            Game previousGame = null;
            Queue<Game> gameToExamine = new Queue<Game>();
            gameToExamine.Enqueue(game);
            Dictionary<Game, Game> gamesWithFather = new Dictionary<Game, Game>();
            gamesWithFather.Add(game, null);
            int countToPrint = 1;

            while (gameToExamine.Count!= 0)
            {
                previousGame = currentGame;
                currentGame = gameToExamine.Dequeue();

                if (GlobalConfig.DisplaySearch)
                    currentGame.Print();

                if (currentGame.IsSolved())
                    break;

                List<Game> nextGames = currentGame.GetNextMoves();

                foreach (Game move in nextGames)
                {
                    if (!gamesWithFather.ContainsKey(move))
                    {
                        gamesWithFather.Add(move, currentGame);
                        gameToExamine.Enqueue(move);
                        //if (gamesWithFather.Count >= countToPrint)
                        //{
                        //    GlobalConfig.Writer.WriteLine(gamesWithFather.Count + " games tried");
                        //    countToPrint *= 10;
                        //}
                    }
                }
            }

            if (currentGame.IsSolved())
            {
                PrintSolution(currentGame, gamesWithFather);
                return true;
            }

            GlobalConfig.Writer.WriteLine("Couldn't solve this ");
            GlobalConfig.Writer.WriteLine("Positions tried : " + gamesWithFather.Count);

            return false;
        }



        private void PrintSolution(Game gameSolved, Dictionary<Game, Game> gamesWithFather)
        {
            Stack<Game> orderedGames = new Stack<Game>();
            Game currentGame = gameSolved;
            int count = 1;
            GlobalConfig.Writer.WriteLine(gamesWithFather.Count + " total games tried");

            do
            {
                orderedGames.Push(currentGame);
            }
            while (gamesWithFather.TryGetValue(currentGame, out currentGame) && currentGame != null);

            GlobalConfig.Writer.WriteLine(orderedGames.Count + " moves for solution");

            foreach (Game game in orderedGames)
            {
                GlobalConfig.Writer.WriteLine("move n°" + count++);
                game.Print();
            }
        }
    }

    class T_Solver
    {
        public void Run()
        {
            GlobalConfig.Writer.Write("Testing T_Position...");

            // Solve
            LevelLoader loader = new LevelLoader();
            
            // Empty
            Game game1 = loader.CreateGameFromString(new string[] { "www", "w-w", "www" });
            Debug.Assert(new Solver().Solve(game1));

            // one jelly
            Game game2 = loader.CreateGameFromString(new string[] { "www", "wrw", "www" });
            Debug.Assert(new Solver().Solve(game2));

            // 2 jellys simple
            Game game3 = loader.CreateGameFromString(new string[] { "wwwww", "wr-rw", "wwwww" });
            Debug.Assert(new Solver().Solve(game3));
            
            // gravity
            Game game4 = loader.CreateGameFromString(new string[] { 
                "wwwww", 
                "wr--w", 
                "ww--w", 
                "w--rw", 
                "wwwww" 
            });
            Debug.Assert(new Solver().Solve(game4));

            // push group of jellys
            Game game5 = loader.CreateGameFromString(new string[] { 
                "wwwww", 
                "w-yww", 
                "w-rgw", 
                "wwgww",
                "wwwww" 
            });
            Debug.Assert(new Solver().Solve(game5));


            GlobalConfig.Writer.WriteLine("finished");
        }
    }
}

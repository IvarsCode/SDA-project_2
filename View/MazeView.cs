

using Model;

namespace View
{
    public class MazeView
    {
        //View

        public void DisplayMaze(Maze maze)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            //Console.Clear();
            var array = maze.MazeArray;

            Console.WriteLine("\n");

            // Loop over the elements of the maze array
            // and display as characters.
            for (int rowIdx = 0; rowIdx < array.Length; rowIdx++)
            {
                var row = array[rowIdx];
                for (int colIdx = 0; colIdx < row.Length; colIdx++)
                {
                    switch (row[colIdx])
                    {
                        case -1:
                            Console.Write("🟦");   //walls
                            break;
                        case 1:
                            Console.Write("🏠");   //begin 
                            break;
                        case 2:
                            Console.Write("🍦");   //end
                            break;
                        case 0:
                            Console.Write("  ");   //not visited
                            break;
                        //Marking strategy
                        case 10:
                            Console.Write("🏅");   //completed
                            break;
                        case 4:
                            Console.Write("⚽️");   //visited
                            break;
                        case 5:
                            Console.Write("🔴");   //explored dead-end (not on shortest path)
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("🟦");
            }

            for (int colIdx = 0; colIdx <= array[0].Length; colIdx++)
                Console.Write("🟦");
            Console.WriteLine("\n");
        }

        public void DisplayMaze(Maze maze, int[] currPos)
        {
            var array = maze.MazeArray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            var rand = new Random();
            // Loop over the elements of the maze array
            // and display as characters.
            for (int rowIdx = 0; rowIdx < array.Length; rowIdx++)
            {
                var row = array[rowIdx];
                for (int colIdx = 0; colIdx < row.Length; colIdx++)
                {
                    switch (row[colIdx])
                    {
                        case -1:
                            Console.Write("🟦");   //walls
                            break;
                        case 1:                    //begin 
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("⚽️");
                            else
                                Console.Write("🏠");
                            break;
                        case 2:
                            Console.Write("🍦");    //end
                            break;
                        case 0:                     //not visited
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("⚽️");
                            else
                                Console.Write("  ");
                            break;
                        //Marking strategy
                        case 10:
                            Console.Write("🏅");   //completed
                            break;
                        case 4:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("⚽️");
                            else
                            {
                                if (rand.NextDouble() < 0.3)
                                    Console.Write("🦖");
                                else if (rand.NextDouble() >= 0.3 && rand.NextDouble() < 0.6)
                                    Console.Write("🦕");
                                else
                                    Console.Write("🐈");
                            }
                            //Console.Write("⚽️");   //visited
                            break;
                        case 5:
                            Console.Write("🔴");   //explored dead-end (not on shortest path)
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("🟦");
            }

            for (int colIdx = 0; colIdx <= array[0].Length; colIdx++)
                Console.Write("🟦");
            Console.WriteLine();
        }

        public void DisplayMaze(Maze maze, int[] currPos, string[] symbolsArr, Queue<int[]> visitedPositions, PathFinderType algType = PathFinderType.Manual)
        {
            var array = maze.MazeArray;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"\n\n{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}{"  " + algType + "  "}{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;

            System.Console.WriteLine();

            // Loop over the elements of the maze array
            // and display as characters.
            for (int rowIdx = 0; rowIdx < array.Length; rowIdx++)
            {
                var row = array[rowIdx];
                for (int colIdx = 0; colIdx < row.Length; colIdx++)
                {
                    switch (row[colIdx])
                    {
                        case -1:
                            Console.Write("🟦");   //walls
                            break;
                        case 1:                    //begin 
                                                   //if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                                   //    Console.Write("⚽️");
                                                   //else
                            Console.Write("🏠");
                            break;
                        case 2:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("🏅");    //completed
                            else
                                Console.Write("🍦");    //end     
                            break;
                        case 0:                     //not visited
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                Console.Write("⚽️");
                            else if (visitedPositions.Any(_ => _[0] == rowIdx && _[1] == colIdx))
                                Console.Write("🏃");
                            else
                                Console.Write("  ");
                            break;
                        //Marking strategy:
                        case 10:
                            Console.Write("🏅");    //completed
                            break;
                        case 4:
                            if (currPos[0] == rowIdx && currPos[1] == colIdx)
                            {
                                Console.Write("⚽️");
                            }
                            else
                            {
                                Console.Write("🏃");
                            }
                            break;
                        case 5:
                            Console.Write("🔴");    //explored dead-end (not on shortest path)
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("🟦");
            }

            for (int colIdx = 0; colIdx <= array[0].Length; colIdx++)
                Console.Write("🟦");

            if (algType == PathFinderType.Manual)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("\n\n👉 👉 👉        Press S to go start again.    👈 👈 👈");
                Console.WriteLine("👉 👉 👉 Press M or ⬅️  to go back to the Menu. 👈 👈 👈\n");
            }

            if (!maze.IsValidMove(currPos[0], currPos[1], true))
            {
                PrintWrongMove(currPos);
            }

            if (currPos[0] == maze.End[0] && currPos[1] == maze.End[1]) //completed
            {
                //Reset Maze
                visitedPositions = new Queue<int[]>();
                Console.WriteLine("\n");
                Console.WriteLine("👍 DONE!!! AMAZING!!! 👍");
                Thread.Sleep(300);
                return;

            }

        }

        public void DisplayMaze(Maze maze, string[] symbolsArr, int timeInterval, Queue<int[]> visitedPositions)
        {

            var array = maze.MazeMDArray;

            var toBeShownPositions = new Queue<int[]>(visitedPositions);
            var shownPositions = new Queue<int[]>();

            while (toBeShownPositions.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear();

                var currPos = toBeShownPositions.Dequeue();

                // Sentinel: transition from exploration phase to shortest-path replay
                if (currPos[0] == -1 && currPos[1] == -1)
                {
                    var pathSet = new HashSet<(int, int)>(toBeShownPositions.Select(p => (p[0], p[1])));
                    foreach (var ep in shownPositions)
                    {
                        if (!pathSet.Contains((ep[0], ep[1])) && array[ep[0], ep[1]] != 1 && array[ep[0], ep[1]] != 2)
                        {
                            array[ep[0], ep[1]] = 5;               // 🔴 dead-end in MazeMDArray
                            maze.MazeArray[ep[0]][ep[1]] = 5;      // 🔴 dead-end in MazeArray
                        }
                    }
                    shownPositions = new Queue<int[]>();
                    continue;
                }

                shownPositions.Enqueue(currPos);

                // Loop over the elements of the maze array
                // and display as characters.
                for (int rowIdx = 0; rowIdx < array.GetLength(0); rowIdx++)
                {
                    for (int colIdx = 0; colIdx < array.GetLength(1); colIdx++)
                    {
                        switch (array[rowIdx, colIdx])
                        {
                            case -1:
                                Console.Write("🟦");   //walls
                                break;
                            case 1:                    //begin
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("⚽️");
                                else
                                    Console.Write("🏠");
                                break;
                            case 2:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("🏅");    //completed
                                else
                                    Console.Write("🍦");    //end
                                break;
                            case 0:                     //not visited or visited in a not marked array
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write("⚽️");
                                }
                                else if (shownPositions.Any(_ => _[0] == rowIdx && _[1] == colIdx))
                                {
                                    Console.Write("🏃");
                                }
                                else
                                    Console.Write("  ");
                                break;
                            //Marking strategy
                            case 10:
                                Console.Write("🏅");    //completed
                                break;
                            case 4:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write("⚽️");
                                }
                                else
                                {
                                    Console.Write("🏃");
                                }
                                break;
                            case 5:
                                Console.Write("🔴");    //explored dead-end (not on shortest path)
                                break;
                            default:
                                break;
                        }
                    }
                    Console.WriteLine("🟦");
                }

                for (int colIdx = 0; colIdx <= array.GetLength(1); colIdx++)
                    Console.Write("🟦");
                Console.WriteLine();

                Thread.Sleep(timeInterval);
                //Console.Clear();
            }

            // Reset dead-end markers so the maze is clean for the next run
            for (int r = 0; r < array.GetLength(0); r++)
                for (int c = 0; c < array.GetLength(1); c++)
                    if (array[r, c] == 5) { array[r, c] = 0; maze.MazeArray[r][c] = 0; }
        }

        public void DisplayMaze(Maze maze, string[] symbolsArr, int timeInterval, Queue<int[]> visitedPositions, PathFinderType algType = PathFinderType.Manual)
        {

            var array = maze.MazeMDArray;

            var toBeShownPositions = new Queue<int[]>(visitedPositions);
            var shownPositions = new Queue<int[]>();

            while (toBeShownPositions.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear();

                var currPos = toBeShownPositions.Dequeue();

                // Sentinel: transition from exploration phase to shortest-path replay
                if (currPos[0] == -1 && currPos[1] == -1)
                {
                    var pathSet = new HashSet<(int, int)>(toBeShownPositions.Select(p => (p[0], p[1])));
                    foreach (var ep in shownPositions)
                    {
                        if (!pathSet.Contains((ep[0], ep[1])) && array[ep[0], ep[1]] != 1 && array[ep[0], ep[1]] != 2)
                        {
                            array[ep[0], ep[1]] = 5;               // 🔴 dead-end in MazeMDArray
                            maze.MazeArray[ep[0]][ep[1]] = 5;      // 🔴 dead-end in MazeArray
                        }
                    }
                    shownPositions = new Queue<int[]>();
                    continue;
                }

                shownPositions.Enqueue(currPos);

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n\n{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}{"  " + algType + "  "}{String.Concat(Enumerable.Repeat("🟨", maze.MazeMDArray.GetLength(1) / 2 - algType.ToString().Length / 3))}");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine();

                // Loop over the elements of the maze array
                // and display as characters.
                for (int rowIdx = 0; rowIdx < array.GetLength(0); rowIdx++)
                {
                    for (int colIdx = 0; colIdx < array.GetLength(1); colIdx++)
                    {
                        switch (array[rowIdx, colIdx])
                        {
                            case -1:
                                Console.Write("🟦");   //walls
                                break;
                            case 1:                    //begin
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("⚽️");
                                else
                                    Console.Write("🏠");
                                break;
                            case 2:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                    Console.Write("🏅");    //completed
                                else
                                    Console.Write("🍦");    //end
                                break;
                            case 0:                     //not visited or visited in a not marked array
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write("⚽️");
                                }
                                else if (shownPositions.Any(_ => _[0] == rowIdx && _[1] == colIdx))
                                {
                                    Console.Write("🏃");
                                }
                                else
                                    Console.Write("  ");
                                break;
                            //Marking strategy
                            case 10:
                                Console.Write("🏅");    //completed
                                break;
                            case 4:
                                if (currPos[0] == rowIdx && currPos[1] == colIdx)
                                {
                                    Console.Write("⚽️");
                                }
                                else
                                {
                                    Console.Write("🏃");
                                }
                                break;
                            case 5:
                                Console.Write("🔴");    //explored dead-end (not on shortest path)
                                break;
                            default:
                                break;
                        }
                    }
                    Console.WriteLine("🟦");
                }

                for (int colIdx = 0; colIdx <= array.GetLength(1); colIdx++)
                    Console.Write("🟦");
                Console.WriteLine();

                Thread.Sleep(timeInterval);
                //Console.Clear();
            }

            // Reset dead-end markers so the maze is clean for the next run
            for (int r = 0; r < array.GetLength(0); r++)
                for (int c = 0; c < array.GetLength(1); c++)
                    if (array[r, c] == 5) { array[r, c] = 0; maze.MazeArray[r][c] = 0; }
        }

        public string[] generateSymbols(int spaces)
        {
            var rnd = new Random();
            var symbols = new string[2 * spaces];
            for (int i = 0; i < 2 * spaces; i++)
            {
                /*
                    if(i < 10)
                        symbols[i] = ":" + i;
                    else
                        symbols[i] = (i % 100) < 10 ? ":" + (i % 100) : (i % 100) + "";

                */
                if (rnd.NextDouble() < 0.3)
                {
                    symbols[i] = "🦖";
                }

                else if (rnd.NextDouble() < 0.7)
                {
                    symbols[i] = "🐈";
                }

                else
                {
                    symbols[i] = "🦕";
                }
            }

            return symbols;
        }

        public void DisplaySuccess(bool success, string msg, int timeInterval)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(success ? msg + "🎉 Path found! 🎊" : msg + "🔎  No path found. 🔎 ");
            if (!success)
                Thread.Sleep(timeInterval);
        }
        private void PrintWrongMove(int[] tmppos)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Wrong Direction -> {tmppos[0]}, {tmppos[1]}");

            Thread.Sleep(100);
            Console.BackgroundColor = ConsoleColor.White;
        }
    }
}



namespace View
{
    public class MenuView
    {
        public static void DisplayMenu(ConsoleKey key = ConsoleKey.None)
        {
            //Menu elements:
            string start = "\n➡️    Welcome to the amazing Maze  ⬅️   \n" +
                               "  🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦    ";
            string choice = "🟦Choose pathfinder:              ✈️   ";
            string recursive = "🟦Press 1 Recursive 🔃            🟦  ";
            string stack = "🟦Press 2 Stack 🥞                🟦  ";
            string Astar = "🟦Press 3 A⭐️                     🟦  ";
            string dijkstra = "🟦Press 4 Dijkstra 🔎             🟦  ";
            string manual = "🟦Press P to Play manually        🟦  ";
            string quit = "🟦Press Q to Quit                 🟦  ";
            string end = "  🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦🟦    ";

            string[] lines = { start + "\n" + choice, recursive, stack, Astar, dijkstra, manual, quit, end };
            int numBlocks = end.Length / 2;

            switch (key)
            {
                case ConsoleKey.D1:
                    PrintMenu(lines, recursive, numBlocks);
                    break;
                case ConsoleKey.D2:
                    PrintMenu(lines, stack, numBlocks);
                    break;
                case ConsoleKey.D3:
                    PrintMenu(lines, Astar, numBlocks);
                    break;
                case ConsoleKey.D4:
                    PrintMenu(lines, dijkstra, numBlocks);
                    break;
                case ConsoleKey.P:
                    PrintMenu(lines, manual, numBlocks);
                    break;
                default:
                    PrintMenu(lines, "", numBlocks);
                    return;
            }
        }

        static void PrintMenu(string[] lines, string option, int numBlocks)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            foreach (var line in lines)
            {
                if (line == option)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(line);
                Console.BackgroundColor = ConsoleColor.White;
            }
            if (option != "")
                ProgressBar(numBlocks, 300);
        }

        static void ProgressBar(int numBlocks, int timeInterval)
        {
            Console.WriteLine($"\n\n{String.Join("", Enumerable.Repeat(" ", numBlocks / 6))}✈️  Starting in a while...⏱️ \n");
            for (int i = 1; i <= numBlocks; i++)
            {
                Console.Write("🟩");
                Thread.Sleep(timeInterval);
            }
            Console.Write($"\n\n{String.Join("", Enumerable.Repeat(" ", numBlocks / 6))}  🏁 START! 🏁");
            Thread.Sleep(2 * timeInterval);
        }

        public static void PrintEndMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n\n       🙏 Bye Bye 🙏           \n\n       😀 See you next time 🖐️  \n");
            Console.ResetColor();
        }
    }
}
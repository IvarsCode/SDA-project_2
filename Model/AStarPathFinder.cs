
namespace Model
{
    // Implementatie van A* - sneller dan Dijkstra door een heuristiek te gebruiken die richting het doel stuurt
    public class AStarPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Astar;
        public PathFinderType algType { get => _algType; set { } }

        // Schat de resterende afstand tot het einde via Manhattan-afstand (geen diagonale beweging mogelijk in deze maze)
        private int Heuristic(int row, int col, int[] end)
        {
            return Math.Abs(row - end[0]) + Math.Abs(col - end[1]);
        }

        // Zoekt het kortste pad met A*: combineert werkelijke afstand (g) en heuristiek (h) zodat het zoekgebied kleiner blijft dan bij Dijkstra
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            // g-kosten (werkelijke afstand vanaf start) op oneindig zetten, parent bijhouden voor padreconstructie
            int[][] gCost = new int[rows][];
            int[][][] parent = new int[rows][][];

            for (int r = 0; r < rows; r++)
            {
                gCost[r] = new int[cols];
                parent[r] = new int[cols][];
                for (int c = 0; c < cols; c++)
                    gCost[r][c] = int.MaxValue;
            }

            bool[][] visited = new bool[rows][];
            for (int r = 0; r < rows; r++)
                visited[r] = new bool[cols];

            // Priority queue sorteert op f = g + h, zodat de meest belovende cel altijd als eerste verwerkt wordt
            PriorityQueue<int[], int> pq = new PriorityQueue<int[], int>();

            // Startpunt heeft g=0, f = alleen de heuristiek
            gCost[pos[0]][pos[1]] = 0;
            pq.Enqueue(pos, Heuristic(pos[0], pos[1], maze.End));

            bool found = false;

            // Verwerk steeds de cel met de laagste f-waarde totdat het einde bereikt is
            while (pq.Count > 0)
            {
                pq.TryDequeue(out int[] current, out int _);

                int row = current[0];
                int col = current[1];

                if (visited[row][col]) continue;
                visited[row][col] = true;

                visitedPositions.Enqueue(current); // voor de visualisatie van de zoekgolf

                if (row == maze.End[0] && col == maze.End[1])
                {
                    found = true;
                    break;
                }

                // Bekijk alle buren en voeg ze toe als we een kortere route gevonden hebben
                foreach (int[] move in maze.moves)
                {
                    int newRow = row + move[0];
                    int newCol = col + move[1];

                    if (!maze.IsValidMove(newRow, newCol)) continue;
                    if (visited[newRow][newCol]) continue;

                    int newG = gCost[row][col] + 1;
                    if (newG < gCost[newRow][newCol])
                    {
                        gCost[newRow][newCol] = newG;
                        parent[newRow][newCol] = current;
                        int f = newG + Heuristic(newRow, newCol, maze.End);
                        pq.Enqueue(new int[] { newRow, newCol }, f);
                    }
                }
            }

            visitedPositions.Enqueue(new int[] { -1, -1 }); // scheiding tussen zoekfase en padweergave

            // Reconstrueer het kortste pad door terug te lopen via de parent-array
            if (found)
            {
                var path = new Stack<int[]>();
                int[] step = maze.End;
                while (step != null)
                {
                    path.Push(step);
                    step = parent[step[0]][step[1]];
                }
                while (path.Count > 0)
                    visitedPositions.Enqueue(path.Pop());
            }
        }
    }
}

namespace Model
{
    // Implementatie van Dijkstra's algoritme om het kortste pad door een maze te vinden
    public class DijkstraPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Dijkstra;
        public PathFinderType algType { get => _algType; set { } }

        // Zoekt het kortste pad van startpositie naar het einde van de maze en slaat de bezochte cellen op in visitedPositions
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            // Alle afstanden op oneindig zetten, parent bijhouden om het pad later te reconstrueren
            int[][] distances = new int[rows][];
            int[][][] parent = new int[rows][][];

            for (int r = 0; r < rows; r++)
            {
                distances[r] = new int[cols];
                parent[r] = new int[cols][];
                for (int c = 0; c < cols; c++)
                    distances[r][c] = int.MaxValue;
            }

            bool[][] visited = new bool[rows][];
            for (int r = 0; r < rows; r++)
                visited[r] = new bool[cols];

            PriorityQueue<int[], int> pq = new PriorityQueue<int[], int>();

            // Startpunt heeft afstand 0
            distances[pos[0]][pos[1]] = 0;
            pq.Enqueue(pos, 0);

            bool found = false;

            // Verwerk steeds de goedkoopste cel uit de priority queue totdat het einde bereikt is
            while (pq.Count > 0)
            {
                pq.TryDequeue(out int[] current, out int currentCost);

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

                // Bekijk alle buren en update hun afstand als we een kortere route gevonden hebben
                foreach (int[] move in maze.moves)
                {
                    int newRow = row + move[0];
                    int newCol = col + move[1];

                    if (!maze.IsValidMove(newRow, newCol)) continue;
                    if (visited[newRow][newCol]) continue;

                    int newCost = currentCost + 1;
                    if (newCost < distances[newRow][newCol])
                    {
                        distances[newRow][newCol] = newCost;
                        parent[newRow][newCol] = current;
                        pq.Enqueue(new int[] { newRow, newCol }, newCost);
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



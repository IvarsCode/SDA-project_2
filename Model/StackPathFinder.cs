
namespace Model
{
    // Implementatie van iteratieve DFS - zoekt via een expliciete stack diepte-eerst door de maze
    public class StackPathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Stack;
        public PathFinderType algType { get => _algType; set { } }

        // Zoekt een pad met DFS: cellen worden LIFO verwerkt, waardoor de zoekgolf diepte-eerst gaat
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            // visited voorkomt dat cellen meerdere keren verwerkt worden (stack kan duplicaten bevatten)
            bool[][] visited = new bool[rows][];
            int[][][] parent = new int[rows][][];

            for (int r = 0; r < rows; r++)
            {
                visited[r] = new bool[cols];
                parent[r] = new int[cols][];
            }

            // Stack zorgt voor LIFO-volgorde: de meest recent gevonden buur wordt als eerste verder onderzocht
            var stack = new Stack<int[]>();
            stack.Push(pos);

            bool found = false;

            // Verwerk steeds de bovenste cel van de stack totdat het einde bereikt is of de stack leeg is
            while (stack.Count > 0)
            {
                int[] current = stack.Pop();
                int row = current[0];
                int col = current[1];

                if (visited[row][col]) continue;
                visited[row][col] = true;

                visitedPositions.Enqueue(current); // voor visualisatie van de zoekgolf

                if (row == maze.End[0] && col == maze.End[1])
                {
                    found = true;
                    break;
                }

                // Voeg alle geldige onbezochte buren toe aan de stack en sla hun parent op
                foreach (int[] move in maze.moves)
                {
                    int newRow = row + move[0];
                    int newCol = col + move[1];

                    if (!maze.IsValidMove(newRow, newCol)) continue;
                    if (visited[newRow][newCol]) continue;

                    parent[newRow][newCol] = current;
                    stack.Push(new int[] { newRow, newCol });
                }
            }

            visitedPositions.Enqueue(new int[] { -1, -1 }); // scheiding zoekfase / padweergave

            // Reconstrueer het gevonden pad door terug te lopen via de parent-array
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

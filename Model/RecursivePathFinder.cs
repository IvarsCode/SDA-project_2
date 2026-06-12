
namespace Model
{
    // Implementatie van recursieve DFS - zoekt via backtracking een pad door de maze
    public class RecursivePathFinder : IPathFinder
    {
        PathFinderType _algType = PathFinderType.Recursive;
        public PathFinderType algType { get => _algType; set { } }

        // Zet de structuren op en start de recursieve zoekfunctie vanaf de startpositie
        public void FindPath(Maze maze, int[] pos, Queue<int[]> visitedPositions)
        {
            int rows = maze.MazeArray.Length;
            int cols = maze.MazeArray[0].Length;

            bool[][] visited = new bool[rows][];
            for (int r = 0; r < rows; r++)
                visited[r] = new bool[cols];

            // currentPath bouwt stap voor stap het huidige pad op; bij backtrack wordt de laatste stap verwijderd
            var currentPath = new List<int[]>();
            bool found = Solve(maze, pos[0], pos[1], visited, currentPath, visitedPositions);

            visitedPositions.Enqueue(new int[] { -1, -1 }); // scheiding zoekfase / padweergave

            if (found)
            {
                foreach (var step in currentPath)
                    visitedPositions.Enqueue(step);
            }
        }

        // Probeert recursief vanuit (row, col) het eindpunt te bereiken; retourneert true als dat lukt
        private bool Solve(Maze maze, int row, int col, bool[][] visited, List<int[]> currentPath, Queue<int[]> visitedPositions)
        {
            if (!maze.IsValidMove(row, col)) return false;
            if (visited[row][col]) return false;

            visited[row][col] = true;
            currentPath.Add(new int[] { row, col });
            visitedPositions.Enqueue(new int[] { row, col }); // voor visualisatie van de zoekgolf

            if (row == maze.End[0] && col == maze.End[1]) return true;

            // Probeer alle richtingen; bij succes direct teruggeven zodat backtracking stopt
            foreach (int[] move in maze.moves)
            {
                if (Solve(maze, row + move[0], col + move[1], visited, currentPath, visitedPositions))
                    return true;
            }

            // Geen van de richtingen leidde naar het einde: stap verwijderen (backtrack)
            currentPath.RemoveAt(currentPath.Count - 1);
            return false;
        }
    }
}

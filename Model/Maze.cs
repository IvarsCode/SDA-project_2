
using System.Data;

namespace Model
{
    public class Maze
    {
        public int[][] MazeArray { get; private set; }
        public int[,] MazeMDArray { get; private set; }
        public int[] Begin { get; private set; }
        public int[] End { get; private set; }

        public readonly int[][] moves = {
            new int[] {  1,  0 },  //down
            new int[] { -1,  0 },  //up
            new int[] {  0, -1 },  //left
            new int[] {  0,  1 },  //right
            };

        public Maze() => GenerateMaze();
        public Maze(bool automatic = true) { if (automatic) GenerateMaze(); else GenerateFromText(MazeGrids.mazeText); }
        public Maze(int rows, int cols) { if (rows <= 0 && cols <= 0) GenerateFromText(MazeGrids.mazeText); else GenerateMaze(rows, cols); }
        public Maze(int rows, int cols, bool useDFS) => GenerateMaze(rows, cols, useDFS);
        public Maze(string lines) => GenerateFromText(lines);

        void GenerateFromText(string lines)
        {
            MazeArray = ToMazeArray(lines);
            MazeMDArray = ToMazeMDArray(lines);
        }

        void GenerateMaze(int rows = 20, int cols = 40, bool useDFS = false)
        {
            if (rows < 5) rows = 21;
            if (cols < 5) cols = 41;
            // Odd dimensions zodat rand-rijen/-kolommen altijd muren zijn
            if (rows % 2 == 0) rows++;
            if (cols % 2 == 0) cols++;

            // Alles beginnen als muur (-1)
            MazeArray = new int[rows][];
            MazeMDArray = new int[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                MazeArray[r] = new int[cols];
                for (int c = 0; c < cols; c++)
                {
                    MazeArray[r][c] = -1;
                    MazeMDArray[r, c] = -1;
                }
            }

            // Kamercellen openen: oneven rij én oneven kolom, binnen de rand
            for (int r = 1; r < rows - 1; r += 2)
                for (int c = 1; c < cols - 1; c += 2)
                    SetCell(r, c, 0);

            if (useDFS)
                GenerateDFS(rows, cols);
            else
                GenerateBinaryTree(rows, cols);

            // Start linksboven, einde rechtsonder
            Begin = new int[] { 1, 1 };
            SetCell(1, 1, 1);

            End = new int[] { rows - 2, cols - 2 };
            SetCell(rows - 2, cols - 2, 2);
        }

        // Schrijft waarde naar zowel MazeArray als MazeMDArray
        void SetCell(int r, int c, int value)
        {
            MazeArray[r][c] = value;
            MazeMDArray[r, c] = value;
        }

        // Binary Tree algoritme: elke kamer verbindt willekeurig oost of zuid
        // Eenvoudig maar met diagonale bias (NW→SE richting altijd vrij)
        void GenerateBinaryTree(int rows, int cols)
        {
            var rnd = new Random();
            for (int r = 1; r < rows - 1; r += 2)
            {
                for (int c = 1; c < cols - 1; c += 2)
                {
                    bool canEast  = c + 2 <= cols - 2;
                    bool canSouth = r + 2 <= rows - 2;

                    if (canEast && canSouth)
                    {
                        if (rnd.Next(2) == 0) SetCell(r, c + 1, 0);  // muur oost open
                        else                  SetCell(r + 1, c, 0);  // muur zuid open
                    }
                    else if (canEast)  SetCell(r, c + 1, 0);
                    else if (canSouth) SetCell(r + 1, c, 0);
                }
            }
        }

        // Recursive Backtracker (iteratieve DFS): bezoekt elke kamer via random buren
        // Produceert mazes met lange kronkelende gangen en weinig dode gangen
        void GenerateDFS(int rows, int cols)
        {
            var rnd = new Random();
            bool[][] visited = new bool[rows][];
            for (int r = 0; r < rows; r++)
                visited[r] = new bool[cols];

            // [dRij, dKol, muurrij, muurkol] naar buurkamer
            int[][] dirs = {
                new int[] {  0,  2,  0,  1 },  // oost
                new int[] {  0, -2,  0, -1 },  // west
                new int[] {  2,  0,  1,  0 },  // zuid
                new int[] { -2,  0, -1,  0 },  // noord
            };

            var stack = new Stack<int[]>();
            visited[1][1] = true;
            stack.Push(new int[] { 1, 1 });

            while (stack.Count > 0)
            {
                int[] curr = stack.Peek();
                int r = curr[0], c = curr[1];

                // Verzamel onbezochte buurkamers
                var candidates = new List<int[]>();
                foreach (var d in dirs)
                {
                    int nr = r + d[0], nc = c + d[1];
                    if (nr > 0 && nr < rows - 1 && nc > 0 && nc < cols - 1 && !visited[nr][nc])
                        candidates.Add(new int[] { nr, nc, r + d[2], c + d[3] });
                }

                if (candidates.Count > 0)
                {
                    var chosen = candidates[rnd.Next(candidates.Count)];
                    SetCell(chosen[2], chosen[3], 0);  // muur tussen kamers open
                    visited[chosen[0]][chosen[1]] = true;
                    stack.Push(new int[] { chosen[0], chosen[1] });
                }
                else
                {
                    stack.Pop();  // backtrack: geen onbezochte buren meer
                }
            }
        }

        int[][] ToMazeArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            int[][] outArray = new int[arrayLines.Length][];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];
                // row array:
                var row = new int[line.Length];
                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                        case '#':
                            row[colIdx] = -1;  //walls
                            break;
                        case '1':
                            row[colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            row[colIdx] = 2;   //end
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            row[colIdx] = 0;   //not visited
                            break;
                    }
                }
                // row in the output jagged array.
                outArray[rowIdx] = row;
            }

            return outArray;

        }

        int[,] ToMazeMDArray(string maze)
        {
            // substrings from the maze string
            var arrayLines = maze.Split(new char[] { '.', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            var lineLength = 0;
            if (arrayLines != null && arrayLines.Length > 0)
                lineLength = arrayLines[0].Length;
            else
                throw new Exception($"Maze incorrect");

            for (var rowIdx = 0; arrayLines != null && rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];
                if (arrayLines[rowIdx] == null || line.Length != lineLength)
                    throw new Exception($"Not same line length for rows in maze:\n at row 0: {lineLength}, at row {rowIdx}: {line.Length}");
            }

            int[,] outArray = new int[arrayLines.Length, lineLength];

            for (var rowIdx = 0; rowIdx < arrayLines.Length; rowIdx++)
            {
                var line = arrayLines[rowIdx];

                for (int colIdx = 0; colIdx < line.Length; colIdx++)
                {
                    //from chars to integers
                    switch (line[colIdx])
                    {
                        case 'x':
                        case '#':
                            outArray[rowIdx, colIdx] = -1;  //walls
                            break;
                        case '1':
                            outArray[rowIdx, colIdx] = 1;   //begin
                            Begin = [rowIdx, colIdx];
                            break;
                        case '2':
                            outArray[rowIdx, colIdx] = 2;   //end
                            End = [rowIdx, colIdx];
                            break;
                        default:
                            outArray[rowIdx, colIdx] = 0;   //not visited
                            break;
                    }
                }
            }
            return outArray;
        }

        static int CountNotVisited(int[][] maze)
        {
            int cnt = 0;
            if (maze != null && maze.Length > 0)
            {
                for (int rowIdx = 0; rowIdx < maze.Length; rowIdx++)
                {
                    for (int colIdx = 0; maze[rowIdx] != null && colIdx < maze[rowIdx].Length; colIdx++)
                    {
                        cnt = maze[rowIdx][colIdx] == 0 ? cnt + 1 : cnt;
                    }
                }
            }
            return cnt;
        }

        public int CountNotVisited() => CountNotVisited(MazeArray);

        static bool IsValidPos(int[][] array, int newRow, int newColumn)
        {
            // ... Ensure position is within the array bounds.
            /*
            if (newRow < 0) return false;
            if (newColumn < 0) return false;
            if (newRow >= array.Length) return false;
            if (newColumn >= array[newRow].Length) return false;
            return true;
            */
            return !(newRow < 0)
                    && !(newColumn < 0)
                    && !(newRow >= array.Length)
                    && !(newColumn >= array[newRow].Length);
        }

        // Make sure the position is within the maze array bounds.
        // no walls
        public bool IsValidMove(int newRow, int newColumn) =>
            IsValidPos(MazeArray, newRow, newColumn) &&
            !(MazeArray[newRow][newColumn] == -1); //no walls 

        //Marking strategy
        public bool IsValidMove(int newRow, int newColumn, bool notVisited = true)
        {
            // Make sure the position is within the maze array bounds.
            // no walls, not yet visited ? (flag notVisited: false)
            return notVisited ?
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1)  //no walls, but already visited -> ok
                    :
                    IsValidPos(MazeArray, newRow, newColumn) &&
                    !(MazeArray[newRow][newColumn] == -1 || MazeArray[newRow][newColumn] == 4); //no walls, not yet visited 
        }

    }

    public static class MazeGrids
    {
        //         public static string mazeText = @"
        // xxxxxx1xxxxxxxxxxxxxxxxxxxxxxx.
        //  x   x   x                    .
        // xx2x xxx   x xxxxxxxx    x xx .
        // x  x xxxxxxx xxxxxxxxxxxxx xxx.
        //  x x xx      x                .
        // x  x xx xxxxx  x xxxx xxxxx  x.
        // xx    x xxx   xx xxx  xxx   xx.
        // xxx   xxx   x xxxx   xx   x xx.
        // xx     xx   x xxxx   xx   x xx.
        // xxxx    xxxxx xx xxxx xxxxx xx.
        // xx            xx            xx.";



        public static string mazeText = @"
###################################################
#1#   #                 #           #   #         #
# # # # ############# # # ### ####### # # # ##### #
# # #   #   #         # # #   #       # # # #     #
# # #####   # ######### ### ### ####### # # #######
# # #           #     #   #     #     #   #       #
# # ###      ## # ######  # ####### # ########### #
# #   #       # #   #     # #       # #   #     # #
# ### ##     ## # #   ##### ### ##### # ### ### # #
# #   #     #   # # #       #   #   #   #   #   # #
# # ### ### # ### # #      ## ### #  ## # ### ### #
# #     #   # #   # #         #   #   # #   #     #
# ### ####### # ### #      ##   ##### ##### ##### #
# #   #       #   # #     #   # #           #   # #
# ##### ########  # ##### # ### # ##     ###### # #
#     # # #   #   #     # #   # #         #     # #
##### # # # # # ### ##### ### # ###       ### ### #
#   # # #   #   # #       #   #   #   # #     #   #
# # # # # ####### ############### # ### ####### ###
# #     #                         #   #          2#";

    }
}

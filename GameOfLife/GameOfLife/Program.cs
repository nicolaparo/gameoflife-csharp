using System;
using System.Threading.Tasks;

namespace GameOfLife
{
    class Program
    {
        static int width = 40;
        static int height = 20;

        private static Random random = new Random();

        static void Main(string[] args)
        {
            width = Console.WindowWidth / 2;
            height = Console.WindowHeight - 1;

            var matrix = createMatrix();

            do
            {
                Task.Delay(200).Wait();
                printMatrix(matrix);

                matrix = processMatrix(matrix);
            }
            while (true);
        }

        private static bool[,] createMatrix()
        {
            var matrix = new bool[width, height];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    matrix[x, y] = random.NextDouble() >= .4;

            return matrix;
        }

        private static bool[,] processMatrix(bool[,] matrix)
        {
            var newMatrix = new bool[width, height];

            /*
            
            The universe of the Game of Life is an infinite, two-dimensional orthogonal grid of square cells, each of which is in one of two possible states,             alive or dead, (or populated and unpopulated, respectively). 
            Every cell interacts with its eight neighbours, which are the cells that are horizontally, vertically, or diagonally adjacent. 
            At each step in time, the following transitions occur:

            Any live cell with fewer than two live neighbours dies, as if by underpopulation.
            Any live cell with two or three live neighbours lives on to the next generation.
            Any live cell with more than three live neighbours dies, as if by overpopulation.
            Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
            These rules, which compare the behavior of the automaton to real life, can be condensed into the following:

            Any live cell with two or three neighbors survives.
            Any dead cell with three live neighbors becomes a live cell.
            All other live cells die in the next generation. Similarly, all other dead cells stay dead.
            The initial pattern constitutes the seed of the system. The first generation is created by applying the above rules simultaneously to every cell in the seed; births and deaths occur simultaneously, and the discrete moment at which this happens is sometimes called a tick. Each generation is a pure function of the preceding one. The rules continue to be applied repeatedly to create further generations.

            */

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var neighbours = neighboursCount(matrix, x, y);

                    // Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                    if (matrix[x, y] && neighbours < 2)
                        newMatrix[x, y] = false;

                    // Any live cell with two or three live neighbours lives on to the next generation.
                    if (matrix[x, y] && (neighbours == 2 || neighbours == 3))
                        newMatrix[x, y] = true;

                    // Any live cell with more than three live neighbours dies, as if by overpopulation.
                    if (matrix[x, y] && neighbours > 3)
                        newMatrix[x, y] = false;

                    // Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
                    if (!matrix[x, y] && neighbours == 3)
                        newMatrix[x, y] = true;
                }
            }

            return newMatrix;
        }

        private static int neighboursCount(bool[,] matrix, int x, int y)
        {
            int count = 0;

            for (var yy = y - 1; yy <= y + 1; yy++)
            {
                for (var xx = x - 1; xx <= x + 1; xx++)
                {
                    if (xx == x && yy == y)
                        continue;

                    if (!existsPoint(xx, yy))
                        continue;

                    if (matrix[xx, yy])
                        count++;
                }
            }

            return count;
        }

        private static bool existsPoint(int x, int y)
        {
            if (x < 0 || x >= width)
                return false;

            if (y < 0 || y >= height)
                return false;

            return true;
        }

        private static void printMatrix(bool[,] matrix)
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (matrix[x, y])
                        Console.Write("[]");
                    else
                        Console.Write("  ");
                }
                Console.WriteLine();
            }
        }


    }

}
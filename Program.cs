using System;
using System.Threading;
using System.Linq;


// Seed a new game map
Game game = new Game(10, 10);
// A glider
game.SetAlive(1, 0);
game.SetAlive(0, 2);
game.SetAlive(1, 2);
game.SetAlive(2, 2);
game.SetAlive(2, 1);

// Dynamic display of map states
game.Launch();


// TODO: naming convention, namespace, tests
public class Game
{
    // 2d boolean map to represent cells states 
    private bool[,] map;
    private int rows, cols;

    // Screen updates delay
    public const int delay = 500;
    
    public Game(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
        this.map = new bool[rows, cols];
    }

    // Render the map as characters in the console
    // TODO: optimize to use only one Console.Write() call
    public void Render()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i,j]) { Console.Write("+"); }
                else { Console.Write("o"); }
            }
            Console.WriteLine();
        }
    }
    
    // Refresh map display in the console
    public void Refresh()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
    }

    // Scan the array to check against the rules of the game
    // and calculate next generation
    public void NextGen()
    {
        // New map to hold next generation states
        bool[,] newMap = new bool[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                //List<bool> neigh = GetNeighbours(i, j);
                int liveCount = GetNeighbours(i, j);
                bool current = map[i,j];

                // Lonely cells die
                if (current && liveCount < 2)
                {
                    newMap[i,j] = false;
                }

                // Overpopulated cells die
                else if (current && liveCount > 3)
                {
                    newMap[i,j] = false;
                }
                
                // New cell lives
                else if (!current && liveCount == 3)
                {
                    newMap[i,j] = true;
                }

                // Other cells stay the same
                else
                {
                    newMap[i,j] = current;
                }
            }
        }
        map = newMap;
    }

    // Get a list of neighbours for a given cell
    private int GetNeighbours(int i, int j)
    {
        int rowLimit = rows - 1;
        int colLimit = cols - 1;
        int res = 0;

        for (int x = Math.Max(0, i-1);
            x <= Math.Min(i+1, rowLimit);
            x++)
        {
            for (int y = Math.Max(0, j-1);
            y <= Math.Min(j+1, colLimit);
            y++)
            {
                if (x != i || y != j)
                {
                    if (map[x, y]) { res += 1; } // better way?
                }
            }
        }
        return res;
    }
    
    // Launch game on a given grid: 
    // TODO: break loop if the map stops changing?
    public void Launch()
    {
        this.Render(); // Initial state render

        while (true)
        {   
            this.Refresh();
            this.NextGen();
            this.Render();
            Thread.Sleep(Game.delay);
        }
    }

    public void SetAlive(int i, int j)
    {
        map[i, j] = true;
    }
}
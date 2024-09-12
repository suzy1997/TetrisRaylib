using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TetrisRaylib
{
    public class Grid
    {
    private int[,] grid;
    private int rows;
    private int columns;
    private int cellSize;
    public int score { get; set; }
    public Grid(int rows, int columns, int cellSize)
    {
        this.rows = rows;
        this.columns = columns;
        this.cellSize = cellSize;
        grid = new int[rows, columns];
        this.score = score;
        score = 0;

    }
    public bool IsOccupied(int x, int y)
    {
        int gridX = x / cellSize;
        int gridY = y / cellSize;

        if (gridX < 0 || gridX >= columns || gridY < 0 || gridY >= rows)
            return true;
        return grid[gridY, gridX] == 1;
    }
    public void AddBlockToGrid(Vector2[] blockCells)
    {
        foreach (var cell in blockCells)
        {
            int gridX = (int)(cell.X / cellSize);
            int gridY = (int)(cell.Y / cellSize);

            if (gridX >= 0 && gridX < columns && gridY >= 0 && gridY < rows)
            {
                grid[gridY, gridX] = 1;
            }
        }
    }
    public void ClearFullRows()
    {
        for (int y = 0; y < rows; y++)
        {
            bool isFull = true;
            for (int x = 0; x < columns; x++)
            {
                if (grid[y, x] == 0)
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                ClearRow(y);
                AddPoint();
            }
        }
    }
    private void ClearRow(int row)
    {
        for (int y = row; y > 0; y--)
        {
            for (int x = 0; x < columns; x++)
            {
                grid[y, x] = grid[y - 1, x];
            }
        }
        for (int x = 0; x < columns; x++)
        {
            grid[0, x] = 0;
        }
    }
    private int AddPoint()
    {
        score ++;
        return score;
    }
    public void Draw()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (grid[y, x] == 1)
                {
                    Raylib.DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, Color.RayWhite);
                }
            }
        }
    }
    }

}

using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;


namespace HelloWorld;

public enum Shape
{
    I,O,T,S,Z,J,L
}
public enum Movement
{
    DOWN, 
    RIGHT,
    LEFT,
    ROTATE
}
public class Block
{ 
    public Shape Shape { get; set; }
    public static Color Color { get; set; }
    public int Size { get; set; }
    public Vector2[] Cells { get; set; }
    public Block(int size,int gameAreaWidth)
    {
        Size = size;
        //Color = Color;
        //Color=Color.Brown;
        GetRandomBlock(gameAreaWidth);
    }
    public void PlayerControl(Movement move, int gameAreaWidth, int gameAreaHeight)
    {
        bool canMove = true;
        if (move == Movement.RIGHT)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].X + Size >= gameAreaWidth) // Check right boundary
                {
                    canMove = false;
                    break;
                }
            }
            if (canMove)
            {
                for (int i = 0; i < Cells.Length; i++)
                {
                    Cells[i].X += Size;
                }
            }
        }
        if (move == Movement.LEFT)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].X <= 0) // // Check left boundary
                {
                    canMove = false;
                    break;
                }
            }
            if (canMove)
            {
                for (int i = 0; i < Cells.Length; i++)
                {
                    Cells[i].X -= Size;
                }
            }
        }
        if (move == Movement.DOWN)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].Y + Size >= gameAreaHeight) // // Check bottom boundary
                {
                    canMove = false;
                    break;
                }
            }
            if (canMove)
            {
                for (int i = 0; i < Cells.Length; i++)
                {
                    Cells[i].Y += Size;
                }
            }
        }
        if (move == Movement.ROTATE)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].Y + Size >= gameAreaHeight) 
                {
                    canMove = false;
                    break;
                }
            }
            if (canMove)
            {
                RotateAroundCenter();
            }
            
        }
    }
    private void RotateAroundCenter()
    {       
        Vector2 center = CalculateCenter();

        for (int i = 0; i < Cells.Length; i++)
        {
            float relativeX = Cells[i].X - center.X;
            float relativeY = Cells[i].Y - center.Y;

            float rotatedX = relativeY;
            float rotatedY = -relativeX;

            Cells[i] = new Vector2(center.X + rotatedX, center.Y + rotatedY);
        }
    }
    private Vector2 CalculateCenter()
    {
        float minX = Cells[0].X, maxX = Cells[0].X;
        float minY = Cells[0].Y, maxY = Cells[0].Y;

        for (int i = 1; i < Cells.Length; i++)
        {
            if (Cells[i].X < minX) minX = Cells[i].X;
            if (Cells[i].X > maxX) maxX = Cells[i].X;
            if (Cells[i].Y < minY) minY = Cells[i].Y;
            if (Cells[i].Y > maxY) maxY = Cells[i].Y;
        }

        return new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
    }
    public void Draw()
    {       
        for (int i = 0; i < Cells.Length; i++)
        {
            Color = GetShapeColor(Shape);
            Raylib.DrawRectangleV(Cells[i], new Vector2(Size, Size), GetShapeColor(Shape));
        }        
    }
    public void GetRandomBlock(int gameAreaWidth)
    {
        int random = Raylib.GetRandomValue(0, 6);
        switch (random)
        {
            case 0: { Shape = Shape.I; } break;
            case 1: { Shape = Shape.O; } break;
            case 2: { Shape = Shape.T; } break;
            case 3: { Shape = Shape.S; } break;
            case 4: { Shape = Shape.Z; } break;
            case 5: { Shape = Shape.J; } break;
            case 6: { Shape = Shape.L; } break;
        }       
        Cells = GetShapeCells(Shape,Size, gameAreaWidth);        
    }
    public static Vector2[] GetShapeCells(Shape shape,int size,int gameAreaWidth)
    {
        float screenWidth = (float)gameAreaWidth;        
        float startX = screenWidth / 2 - (2 * size);
        // BLOCK DIFFERENT SHAPE
        switch (shape)
        {
            case Shape.I:
                return new Vector2[] { new(startX, size),new(startX + size, size),new(startX + 2 * size, size),new(startX + 3 * size, size) };
            case Shape.O:
                return new Vector2[] { new(startX, 0), new(startX + size, 0), new(startX, size), new(startX + size, size) };
            case Shape.T:
                return new Vector2[] { new(startX, 0), new(startX + size, 0), new(startX + 2 * size, 0), new(startX + size, size) };
            case Shape.S:
                return new Vector2[] { new(startX, 0), new(startX + size, 0), new(startX + size, size), new(startX + 2 * size, size) };
            case Shape.Z:
                return new Vector2[] { new(startX, size), new(startX + size, size), new(startX + size, 0), new(startX + 2 * size, 0) };
            case Shape.J:
                return new Vector2[] { new(startX, 0), new(startX + size, 0), new(startX + size, size), new(startX + size, 2 * size) };
            case Shape.L:
                return new Vector2[] { new(startX + size, 0), new(startX, 0), new(startX, size), new(startX, 2 * size) };
            default:
                throw new ArgumentException("Invalid shape");
        }
    }
    private Color GetShapeColor(Shape shape)
    {
        switch (shape)
        {
            case Shape.I: return Color.Maroon;
            case Shape.O: return Color.Yellow;
            case Shape.T: return Color.Pink;
            case Shape.S: return Color.Green;
            case Shape.Z: return Color.Red;
            case Shape.J: return Color.Blue;
            case Shape.L: return Color.Orange;
            default: return Color.White;
        }
    }
}

public class Grid
{
    private int[,] grid; 
    private int rows;  
    private int columns; 
    private int cellSize;

    public Grid(int rows, int columns, int cellSize)
    {
        this.rows = rows;
        this.columns = columns;
        this.cellSize = cellSize;
        grid = new int[rows, columns];
    }

    public bool IsOccupied(int x, int y)
    {
        int gridX = x / cellSize;
        int gridY = y / cellSize;

        if (gridX < 0 || gridX >= columns || gridY >= rows)
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

    public void Draw()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (grid[y, x] == 1)
                {
                    Raylib.DrawRectangle(x * cellSize, y * cellSize, cellSize, cellSize, Color.Magenta);
                }
            }
        }
    }
}
class GamePlay
{
    //WINDOW STATUS
    const int screenWidth = 800;
    const int screenHeight = 610;
    const int gameAreaWidth = 600; 
    const int gameAreaHeight = 610; 
    const int uiAreaWidth = 200; 
    const int uiAreaHeight = 600; 
                                  
    //OOP
    private static Block currentBlock;
    private static Block nextBlock;
    private static Grid grid;

    // TIMER
    private static double lastMoveTime = 0;
    private static double moveInterval = 1.0;
    // Key status
    private static bool leftPressed = false;
    private static bool rightPressed = false;
    private static bool downPressed = false;
    private static bool rotatePressed = false;
    private static bool spacePressed = false;

    static void InitGame()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "TETRIS");
        //OOP       
        currentBlock = new Block(20, gameAreaWidth);
        nextBlock = new Block(20, gameAreaWidth);
        grid = new Grid(30, 30, 20);         
    }
    public static void Main()
    {        
        InitGame();
        while (!Raylib.WindowShouldClose())
        {
            Update();
            Draw();            
        }        
        Raylib.CloseWindow();
    }
    static void Update()
    {
        #region BlockMovement
        double currentTime = Raylib.GetTime();
        if (currentTime - lastMoveTime >= moveInterval)
        {
            //MOVEDOWN
            //currentBlock.FallDown();
            if (!HasBlockTouched())
            {
                currentBlock.PlayerControl(Movement.DOWN, gameAreaWidth, gameAreaHeight);
            }
            else
            {
                grid.AddBlockToGrid(currentBlock.Cells); 
                grid.ClearFullRows();
                currentBlock = new Block(currentBlock.Size, gameAreaWidth); 
            }
            lastMoveTime = currentTime;
        }
        // PLAYERCONTROL
        if (Raylib.IsKeyDown(KeyboardKey.Left) && !leftPressed)
        {
            currentBlock.PlayerControl(Movement.LEFT, gameAreaWidth, gameAreaHeight);  // MOVELEFT
            leftPressed = true;  
        }
        if (Raylib.IsKeyUp(KeyboardKey.Left))
        {
            leftPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right) && !rightPressed)
        {
            currentBlock.PlayerControl(Movement.RIGHT, gameAreaWidth, gameAreaHeight);  // MOVERIGHT
            rightPressed = true;  
        }
        if (Raylib.IsKeyUp(KeyboardKey.Right))
        {
            rightPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down) && !downPressed)
        {
            currentBlock.PlayerControl(Movement.DOWN, gameAreaWidth, gameAreaHeight);  // MOVEDOWN
            downPressed = true; 
        }
        if (Raylib.IsKeyUp(KeyboardKey.Down))
        {
            downPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.A) && !rotatePressed)
        {
            currentBlock.PlayerControl(Movement.ROTATE, gameAreaWidth, gameAreaHeight);  // ROTATE
            rotatePressed = true;
        }
        if (Raylib.IsKeyUp(KeyboardKey.A))
        {
            rotatePressed = false;       
        }
        #endregion

        if (Raylib.IsKeyDown(KeyboardKey.Space) && !spacePressed)
        {
            currentBlock.GetRandomBlock(gameAreaWidth);
            spacePressed = true;
        }
        if (Raylib.IsKeyUp(KeyboardKey.Space))
        {
            spacePressed = false;
        }
    }
    private static bool HasBlockTouched()
    {
        foreach (var cell in currentBlock.Cells)
        {
            int targetPostionY= (int)cell.Y + currentBlock.Size;
            // touch bottom boundary  //touch other brick
            if (targetPostionY >= gameAreaHeight)
            {
                return true;
            }
            if (grid.IsOccupied((int)cell.X, targetPostionY))
            {
                return true;
            }
        }
        return false;
    }
    public static void Draw()
    { 
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib_cs.Color.White);
        //DrawGameWindow
        DrawGameArea();
        DrawUIArea();
        Raylib.EndDrawing();
    }
    public static void DrawGameArea()
    {
        Raylib.DrawRectangle(0, 0, gameAreaWidth, gameAreaHeight, Color.Gray);
        currentBlock.Draw();
        grid.Draw();
    }

    public static void DrawUIArea()
    {

        Raylib.DrawRectangle(gameAreaWidth, 0, uiAreaWidth, uiAreaHeight, Color.DarkGray);

        //Raylib.DrawText("Score: " + score, gameAreaWidth + 20, 20, 20, Color.White);
        Raylib.DrawText("Level: 1", gameAreaWidth + 20, 60, 20, Color.White);
        Raylib.DrawText("Next:", gameAreaWidth + 20, 100, 20, Color.White);


    }
}
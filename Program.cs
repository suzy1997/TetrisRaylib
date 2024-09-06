using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public int Size { get; set; }
    public Vector2[] Cells { get; set; }
    public Vector2 Position { get; set; }
    public Block(int size,int gameAreaWidth)
    {
        //Shape = shape;
        Size = size;
        Size = 20;
        //Cells = GetShapeCells(Shape, Size, gameAreaWidth);
        GetRandomBlock(gameAreaWidth);
    }
    public void FallDown()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Cells[i].Y += Size;
        }
    }
    public void PlayerControl(Movement move)
    {
        if (move == Movement.RIGHT)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].X += Size;
            }
        }
        if (move == Movement.LEFT)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].X -= Size;
            }
        }
        if (move == Movement.DOWN)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].Y += Size;
            }
        }
        if (move == Movement.ROTATE)
        {
            RotateAroundCenter();
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
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public Color[,] Cells { get; private set; }
    public Grid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Cells = new Color[rows, columns];
        // 初始化网格为空
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Cells[i, j] = Color.Black; // BLACK 表示空格
            }
        }
    }
    public void DrawGrid(Vector2 cellSize)
    {
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                //if (Cells[i, j] != Color.Black) // 如果不是空格，绘制颜色方块
                {
                    Vector2 position = new Vector2(j * cellSize.X, i * cellSize.Y);
                    Raylib.DrawRectangleV(position, cellSize, Cells[i, j]);
                }
            }
        }
    }
}
class GamePlay
{
    //WINDOW STATUS
    const int screenWidth = 800;
    const int screenHeight = 600;
    const int gameAreaWidth = 600; 
    const int gameAreaHeight = 600; 
    const int uiAreaWidth = 200; 
    const int uiAreaHeight = 600; 
                                  
    //OOP
    private static Block currentBlock;
    private static Vector2[] currentBlockPosition;
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
        currentBlock = new Block(15, gameAreaWidth);       
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
            currentBlock.FallDown();  
            lastMoveTime = currentTime;
        }
        // PLAYERCONTROL
        if (Raylib.IsKeyDown(KeyboardKey.Left) && !leftPressed)
        {
            currentBlock.PlayerControl(Movement.LEFT);  // MOVELEFT
            leftPressed = true;  
        }
        if (Raylib.IsKeyUp(KeyboardKey.Left))
        {
            leftPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right) && !rightPressed)
        {
            currentBlock.PlayerControl(Movement.RIGHT);  // MOVERIGHT
            rightPressed = true;  
        }
        if (Raylib.IsKeyUp(KeyboardKey.Right))
        {
            rightPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down) && !downPressed)
        {
            currentBlock.PlayerControl(Movement.DOWN);  // MOVEDOWN
            downPressed = true; 
        }
        if (Raylib.IsKeyUp(KeyboardKey.Down))
        {
            downPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.A) && !rotatePressed)
        {
            currentBlock.PlayerControl(Movement.ROTATE);  // ROTATE
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
    }

    public static void DrawUIArea()
    {
        // 绘制 UI 区域背景
        Raylib.DrawRectangle(gameAreaWidth, 0, uiAreaWidth, uiAreaHeight, Color.DarkGray);

        // 显示分数等内容
        //Raylib.DrawText("Score: " + score, gameAreaWidth + 20, 20, 20, Color.White);
        Raylib.DrawText("Level: 1", gameAreaWidth + 20, 60, 20, Color.White);
        Raylib.DrawText("Next:", gameAreaWidth + 20, 100, 20, Color.White);

        // 你可以在这里绘制下一个方块的提示
    }
}








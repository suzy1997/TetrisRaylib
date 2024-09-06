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
    public Color Color { get; set; }
    public int Size { get; set; }
    public Vector2[] Cells { get; set; }
    public Vector2 Position { get; set; }
    public Block(Shape shape, int size,int screenWidth)
    {
        Shape = shape;
        Size = size;
        Size = 20;
        
        Cells = GetShapeCells(Shape, Size);
         
            
        
    }
    public void Move(Vector2 direction)
    {
        Position += direction;
    }
    public void FallDown(float amount)
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            Cells[i].Y += amount;
        }
    }
    public void PlayerControl(Movement move)
    {
        if (move == Movement.RIGHT)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].X += 10;
            }
        }
        if (move == Movement.LEFT)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].X -= 10;
            }
        }
        if (move == Movement.DOWN)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i].Y += 10;
            }
        }
        if (move == Movement.ROTATE)
        {
            RotateAroundCenter();
        }
    }
    private void RotateAroundCenter()
    {
        // 计算几何中心
        Vector2 center = CalculateCenter();

        for (int i = 0; i < Cells.Length; i++)
        {
            // 计算相对于中心的相对坐标
            float relativeX = Cells[i].X - center.X;
            float relativeY = Cells[i].Y - center.Y;

            // 应用顺时针旋转 90 度的公式 (x', y') = (y, -x)
            float rotatedX = relativeY;
            float rotatedY = -relativeX;

            // 转换回绝对坐标
            Cells[i] = new Vector2(center.X + rotatedX, center.Y + rotatedY);
        }
    }
    private Vector2 CalculateCenter()
    {
        float minX = Cells[0].X, maxX = Cells[0].X;
        float minY = Cells[0].Y, maxY = Cells[0].Y;

        // 找到方块的最小和最大坐标
        for (int i = 1; i < Cells.Length; i++)
        {
            if (Cells[i].X < minX) minX = Cells[i].X;
            if (Cells[i].X > maxX) maxX = Cells[i].X;
            if (Cells[i].Y < minY) minY = Cells[i].Y;
            if (Cells[i].Y > maxY) maxY = Cells[i].Y;
        }

        // 返回几何中心
        return new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
    }
    public void Draw()
    {       
        for (int i = 0; i < Cells.Length; i++)
        {        
            Raylib.DrawRectangleV(Cells[i], new Vector2(Size, Size), GetShapeColor(Shape));
        }        
    }
    public void GetRandomBlock()
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
        Cells = GetShapeCells(Shape,Size);        
    }
    public static Vector2[] GetShapeCells(Shape shape,int size)
    {
        float screenWidth = (float)Raylib.GetScreenWidth();
        
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
    const int screenWidth = 640;
    const int screenHeight = 480;

    
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
        currentBlock = new Block(Shape.L,15,screenWidth);       
    }
    public static void Main()
    {        
        InitGame();
        while (!Raylib.WindowShouldClose())
        {
            Update();
            PressButton();
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
            currentBlock.FallDown(10);  
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
            currentBlock.GetRandomBlock();
            spacePressed = true;
        }
        if (Raylib.IsKeyUp(KeyboardKey.Space))
        {
            spacePressed = false;
        }
    }
    public static void Generate()
    {
        currentBlock.Draw();     
    }
    static void PressButton()
    {
        
    }
    public static void Draw()
    { 
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib_cs.Color.White);
        Generate();
        Raylib.EndDrawing();
    } 
}








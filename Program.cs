using Raylib_cs;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;
using static System.Formats.Asn1.AsnWriter;



namespace TetrisRaylib;
public enum Movement
{
    DOWN, 
    RIGHT,
    LEFT,
    ROTATE
} 
public enum StateType
{ 
    Stopped = 0,
    Pausing = 1,
    Gaming = 2,
    Lost = 3,
}


class GamePlay
{
    //WINDOW STATUS
    const int screenWidth = 520;
    const int screenHeight = 640;
    const int gameAreaWidth = 320; 
    const int gameAreaHeight = 640; 
    const int uiAreaWidth = 200; 
    const int uiAreaHeight = 640;
    //OOP
    public static Block currentBlock;
    public static Block nextBlock;
    public static Grid grid;
    // TIMER
    private static double lastMoveTime = 0;
    private static double moveInterval = 1.0;
    // Key status
    private static bool leftPressed = false;
    private static bool rightPressed = false;
    private static bool downPressed = false;
    private static bool rotatePressed = false;
    private static bool spacePressed = false;
    //Block Status
    const int CellSize = 32;
    //game status


    static void InitGame()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "TETRIS");
        //OOP
        //inital generate block 
        currentBlock = new Block(CellSize, gameAreaWidth);
        currentBlock.GetRandomBlockShape(gameAreaWidth);
        GetNextBlock();
        grid = new Grid(gameAreaHeight / CellSize, gameAreaWidth / CellSize, CellSize);         
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
            if (!HasBlockTouched())
            {
                currentBlock.PlayerControl(Movement.DOWN, gameAreaWidth, gameAreaHeight);
            }
            else
            {
                grid.AddBlockToGrid(currentBlock.Cells); 
                grid.ClearFullRows();
                GetNewBlock();
                
            }
            lastMoveTime = currentTime;
        }
        // PLAYERCONTROL
        if (Raylib.IsKeyDown(KeyboardKey.Left) && !leftPressed && !HasBlockTouchedSide(-1))
        {
            currentBlock.PlayerControl(Movement.LEFT, gameAreaWidth, gameAreaHeight);  // MOVELEFT
            leftPressed = true;  
        }
        if (Raylib.IsKeyUp(KeyboardKey.Left))
        {
            leftPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right) && !rightPressed && !HasBlockTouchedSide(1))
        {
            currentBlock.PlayerControl(Movement.RIGHT, gameAreaWidth, gameAreaHeight);  // MOVERIGHT
            rightPressed = true;  
        }
        if (Raylib.IsKeyUp(KeyboardKey.Right))
        {
            rightPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down) && !downPressed && !HasBlockTouched())
        {
            currentBlock.PlayerControl(Movement.DOWN, gameAreaWidth, gameAreaHeight);  // MOVEDOWN
            downPressed = true; 
        }
        if (Raylib.IsKeyUp(KeyboardKey.Down))
        {
            downPressed = false;  
        }
        if (Raylib.IsKeyDown(KeyboardKey.A) && !rotatePressed && !HasRotateBlockTouched())
        {
            currentBlock.PlayerControl(Movement.ROTATE, gameAreaWidth, gameAreaHeight);  // ROTATE
            rotatePressed = true;
        }
        if (Raylib.IsKeyUp(KeyboardKey.A))
        {
            rotatePressed = false;       
        }
        #endregion      
    }
    //ui show what is next block
    public static void GetNextBlock()
    {
        //Inital UI show what is the next block 
        nextBlock = new Block(CellSize, gameAreaWidth);
        nextBlock.GetRandomBlockShape(gameAreaWidth);
    }
    //generate new blcok
    public static void GetNewBlock()
    {  
        //nextblock to be the currenblock
        currentBlock=nextBlock;
        //generate new blcok
        GetNextBlock();       
    }

    #region Collision Detection
    private static bool HasRotateBlockTouched()
    {
        Vector2[] tarPositon = currentBlock.CaculateTarPos();
        for (int i = 0; i < tarPositon.Length; i++)
        {
            int targetX = (int)tarPositon[i].X;
            int targetY = (int)tarPositon[i].Y;
            // touch bottom boundary  //touch other brick

            if (grid.IsOccupied(targetX, targetY))
            {
                return true;
            }
        }
      
        return false;
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
    private static bool HasBlockTouchedSide(int direction)
    {
        foreach (var cell in currentBlock.Cells)
        {
            int targetPostionX = (int)cell.X + currentBlock.Size* direction;
            // touch bottom boundary  //touch other brick
            if (grid.IsOccupied(targetPostionX, (int)cell.Y))
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    public static void Draw()
    { 
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib_cs.Color.White);
        DrawGameArea();
        DrawUIArea();
        Raylib.EndDrawing();
    }
    public static void DrawGameArea()
    {
        Raylib.DrawRectangle(0, 0, gameAreaWidth, gameAreaHeight, Color.Gray);
        currentBlock.Draw(0);       
        grid.Draw();
    }

    public static void DrawUIArea()
    {
        Raylib.DrawRectangle(gameAreaWidth, 0, uiAreaWidth, uiAreaHeight, Color.DarkGray);
        Raylib.DrawText("Row Count: " + grid.score, gameAreaWidth + 20, 20, 20, Color.White);
        Raylib.DrawText("Score: " + grid.score * 10, gameAreaWidth + 20, 60, 20, Color.White);
        Raylib.DrawText("Level: 1", gameAreaWidth + 20, 100, 20, Color.White);
        Raylib.DrawText("Next:", gameAreaWidth + 20, 140, 20, Color.White);
        nextBlock.Draw(1);
    }
}
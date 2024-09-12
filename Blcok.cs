
using Raylib_cs;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TetrisRaylib
{
    public enum Shape
    {
        I, O, T, S, Z, J, L
    }
    public class Block
    {
        public Shape Shape { get; set; }
        public static Color Color { get; set; }
        public int Size { get; set; }
        public Vector2[] Cells { get; set; }
        public Block(int size, int gameAreaWidth)
        {
            Size = size;
            Color = Color;
            //GetRandomBlock(gameAreaWidth);
        }
        #region control
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
                    RotateAroundCenter(gameAreaWidth, gameAreaHeight);
                }

            }
        }
        public Vector2[] CaculateTarPos()
        {
            Vector2 center = CalculateCenter();
            Vector2[] rotatedCells = new Vector2[Cells.Length];

            for (int i = 0; i < Cells.Length; i++)
            {
                float relativeX = Cells[i].X - center.X;
                float relativeY = Cells[i].Y - center.Y;

                float rotatedX = relativeY;
                float rotatedY = -relativeX;

                float TargtPosX = (float)Math.Floor((center.X + rotatedX) / Size) * Size;
                float TargtPosY = (float)Math.Floor((center.Y + rotatedY) / Size) * Size;
                rotatedCells[i] = new Vector2(TargtPosX, TargtPosY);
            }
            return rotatedCells;

        }
        private void RotateAroundCenter(int gameAreaWidth, int gameAreaHeight)
        {

            if (!IsCollision(CaculateTarPos(), gameAreaWidth, gameAreaHeight))
            {
                Cells = CaculateTarPos();
                AlignToGrid();
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
        private void AlignToGrid()
        {
            for (int i = 0; i < Cells.Length; i++)
            {

                Cells[i] = new Vector2((float)Math.Floor(Cells[i].X / Size) * Size, (float)Math.Floor(Cells[i].Y / Size) * Size);
            }
        }
        private bool IsCollision(Vector2[] cells, int gameAreaWidth, int gameAreaHeight)
        {
            foreach (var cell in cells)
            {
                if (cell.X < 0 || cell.X >= gameAreaWidth || cell.Y >= gameAreaHeight)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        public void Draw(int BlcokStatus)
        {
            Vector2[] changpos = new Vector2[] { new(250 * BlcokStatus, 180 * BlcokStatus), new(250 * BlcokStatus, 180 * BlcokStatus), new(250 * BlcokStatus, 180 * BlcokStatus), new(250 * BlcokStatus, 180 * BlcokStatus) };
            for (int i = 0; i < Cells.Length; i++)
            {
                Color = GetShapeColor(Shape);
                Raylib.DrawRectangleV(Cells[i] + changpos[i], new Vector2(Size, Size), GetShapeColor(Shape));
            }
        }
        
        public void GetRandomBlockShape(int gameAreaWidth)
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
            GetRandomBlock(gameAreaWidth);
        }
        public void GetRandomBlock(int gameAreaWidth)
        {            
            Cells = GetShapeCells(Shape, Size, gameAreaWidth);
        }
        public Vector2[] GetShapeCells(Shape shape, int size, int gameAreaWidth)
        {
            float screenWidth = (float)gameAreaWidth;
            float startX = screenWidth / 2 - (2 * size);
            // BLOCK DIFFERENT SHAPE
            switch (shape)
            {
                case Shape.I:
                    return new Vector2[] { new(startX, 0), new(startX + size, 0), new(startX + 2 * size, 0), new(startX + 3 * size, 0) };
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
}

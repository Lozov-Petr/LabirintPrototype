using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    abstract class DrawingObject : ObjectWithPosition
    {
        internal Sprite sprite;

        public ObjectConstans constants;

        abstract public void Draw(Vector2 screenVector);

        public virtual void Update(Labirint labirint)
        {
            CurrentDirection = (slide * CurrentDirection + NewDirection) / (slide + 1);

            float lengthOfCurrentDirection = CurrentDirection.Length();

            if (lengthOfCurrentDirection > Const.Epsilon || lengthOfCurrentDirection >= NewDirection.Length()) Move(labirint);

            sprite.UpdateFrame();
        }

        public void Move(Labirint labirint)
        {
            Vector2 newPositionWithoutAllCollision = Position + CurrentDirection * Speed;
            Vector2 newPositionWithoutWallsCollision = CollisionWithObjects(labirint, newPositionWithoutAllCollision);
            Vector2 newPosition = CollisionWithWalls(labirint, newPositionWithoutWallsCollision);

            if (labirint.labirintDraw[(int)newPosition.X / Const.SizeCell, (int)newPosition.Y / Const.SizeCell].IsWall())
            {
                if (!labirint.labirintDraw[GetCellX(), (int)newPosition.Y / Const.SizeCell].IsWall()) newPosition.X = Position.X;
                else newPosition = Position;
            }

            MoveInCells(labirint, newPosition);

            UpdateCurrentDirection(newPosition);
            Position = newPosition;
        }

        Vector2 CollisionWithObjects(Labirint labirint, Vector2 newPosition)
        {
            int cellPositionX = GetCellX();
            int cellPositionY = GetCellY();

            int shiftX = -1;
            int shiftY = -1;

            if (GetShiftX() > Const.HalfSizeCell) shiftX = 1;
            if (GetShiftY() > Const.HalfSizeCell) shiftY = 1;

            for (int i = cellPositionX + shiftX; i != cellPositionX - shiftX; i -= shiftX)
                for (int j = cellPositionY + shiftY; j != cellPositionY - shiftY; j -= shiftY)
                    foreach (DrawingObject obj in labirint.labirintDraw[i, j].objects)
                    {
                        if (obj != this)
                        {
                            int summOfRadiuses = this.constants.collisionRadius + obj.constants.collisionRadius;
                            Vector2 difference = newPosition - obj.Position;
                            if (difference.Length() < summOfRadiuses)
                            {
                                if (difference != Vector2.Zero)
                                {
                                    difference.Normalize();
                                    Vector2 newPosition1 = obj.Position + difference * summOfRadiuses;
                                    Vector2 newPosition2 = obj.Position - difference * summOfRadiuses;
                                    if ((Position - newPosition1).Length() < (Position - newPosition2).Length()) newPosition = newPosition1;
                                    else newPosition = newPosition2;
                                }
                                else newPosition = Position;
                            }
                        }
                    }

            UpdateCurrentDirection(newPosition);

            return newPosition;
        }

        void UpdateCurrentDirection(Vector2 newPosition)
        {            
            CurrentDirection = (newPosition - Position) / Speed;
        }

        Vector2 CollisionWithWalls(Labirint labirint, Vector2 newPosition)
        {
            int signDirectionX = Math.Sign(CurrentDirection.X);
            int signDirectionY = Math.Sign(CurrentDirection.Y);

            if (signDirectionX == 0) signDirectionX = 1;
            if (signDirectionY == 0) signDirectionY = 1;

            int currentCellX = GetCellX();
            int currentCellY = GetCellY();

            int currentPositionCellX = currentCellX * Const.SizeCell;
            int currentPositionCellY = currentCellY * Const.SizeCell;

            int rightBorder = currentPositionCellX + Const.SizeCell - constants.collisionRadius;
            int leftBorder = currentPositionCellX + constants.collisionRadius;

            int upBorder = currentPositionCellY + constants.collisionRadius;
            int downBorder = currentPositionCellY + Const.SizeCell - constants.collisionRadius;

            bool xCellIsWall = labirint.labirintDraw[currentCellX + signDirectionX, currentCellY].IsWall();
            bool yCellIsWall = labirint.labirintDraw[currentCellX, currentCellY + signDirectionY].IsWall();

            if (xCellIsWall)
            {
                if      (signDirectionX == 1 && rightBorder < newPosition.X) newPosition.X = rightBorder;
                else if (signDirectionX == -1 && newPosition.X < leftBorder) newPosition.X = leftBorder;
            }
            if (yCellIsWall)
            {
                if      (signDirectionY == 1 && downBorder < newPosition.Y) newPosition.Y = downBorder;
                else if (signDirectionY == -1 && newPosition.Y < upBorder) newPosition.Y = upBorder;
            }

            if (!xCellIsWall && !yCellIsWall)
            {

                Cell cell = labirint.labirintDraw[currentCellX, currentCellY];
                Cell cellLU = labirint.labirintDraw[currentCellX - 1, currentCellY - 1];
                Cell cellRU = labirint.labirintDraw[currentCellX + 1, currentCellY - 1];
                Cell cellRD = labirint.labirintDraw[currentCellX + 1, currentCellY + 1];
                Cell cellLD = labirint.labirintDraw[currentCellX - 1, currentCellY + 1];

                Vector2 angle = Vector2.Zero;

                if      ((cell.angleLU - newPosition).Length() < constants.collisionRadius && cellLU.IsWall()) angle = cell.angleLU;
                else if ((cell.angleRU - newPosition).Length() < constants.collisionRadius && cellRU.IsWall()) angle = cell.angleRU;
                else if ((cell.angleRD - newPosition).Length() < constants.collisionRadius && cellRD.IsWall()) angle = cell.angleRD;
                else if ((cell.angleLD - newPosition).Length() < constants.collisionRadius && cellLD.IsWall()) angle = cell.angleLD;

                if (angle != Vector2.Zero)
                {
                    Vector2 temp = newPosition - angle;
                    if (temp.Length() < constants.collisionRadius)
                    {
                        temp.Normalize();
                        newPosition = temp * constants.collisionRadius + angle;
                    }
                }
            }

            return newPosition;
        }

        void MoveInCells(Labirint labirint, Vector2 newPosition)
        {
            int newCellPositionX = (int)newPosition.X / Const.SizeCell;
            int newCellPositionY = (int)newPosition.Y / Const.SizeCell;
            int currentCellPositionX = GetCellX();
            int currentCellPositionY = GetCellY();

            if (newCellPositionX != currentCellPositionX || newCellPositionY != currentCellPositionY)
            {
                
                Cell currentCell = labirint.labirintDraw[currentCellPositionX, currentCellPositionY];
                Cell newCell = labirint.labirintDraw[newCellPositionX, newCellPositionY];

                currentCell.objects.Remove(this);
                newCell.objects.Add(this);
            }
        }
    }
}

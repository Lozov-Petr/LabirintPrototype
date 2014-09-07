using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace LabPrototype
{
    class GenerationLogic
    {
        static Random rnd = new Random();
        static int[,] newRoom = new int[Const.SizeRoom, Const.SizeRoom];

        static int XCell;
        static int YCell;
        static bool IsEnd;
        static bool IsWalkUpEnd;
        static bool IsWalkRightEnd;
        static bool IsWalkDownEnd;
        static bool IsWalkLeftEnd;
        static int yHuntStartAgain = 0;
        public static Room GenerationLabirint(SpriteBatch spriteBatch, int column, int line)
        {
            newRoom = new int[Const.SizeRoom, Const.SizeRoom];

            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 5; ++j)
                    newRoom[i + 10, j + 10] = 2;
            newRoom[12, 9] = 2;

            Room room = new Room(spriteBatch, column, line);
            Vector2 shift = new Vector2(column, line) * Const.SizeRoom * Const.SizeCell;

            IsEnd = false;
            //MovedToAnotherCell(rnd.Next(Const.SizeRoom) / 2 * 2, rnd.Next(Const.SizeRoom) / 2 * 2); //обнуляем все переменные и задаем стартовое значение
            MovedToAnotherCell(0, 0);

            while (!IsEnd)
            {
                Walk();
                Hunt();
            }

            for (int i = 0; i < Const.SizeRoom; ++i)
                for (int j = 0; j < Const.SizeRoom; ++j)
                {
                    room.room[i, j] = new Cell(spriteBatch, shift + new Vector2(i * Const.SizeCell, j * Const.SizeCell));
                    if (newRoom[i, j] == 0) room.room[i, j].Type = CellType.WALL;
                }

            return room;
        }

        static void Walk()
        {
            newRoom[XCell, YCell] = 1;

            IsWalkUpEnd = false;
            IsWalkRightEnd = false;
            IsWalkDownEnd = false;
            IsWalkLeftEnd = false;

            while (IsWalkUpEnd == false || IsWalkRightEnd == false || IsWalkDownEnd == false || IsWalkLeftEnd == false)
            {
                int direction = RandomizeDirection();

                if (direction == 0 && !IsWalkUpEnd)
                {
                    if (YCell - 2 >= 0)
                    {
                        if (newRoom[XCell, YCell - 2] == 0)
                        {
                            newRoom[XCell, YCell - 1] = 1;
                            newRoom[XCell, YCell - 2] = 1;
                            MovedToAnotherCell(XCell, YCell - 2);
                        }
                        else
                        {
                            IsWalkUpEnd = true;
                        }
                    }
                    else
                    {
                        IsWalkUpEnd = true;
                    }
                }
                else if (direction == 1 && !IsWalkRightEnd)
                {
                    if (XCell + 2 < Const.SizeRoom)
                    {
                        if (newRoom[XCell + 2, YCell] == 0)
                        {
                            newRoom[XCell + 1, YCell] = 1;
                            newRoom[XCell + 2, YCell] = 1;
                            MovedToAnotherCell(XCell + 2, YCell);
                        }
                        else
                        {
                            IsWalkRightEnd = true;
                        }
                    }
                    else
                    {
                        IsWalkRightEnd = true;
                    }
                }
                else if (direction == 2 && !IsWalkDownEnd)
                {
                    if (YCell + 2 < Const.SizeRoom)
                    {
                        if (newRoom[XCell, YCell + 2] == 0)
                        {
                            newRoom[XCell, YCell + 1] = 1;
                            newRoom[XCell, YCell + 2] = 1;
                            MovedToAnotherCell(XCell, YCell + 2);
                        }
                        else
                        {
                            IsWalkDownEnd = true;
                        }
                    }
                    else
                    {
                        IsWalkDownEnd = true;
                    }
                }
                else if (direction == 3 && !IsWalkLeftEnd)
                {
                    if (XCell - 2 >= 0)
                    {
                        if (newRoom[XCell - 2, YCell] == 0)
                        {
                            newRoom[XCell - 1, YCell] = 1;
                            newRoom[XCell - 2, YCell] = 1;
                            MovedToAnotherCell(XCell - 2, YCell);
                        }
                        else
                        {
                            IsWalkLeftEnd = true;
                        }
                    }
                    else
                    {
                        IsWalkLeftEnd = true;
                    }
                }
                
            }
        }

        static void Hunt()
        {
            for (int i = yHuntStartAgain; i <= Const.SizeRoom - 2; i += 2)
            {
                for (int j = 0; j <= Const.SizeRoom - 2; j += 2)
                {
                    if (newRoom[i, j] == 1)
                    {
                        if (i > 0 && i < Const.SizeRoom - 2 && j > 0 && j < Const.SizeRoom - 2)
                        {
                            if (newRoom[i, j + 2] == 0 || newRoom[i + 2, j] == 0 || newRoom[i, j - 2] == 0 || newRoom[i - 2, j] == 0)
                            {
                                yHuntStartAgain = i;
                                XCell = i;
                                YCell = j;
                                return;
                            }
                        }
                        else if (j == 0)
                        {
                            if (i == 0)
                            {
                                if (newRoom[i, j + 2] == 0 || newRoom[i + 2, j] == 0)
                                {
                                    yHuntStartAgain = i;
                                    XCell = i;
                                    YCell = j;
                                    return;
                                }
                            }
                            else if (i == Const.SizeRoom - 2)
                            {
                                if (newRoom[i, j + 2] == 0 || newRoom[i - 2, j] == 0)
                                {
                                    yHuntStartAgain = i;
                                    XCell = i;
                                    YCell = j;
                                    return;
                                }
                            }
                            else if (i > 0 && i < Const.SizeRoom - 2)
                            {
                                if (newRoom[i + 2, j] == 0 || newRoom[i, j + 2] == 0 || newRoom[i - 2, j] == 0)
                                {
                                    yHuntStartAgain = i;
                                    XCell = i;
                                    YCell = j;
                                    return;
                                }
                            }
                        }
                        else if (j == Const.SizeRoom - 2)
                        {
                            if (i == 0)
                            {
                                if (newRoom[i, j - 2] == 0 || newRoom[i + 2, j] == 0)
                                {
                                    yHuntStartAgain = i;
                                    XCell = i;
                                    YCell = j;
                                    return;
                                }
                            }
                            else if (i == Const.SizeRoom - 2)
                            {
                                if (newRoom[i, j - 2] == 0 || newRoom[i - 2, j] == 0)
                                {
                                    yHuntStartAgain = i;
                                    XCell = i;
                                    YCell = j;
                                    return;
                                }
                            }
                            else if (i > 0 && i < Const.SizeRoom - 2)
                            {
                                if (newRoom[i + 2, j] == 0 || newRoom[i, j - 2] == 0 || newRoom[i - 2, j] == 0)
                                {
                                    yHuntStartAgain = i;
                                    XCell = i;
                                    YCell = j;
                                    return;
                                }
                            }
                        }
                    }
                } 
            }
            IsEnd = true;
        }

        static void MovedToAnotherCell(int x, int y)
        {
            XCell = x;
            YCell = y;
            IsWalkUpEnd = false;
            IsWalkRightEnd = false;
            IsWalkDownEnd = false;
            IsWalkLeftEnd = false;
        }

        static int RandomizeDirection()
        {
            int minRnd = 0;
            int maxRnd = 0;

            if (IsWalkUpEnd == false) minRnd = 0;
            else
                if (IsWalkRightEnd == false) minRnd = 1;
                else
                    if (IsWalkDownEnd == false) minRnd = 2;
                    else
                        if (IsWalkLeftEnd == false) minRnd = 3;

            if (IsWalkLeftEnd == false) maxRnd = 4;
            else
                if (IsWalkDownEnd == false) maxRnd = 3;
                else
                    if (IsWalkRightEnd == false) maxRnd = 2;
                    else
                        if (IsWalkUpEnd == false) maxRnd = 1;
            return rnd.Next(minRnd, maxRnd);
        }
    }
}

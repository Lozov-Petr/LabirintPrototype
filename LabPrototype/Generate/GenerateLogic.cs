using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    class GenerateLogic
    {
        static Random rnd = new Random();
        public static Room GenerateRoom(SpriteBatch spriteBatch, int column, int line)
        {
            int currentMultiplicity = 0; //Текушее множество
            int currentMultiplicityCount = 0; //Кол-во ячеек в множестве
            int currentMultiplicityCountBorder = 0; //Кол-во стен в множестве         

            int[,] Multiplicity = new int[Const.SizeRoom, Const.SizeRoom];
            Room room = new Room(spriteBatch, column, line);
            Vector2 shift = new Vector2(column, line) * Const.SizeRoom * Const.SizeCell;
            
            for (int i = 0; i <= Const.SizeRoom - 2; i += 2) //столбцы
            {
                for (int j = 0; j <= Const.SizeRoom - 2; j += 2) //строки
                {
                    if (i == 0) //Первая строка
                    {
                        Multiplicity[i, j] = j + 1; //Присоединим все ячейки не принадлежащие множествам к свои новым множествам
                    }
                    else
                    {
                        if (i != Const.SizeRoom - 2)
                        {
                            if (Multiplicity[i - 2, j] != 0 && Multiplicity[i - 1, j] != 0)
                            {
                                Multiplicity[i, j] = Multiplicity[i - 2, j];
                            }
                            else Multiplicity[i, j] = j + 1;
                        }
                        else
                        {
                            Multiplicity[i, j] = Multiplicity[i - 2, j];
                            if (j != Const.SizeRoom - 2) Multiplicity[i, j + 1] = Multiplicity[i - 2, j + 1];
                        }
                    }


                    if (i != Const.SizeRoom - 2) //Если не первая ячейка и не последняя строка
                    {
                        if (0 < j &&  Multiplicity[i, j - 2] != 0) //Нужна ли тут проверка?
                        {
                            if (rnd.NextDouble() <= 0.5f) //Объеденяем
                            {
                                Multiplicity[i, j] = Multiplicity[i, j - 2];
                                Multiplicity[i, j - 1] = Multiplicity[i, j - 2];
                            }
                            else //Ставим стену
                            {
                                Multiplicity[i, j - 1] = 0;
                            }
                        }
                    }
                    else //последняя строка
                    {
                        if (j != 0)
                        {
                            if (Multiplicity[i, j - 1] == 0) //Читерство и костыль
                            {
                                if (rnd.NextDouble() <= 0.5f) //Объеденяем
                                {
                                    Multiplicity[i - 2, j - 1] = Multiplicity[i, j];
                                }
                                else
                                {
                                    Multiplicity[i, j - 1] = Multiplicity[i, j];
                                }
                            }
                        }
                    }

                }

                currentMultiplicity = 0;
                currentMultiplicityCount = 0;
                currentMultiplicityCountBorder = 0;

                if (i != Const.SizeRoom - 2) //Не последняя строка
                {
                    for (int j = 0; j <= Const.SizeRoom - 2; j += 1) //строки
                    {
                        if (currentMultiplicity != Multiplicity[i, j])
                        {
                            currentMultiplicity = Multiplicity[i, j];
                            currentMultiplicityCount = 1;
                            currentMultiplicityCountBorder = 0;
                        }
                        else
                        {
                            currentMultiplicityCount += 1;
                        }

                        if (j != Const.SizeRoom - 2) //Не последняя ячейка
                        {
                            if (currentMultiplicityCount == 1)
                            {
                                if (currentMultiplicity != Multiplicity[i, j + 1]) //Множество одинарной длинны
                                {
                                    Multiplicity[i + 1, j] = Multiplicity[i, j]; //делаем проход вниз
                                }
                                else
                                {
                                    if (rnd.NextDouble() <= 0.5f) //Объеденяем
                                    {
                                        Multiplicity[i + 1, j] = Multiplicity[i, j]; //делаем проход вниз
                                    }
                                    else
                                    {
                                        Multiplicity[i + 1, j] = 0; //делаем проход вниз
                                        currentMultiplicityCountBorder++;
                                    }
                                }
                            }
                            else
                            {
                                if (currentMultiplicityCount == currentMultiplicityCountBorder + 1 && currentMultiplicity != Multiplicity[i, j + 1])
                                {
                                    Multiplicity[i + 1, j] = Multiplicity[i, j]; //делаем проход вниз
                                }
                                else
                                {
                                    Multiplicity[i + 1, j] = 0;
                                    currentMultiplicityCountBorder++;
                                }
                            }
                        }
                        else
                        {
                            if (currentMultiplicityCount == 1)
                            {
                                Multiplicity[i + 1, j] = Multiplicity[i, j]; //делаем проход вниз
                            }
                            else
                            {
                                if (currentMultiplicityCount == currentMultiplicityCountBorder + 1)
                                {
                                    Multiplicity[i + 1, j] = Multiplicity[i, j]; //делаем проход вниз
                                }
                                else
                                {
                                    Multiplicity[i + 1, j] = 0;
                                    currentMultiplicityCountBorder++;
                                }
                            }
                        }

                    }
                }                
            }

            for (int i = 0; i < Const.SizeRoom; ++i)
                for (int j = 0; j < Const.SizeRoom; ++j)
                {
                    room.room[i, j] = new Cell(spriteBatch, shift + new Vector2(i * Const.SizeCell, j * Const.SizeCell));
                    if (Multiplicity[j, i] == 0) room.room[i, j].Type = CellType.WALL;
                }

            return room;
        }
    }
}

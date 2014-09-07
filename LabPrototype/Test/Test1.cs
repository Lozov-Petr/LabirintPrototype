using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype.Test
{
    class Test1
    {
        public static Labirint GetLab1(SpriteBatch spriteBatch)
        {
            Vector2 playerPosition = new Vector2((Const.SizeRoom + Const.SizeRoom / 2) * 64, (Const.SizeRoom + Const.SizeRoom / 2) * 64);
            //new Vector2((Const.SizeRoom + 1) * Const.SizeCell, (Const.SizeRoom + 1) * Const.SizeCell) + ObjectConstans.PlayerConstans.shiftForDrawing;
            Labirint lab = new Labirint(spriteBatch, playerPosition);

            lab.labirintLogic[1, 1] = GenerationLogic.GenerationLabirint(spriteBatch, 1, 1);

            for (int i1 = 0; i1 < Const.SizeLabirint; ++i1)
                for (int j1 = 0; j1 < Const.SizeLabirint; ++j1)
                    for (int i2 = 0; i2 < Const.SizeRoom; ++i2)
                        for (int j2 = 0; j2 < Const.SizeRoom; ++j2)
                            lab.labirintDraw[i1 * Const.SizeRoom + i2, j1 * Const.SizeRoom + j2] = lab.labirintLogic[i1, j1].room[i2, j2];


            lab.player.Speed = 4;
            //lab.labirintLogic[1, 1].room[3, 3].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[4, 3].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[5, 3].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[3, 5].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[4, 5].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[5, 5].Type = CellType.WALL;

            //lab.labirintLogic[1, 1].room[9, 3].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[9, 4].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[9, 5].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[8, 4].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[10, 4].Type = CellType.WALL;

            //lab.labirintLogic[1, 1].room[3, 8].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[3, 9].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[3, 10].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[4, 8].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[5, 8].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[5, 9].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[5, 10].Type = CellType.WALL;

            //lab.labirintLogic[1, 1].room[8, 8].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[10, 8].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[8, 10].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[10, 10].Type = CellType.WALL;

            //lab.labirintLogic[1, 1].room[3, 12].Type = CellType.WALL;
            //lab.labirintLogic[1, 1].room[4, 13].Type = CellType.WALL;

            var rnd = new Random();

            for (int i = 0; i < 0; ++i)
            {
                var a = new FirstEnemy(spriteBatch, new Vector2(rnd.Next(20 * 64, 40 * 64), rnd.Next(20 * 64, 40 * 64)));
                a.Speed = rnd.Next(2, 4);
                a.constants.collisionRadius = rnd.Next(3, 16);
                a.constants.color = new Color(rnd.Next(40, 200), rnd.Next(40, 200), rnd.Next(40, 200));
                lab.objects.Add(a);   
            }

             for (int i = 0; i < 0; ++i)
            {
                var a = new FirstItem(spriteBatch, new Vector2(rnd.Next(20 * 64, 40 * 64), rnd.Next(20 * 64, 40 * 64)));
                a.Speed = rnd.Next(2, 4);
                a.constants.collisionRadius = 32;
                a.constants.shiftForDrawing = new Vector2(32, 32);
                a.constants.color = new Color(rnd.Next(40, 200), rnd.Next(40, 200), rnd.Next(40, 200));
                lab.objects.Add(a);   
            }

            //lab.objects.Add(new FirstItem(spriteBatch, lab.player.Position));

            return lab;
        }
    }
}

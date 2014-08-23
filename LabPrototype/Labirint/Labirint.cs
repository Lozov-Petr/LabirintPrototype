using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using LabPrototype.Test;

namespace LabPrototype
{
    class Labirint
    {
        public Room[,] labirintLogic;
        public Cell[,] labirintDraw;
        public Camera camera;
        public Player player;

        public List<DrawingObject> objects;

        int screenZeroCellX; // Координата X левой верхний клетки на экране
        int screenZeroCellY; // Координата Y левой верхний клетки на экране

        public Labirint(SpriteBatch spriteBatch, Vector2 playerPosition)
        {
            labirintLogic = new Room[Const.SizeLabirint, Const.SizeLabirint];
            labirintDraw = new Cell[Const.SizeTotal, Const.SizeTotal];
            camera = new Camera(playerPosition);
            player = new Player(spriteBatch, playerPosition);

            objects = new List<DrawingObject>();
            objects.Add(player);

            for (int i = 0; i < Const.SizeLabirint - 1; ++i)
            {
                labirintLogic[0, i] = Room.GetRoadRoom(spriteBatch);
                labirintLogic[i, Const.SizeLabirint - 1] = Room.GetRoadRoom(spriteBatch);
                labirintLogic[Const.SizeLabirint - 1, i + 1] = Room.GetRoadRoom(spriteBatch);
                labirintLogic[i + 1, 0] = Room.GetRoadRoom(spriteBatch);
            }

            for (int i = 1; i < Const.SizeLabirint - 1; ++i)
                for (int j = 1; j < Const.SizeLabirint - 1; ++j)
                    labirintLogic[i, j] = new Room(spriteBatch);

            WriteInCell();
        }

        void WriteInCell()
        {
            for (int i1 = 0; i1 < Const.SizeLabirint; ++i1)
                for (int j1 = 0; j1 < Const.SizeLabirint; ++j1)
                    for (int i2 = 0; i2 < Const.SizeRoom; ++i2)
                        for (int j2 = 0; j2 < Const.SizeRoom; ++j2)
                            labirintDraw[i1 * Const.SizeRoom + i2, j1 * Const.SizeRoom + j2] = labirintLogic[i1, j1].room[i2, j2];
        }

        public void Draw()
        {
            screenZeroCellX = camera.GetCellX() - Const.SizeViewX / 2;
            screenZeroCellY = camera.GetCellY() - Const.SizeViewY / 2;

            Vector2 screenVector = camera.Position - new Vector2(ResolutionOfScreen.GetVWidth() / 2, ResolutionOfScreen.GetVHeight() / 2);

            // Вывод клеток
            for (int i = 0; i < Const.SizeViewX; ++i)
                for (int j = 0; j < Const.SizeViewY; ++j)
                    if (screenZeroCellX + i >= 0 && screenZeroCellY + j >= 0)
                        labirintDraw[screenZeroCellX + i, screenZeroCellY + j].Draw((screenZeroCellX + i) * Const.SizeCell - screenVector.X, (screenZeroCellY + j) * Const.SizeCell - screenVector.Y);

            // Вывод объектов
            foreach (DrawingObject obj in objects) obj.Draw(screenVector); 
        }

        public void Update(GameTime gameTime)
        {
            // Обновление клеток
            for (int i = 0; i < Const.SizeViewX; ++i)
                for (int j = 0; j < Const.SizeViewY; ++j)
                    if (screenZeroCellX + i >= 0 && screenZeroCellY + j >= 0)
                    labirintDraw[screenZeroCellX + i, screenZeroCellY + j].Update(gameTime);

            // Обновление объектов
            foreach (DrawingObject obj in objects) obj.Update(this, gameTime); 

            // Обновление камеры
            camera.Update(player.Position);
        }

        public static Labirint Generate(SpriteBatch spriteBatch)
        {
            Labirint lab =  Test1.GetLab1(spriteBatch);
            lab.AddObjectsInCells();
            return lab;
        }

        void AddObjectsInCells()
        {
            foreach (DrawingObject obj in objects)
                labirintDraw[obj.GetCellX(), obj.GetCellY()].objects.Add(obj);
        }
    }
}

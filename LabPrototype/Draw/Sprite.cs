#region File Description
//-----------------------------------------------------------------------------
// Sprite.cs
// Author: DrKillJoy aka Apanasevich Egor
//
// Version: 1.5
// Data: 24.08.2014
//
// Удалены лишние перегрузки и метод DrawAnimation
// 
//-----------------------------------------------------------------------------
#endregion
#region using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace LabPrototype
{
    class Sprite
    {
        public static int CountDrawingSprite;
        
        public Texture2D spriteTexture;

        int frameCount; //Кол-во фреймов(кадров) анимации
        int frameStringCount; //Кол-во фреймов(кадров) анимации в строке рисунка
        int frameRowCount; //Кол-во фреймов(кадров) анимации в столбце рисунка
        int countInterationForNextFrame; //Время в мс для показа фрейма
        int currentInteration; //Время для расчета задержки
        int currentFrame; //Индекс показанного фрейма
        int currentString;

        public int frameWidth { get; private set; }  //Ширина фрейма
        public int frameHeight { get; private set; }  //Высота фрейма
        Rectangle rectangle;

        SpriteBatch spriteBatch;


        /// <summary>
        /// Класс спрайта
        /// </summary>
        /// <param name="texture">Текстура.</param>
        /// <param name="spriteBatch">Используемый SpriteBatch.</param>
        /// <param name="frameStringCount">Кол-во фреймов в строке.</param>
        /// <param name="frameRowCount">Кол-во столбцов.</param>
        /// <param name="framesPer60Interation">Кол-во фреймов в 60 обновлений.</param>
        public Sprite(Texture2D texture, SpriteBatch spriteBatch, int frameStringCount = 1, int frameRowCount = 1, int framesPer60Interation = 5)
        {
            this.spriteTexture = texture;
            Init(spriteBatch, frameStringCount, frameRowCount, framesPer60Interation);
        }

        /// <summary>
        /// Класс спрайта
        /// </summary>
        /// <param name="content">Используемый ContentManager.</param>
        /// <param name="stringTexture">Имя текстуры.</param>
        /// <param name="spriteBatch">Используемый SpriteBatch.</param>
        /// <param name="frameStringCount">Кол-во фреймов в строке.</param>
        /// <param name="frameRowCount">Кол-во столбцов.</param>
        /// <param name="framesPer60Interation">Кол-во фреймов в 60 обновлений.</param>
        public Sprite(ContentManager content, String stringTexture, SpriteBatch spriteBatch, int frameStringCount = 1, int frameRowCount = 1, int framesPer60Interation = 5) //Объект спрайт (всего фреймов, фреймов в строке, скорость отображения)
        {
            Load(content, stringTexture);
            Init(spriteBatch, frameStringCount, frameRowCount, framesPer60Interation);
        }

        /// <summary>
        /// Загрузка спрайта в игру
        /// </summary>
        /// <param name="content">Используемый ContentManager.</param>
        /// <param name="stringTexture">Имя текстуры.</param>
        void Load(ContentManager content, String stringTexture)//Загрузка спрайта в игру
        {
            spriteTexture = content.Load<Texture2D>(stringTexture);
        }

        /// <summary>
        /// Инициализация спрайта
        /// </summary>
        /// <param name="spriteBatch">Используемый SpriteBatch.</param>
        /// <param name="frameStringCount">Кол-во фреймов в строке.</param>
        /// <param name="frameRowCount">Кол-во столбцов.</param>
        /// <param name="framesPer60Interation">Кол-во фреймов в 60 обновлений.</param>
        void Init(SpriteBatch spriteBatch, int frameStringCount, int frameRowCount, int framesPer60Interation)
        {
            this.spriteBatch = spriteBatch;

            this.frameWidth = spriteTexture.Width / frameStringCount;  //Ширина фрейма
            this.frameHeight = spriteTexture.Height / frameRowCount; //Высота фрейма

            this.frameStringCount = frameStringCount;
            this.frameRowCount = frameRowCount;
            this.frameCount = frameStringCount * frameRowCount;

            if (framesPer60Interation > 0) countInterationForNextFrame = 60 / framesPer60Interation; else countInterationForNextFrame = 0;
            currentFrame = 0;
            currentInteration = 0;
        }

        /// <summary>
        /// Установить скорость анимции
        /// </summary>
        /// <param name="FrameCountPer60Interation">Кол-во фреймов в 60 обновлений.</param>
        public void SetupFramePer60Interation(int FrameCountPer60Interation)
        {
            countInterationForNextFrame = 60 / FrameCountPer60Interation;
        }

        /// <summary>
        /// Цикличный переход по фреймам - анимация
        /// </summary>
        public void UpdateFrame()//Цикличный переход по фреймам - анимация
        {
            currentInteration++;
            if (currentInteration >= countInterationForNextFrame)
            {
                currentFrame++;
                if (currentFrame >= frameCount) currentFrame = 0;
                currentInteration = 0;
            }
        }
        
        /// <summary>
        /// Отрисовка спрайта
        /// </summary>
        /// <param name="Frames">Массив кадров.</param>
        /// <param name="position">Позиция рисуемого спрайта.</param>
        /// <param name="color">Общий цвет рисуемого спрайта.</param>
        /// <param name="origin">Вектор оси вращения.</param>
        /// <param name="Rotation">Угол поворота (Rad).</param>
        /// <param name="FlipH">Отразить слева направо.</param>
        /// <param name="ScaleX">Масштаб по оси X (1 = 100%).</param>
        /// <param name="ScaleY">Масштаб по оси Y (1 = 100%).</param>
        /// <param name="Depth">Глубина отрисовки спрайта (типо ось Z).</param>
        public void Draw(int[] Frames, Vector2 position, Color color, Vector2 origin, float Rotation = 0, bool FlipHorisontal = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)
        {
            rectangle.X = (int)position.X - (int)(origin.X * ScaleX);
            rectangle.Y = (int)position.Y - (int)(origin.Y * ScaleY);
            rectangle.Width = (int)(frameWidth * ScaleX);
            rectangle.Height = (int)(frameHeight * ScaleY);

            if (ResolutionOfScreen.WindowsRectangle.Intersects(rectangle))
            {
                if (currentFrame >= Frames.Length) currentFrame = 0; //Место для фидбека
                int currentFrameInAnimation = Frames[currentFrame]; // Номер кадра

                if (frameRowCount != 1) currentString = currentFrameInAnimation / frameRowCount; else currentString = 0;

                rectangle.X = frameWidth * (currentFrameInAnimation - (frameStringCount * currentString));
                rectangle.Y = frameHeight * currentString;
                rectangle.Width = frameWidth;
                rectangle.Height = frameHeight;

                SpriteEffects effect = new SpriteEffects();
                effect = SpriteEffects.None;
                if (FlipHorisontal == true) effect = SpriteEffects.FlipHorizontally; else effect = SpriteEffects.None;

                spriteBatch.Draw(spriteTexture, position, rectangle, color, Rotation, origin, new Vector2(ScaleX, ScaleY), effect, Depth);
                CountDrawingSprite++;
            }
        }
        /// <summary>
        /// Отрисовка спрайта
        /// </summary>
        /// <param name="FrameMin">Начальный кадр.</param>
        /// <param name="FrameMax">Конечный кадр.</param>
        /// <param name="position">Позиция рисуемого спрайта.</param>
        /// <param name="color">Общий цвет рисуемого спрайта.</param>
        /// <param name="origin">Вектор оси вращения.</param>
        /// <param name="Rotation">Угол поворота (Rad).</param>
        /// <param name="FlipH">Отразить слева направо.</param>
        /// <param name="ScaleX">Масштаб по оси X (1 = 100%).</param>
        /// <param name="ScaleY">Масштаб по оси Y (1 = 100%).</param>
        /// <param name="Depth">Глубина отрисовки спрайта (типо ось Z).</param>
        public void Draw(int FirstFrame, int EndFrame, Vector2 position, Color color, bool FlipHorisontal = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)
        {
            int[] Frames = new int[EndFrame + 1 - FirstFrame];
            for (int i = 0; i <= EndFrame - FirstFrame; ++i) Frames[i] = FirstFrame + i;
            Draw(Frames, position, color, Vector2.Zero, 0f, FlipHorisontal, ScaleX, ScaleY, Depth);
        }
        /// <summary>
        /// Отрисовка спрайта
        /// </summary>
        /// <param name="Frame">Начальный кадр.</param>
        /// <param name="FrameMax">Конечный кадр.</param>
        /// <param name="position">Позиция рисуемого спрайта.</param>
        /// <param name="color">Общий цвет рисуемого спрайта.</param>
        /// <param name="origin">Вектор оси вращения.</param>
        /// <param name="Rotation">Угол поворота (Rad).</param>
        /// <param name="FlipH">Отразить слева направо.</param>
        /// <param name="ScaleX">Масштаб по оси X (1 = 100%).</param>
        /// <param name="ScaleY">Масштаб по оси Y (1 = 100%).</param>
        /// <param name="Depth">Глубина отрисовки спрайта (типо ось Z).</param>
        public void Draw(int Frame, Vector2 position, Color color, Vector2 origin, float Rotation = 0, bool FlipHorisontal = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)
        {
            Draw(new int[1] { Frame }, position, color, origin, Rotation, FlipHorisontal, ScaleX, ScaleY, Depth);
        }
    }

}

#region File Description
//-----------------------------------------------------------------------------
// Sprite.cs
// Author: DrKillJoy aka Apanasevich Egor
//
// Version: 1.2
// Data: 16.08.2013
//
// !!!! Изминения... Проба, переделать загрузку.
// 
//-----------------------------------------------------------------------------
#endregion
#region using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace LabPrototype
{
    class Sprite
    {
        #region  Переменные

        public static int CountSprite;
        
        public Texture2D spriteTexture;

        public int frameCount; //Кол-во фреймов(кадров) анимации
        public int frameStringCount; //Кол-во фреймов(кадров) анимации в строке рисунка
        public int frameRowCount; //Кол-во фреймов(кадров) анимации в столбце рисунка
        private double timeFrame; //Время в мс для показа фрейма
        public int frame; //Индекс показанного фрейма
        private double totalElapsed; //Время для расчета задержки

        int frameWidth;  //Ширина фрейма
        int frameHeight; //Высота фрейма
        int stringID = 0; //Идентификатор строки фрейма
        Rectangle rectangle;

        SpriteBatch spriteBatch;
        #endregion




        #region  Класс спрайта
        /// <summary>
        /// Класс спрайта
        /// </summary>
        public Sprite(ContentManager content, String stringTexture, SpriteBatch spriteBatch)
        {
            this.frameCount = 1;
            this.frameStringCount = 1;
            this.frameRowCount = 1;
            timeFrame = 1f / 5;
            frame = 0;
            totalElapsed = 0;

            Load(content, stringTexture, spriteBatch);
        }

        public Sprite(Texture2D texture, SpriteBatch spriteBatch, int frameCount = 1, int frameStringCount = 1, int frameRowCount = 1, int framesPerSec = 1)
        {
            this.spriteTexture = texture;
            this.spriteBatch = spriteBatch;
            this.frameWidth = spriteTexture.Width / frameStringCount;  //Ширина фрейма
            this.frameHeight = spriteTexture.Height / frameRowCount; //Высота фрейма
            this.frameCount = frameCount;
            this.frameStringCount = frameStringCount;
            this.frameRowCount = frameRowCount;
            timeFrame = 1f / framesPerSec;
            frame = 0;
            totalElapsed = 0;
        }

        /// <summary>
        /// Класс спрайта
        /// </summary>
        /// <remarks> текст </remarks>
        /// <param name="frameCount">Всего фреймов в спрайте.</param>
        /// <param name="frameStringCount">Кол-во фреймов в строке.</param>
        /// <param name="frameRowCount">Кол-во фреймов в столбце.</param>
        /// <param name="framesPerSec">Частота кадров в секунду.</param>
        public Sprite(ContentManager content, String stringTexture, SpriteBatch spriteBatch, int frameCount, int frameStringCount, int frameRowCount, int framesPerSec) //Объект спрайт (всего фреймов, фреймов в строке, скорость отображения)
        {
            this.frameCount = frameCount;
            this.frameStringCount = frameStringCount;
            this.frameRowCount = frameRowCount;
            timeFrame = 1f / framesPerSec;
            frame = 0;
            totalElapsed = 0;

            Load(content, stringTexture, spriteBatch);
        }

        /// <summary>
        /// Загрузка спрайта в игру
        /// </summary>
        /// <param name="content">Используемый ContentManager.</param>
        /// <param name="stringTexture">Имя текстуры.</param>
        /// <param name="lSpriteBatch">Используемый SpriteBatch.</param>
        void Load(ContentManager content, String stringTexture, SpriteBatch spriteBatch)//Загрузка спрайта в игру
        {
            
            spriteTexture = content.Load<Texture2D>(stringTexture);
            this.spriteBatch = spriteBatch;
            this.frameWidth = spriteTexture.Width / frameStringCount;  //Ширина фрейма
            this.frameHeight = spriteTexture.Height / frameRowCount; //Высота фрейма
        }
        #endregion

        public int GetFrameWidth()
        {
            return frameWidth;
        }
        public int GetFrameHeight()
        {
            return frameHeight;
        }

        public void setup_frame_in_secund(int FrameCountInSecond)
        {
            timeFrame = 1f / FrameCountInSecond;
        }

        #region  Цикличный переход по фреймам - анимация
        /// <summary>
        /// Цикличный переход по фреймам - анимация
        /// </summary>
        /// <param name="elapsed">Время прошедшее с последнего кадра.</param>
        public void UpdateFrame(double elapsed)//Цикличный переход по фреймам - анимация
        {
            totalElapsed += elapsed;
            if (totalElapsed >= timeFrame)
            {
                frame++;
                if (frame >= frameCount) frame = 0;
                totalElapsed = 0;
            }
        }

        /// <summary>
        /// Цикличный переход по фреймам - анимация
        /// </summary>
        /// <param name="elapsed">Время прошедшее с последнего кадра.</param>
        public void UpdateFrame(double elapsed, ref double total_elapsed, ref int frame)//Цикличный переход по фреймам - анимация
        {
            total_elapsed += elapsed;
            if (total_elapsed > timeFrame)
            {
                frame++;
                if (frame >= frameCount) frame = 0;
                total_elapsed = 0;
            }
        }
        #endregion




        # region Draw
        /// <summary>
        /// Отрисовка простого спрайта
        /// </summary>
        /// <param name="spriteBatch">Используемый SpriteBatch.</param>
        /// <param name="frameID">ID рисуемого фрейма.</param>
        /// <param name="position">Позиция рисуемого спрайта.</param>
        /// <param name="color">Общий цвет рисуемого спрайта.</param>
        /// <param name="origin">Вектор оси вращения.</param>
        /// <param name="Rotation">Угол поворота (Rad).</param>
        /// <param name="FlipH">Отразить слева направо.</param>
        /// <param name="ScaleX">Масштаб по оси X (1 = 100%).</param>
        /// <param name="ScaleY">Масштаб по оси Y (1 = 100%).</param>
        /// <param name="IsGround">Применяется для рисования тайлов земли (+2 пикселя по бокам, для правильного сглаживания).</param>
        /// <param name="Depth">Глубина отрисовки спрайта (типо ось Z).</param>
        public void Draw(int frameID, Vector2 position, Color color, Vector2 origin, float Rotation = 0, bool FlipH = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1) //Рисуем простой спрайт (spriteBatch, номер фрейма, цвет спрайта, вектор оси (origin), поворот спрайта, отразить спрайт слева направо, масштаб по X, масштаб по Y, таил земли, глубина спрайта)
        {
            stringID = 0; //Идентификатор строки фрейма
            frame = frameID;
            //rectangle.X = (int)position.X - (int)origin.X;
            //rectangle.Y = (int)position.Y - (int)origin.Y;
            //rectangle.Width = (int)(frameWidth * ScaleX);
            //rectangle.Height = (int)(frameHeight * ScaleY);

            //if (Const.WindowsRectangle.Intersects(rectangle))
            //{

                if (frame < frameStringCount) //Фрейм в первой строке
                {
                    rectangle.X = frameWidth * frameID;
                    rectangle.Y = 0;
                    rectangle.Width = frameWidth;
                    rectangle.Height = frameHeight;
                }
                else
                {
                    for (int i = frame; frameStringCount <= i; i -= frameStringCount)
                    {
                        stringID++; //Переходим к следующей строке
                        frame -= frameStringCount; //Убираем из фрейма строку
                    }
                    rectangle.X = frameWidth * frame;
                    rectangle.Y = frameHeight * stringID;
                    rectangle.Width = frameWidth;
                    rectangle.Height = frameHeight;
                }

                SpriteEffects effect = new SpriteEffects();
                effect = SpriteEffects.None;
                if (FlipH == true) effect = SpriteEffects.FlipHorizontally; else effect = SpriteEffects.None;

                spriteBatch.Draw(spriteTexture, position, rectangle, color, Rotation, origin, new Vector2(ScaleX, ScaleY), effect, Depth);
                CountSprite++;
            //}
        }
        public void Draw(float ScaleX, float ScaleY, float Depth = 1)
        {
            Draw(0, Vector2.Zero, Color.White, Vector2.Zero, 0, false, ScaleX, ScaleY, Depth);
        }

        public void Draw(Vector2 position, Color color)
        {
            Draw(0, position, color, Vector2.Zero, 0, false, 1, 1, 0);
        }

        public void Draw(int frameID, Rectangle rectangle, Color color)
        {
            Draw(frameID, new Vector2(rectangle.X, rectangle.Y), color, Vector2.Zero, 0, false, rectangle.Width / (float)frameWidth, rectangle.Height / (float)frameHeight, 0);
        }
        #endregion

        public void DrawAnimation(ref double total_elapsed, ref int frame, int[] animFrameNumbers, Vector2 position, Color color, Vector2 origin, float Rotation = 0, bool FlipH = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)//Рисуем анимированный спрайт (spriteBatch, № первого фрейма анимации, № последнего фрейма)
        {
            
            stringID = 0; //Идентификатор строки фрейма

            rectangle.X = (int)position.X - (int)origin.X;
            rectangle.Y = (int)position.Y - (int)origin.Y;
            rectangle.Width = (int)(frameWidth * ScaleX);
            rectangle.Height = (int)(frameHeight * ScaleY);

            if (Const.WindowsRectangle.Intersects(rectangle))
            {
                if (frame >= animFrameNumbers.Length) frame = 0; //Место для фидбека
                int frameID = animFrameNumbers[frame]; // Номер кадра

                if (frame <= frameStringCount) //Фрейм в первой строке
                {
                    rectangle.X = frameWidth * frameID;
                    rectangle.Y = 0;
                    rectangle.Width = frameWidth;
                    rectangle.Height = frameHeight;
                }
                else
                {
                    for (int i = frame; frameStringCount <= i; i -= frameStringCount)
                    {
                        stringID++; //Переходим к следующей строке
                        frame -= frameStringCount; //Убираем из фрейма строку
                    }
                    rectangle.X = frameWidth * frame;
                    rectangle.Y = frameHeight * stringID;
                    rectangle.Width = frameWidth;
                    rectangle.Height = frameHeight;
                }


                SpriteEffects effect = new SpriteEffects();
                effect = SpriteEffects.None;
                if (FlipH == true) effect = SpriteEffects.FlipHorizontally; else effect = SpriteEffects.None;

                spriteBatch.Draw(spriteTexture, position, rectangle, color, Rotation, origin, new Vector2(ScaleX, ScaleY), effect, Depth);
                CountSprite++;
            }
        }
        /// <summary>
        /// Отрисовка анимированного спрайта
        /// </summary>
        /// <param name="animFrameNumbers">Массив кадров.</param>
        /// <param name="position">Позиция рисуемого спрайта.</param>
        /// <param name="color">Общий цвет рисуемого спрайта.</param>
        /// <param name="origin">Вектор оси вращения.</param>
        /// <param name="Rotation">Угол поворота (Rad).</param>
        /// <param name="FlipH">Отразить слева направо.</param>
        /// <param name="ScaleX">Масштаб по оси X (1 = 100%).</param>
        /// <param name="ScaleY">Масштаб по оси Y (1 = 100%).</param>
        /// <param name="Depth">Глубина отрисовки спрайта (типо ось Z).</param>
        public void DrawAnimation(int[] animFrameNumbers, Vector2 position, Color color, Vector2 origin, float Rotation = 0, bool FlipH = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)
        {
            stringID = 0; //Идентификатор строки фрейма

            rectangle.X = (int)position.X - (int)origin.X;
            rectangle.Y = (int)position.Y - (int)origin.Y;
            rectangle.Width = (int)(frameWidth * ScaleX);
            rectangle.Height = (int)(frameHeight * ScaleY);

            if (Const.WindowsRectangle.Intersects(rectangle))
            {
                if (frame >= animFrameNumbers.Length) frame = 0; //Место для фидбека
                int frameID = animFrameNumbers[frame]; // Номер кадра

                if (frameID < frameStringCount) //Фрейм в первой строке
                {
                    rectangle.X = frameWidth * frameID;
                    rectangle.Y = 0;
                    rectangle.Width = frameWidth;
                    rectangle.Height = frameHeight;
                }
                else
                {
                    for (int i = frameID; frameStringCount <= i; i -= frameStringCount)
                    {
                        stringID++; //Переходим к следующей строке
                        frameID -= frameStringCount; //Убираем из фрейма строку
                    }
                    rectangle.X = frameWidth * frameID;
                    rectangle.Y = frameHeight * stringID;
                    rectangle.Width = frameWidth;
                    rectangle.Height = frameHeight;
                }
                

                SpriteEffects effect = new SpriteEffects();
                effect = SpriteEffects.None;
                if (FlipH == true) effect = SpriteEffects.FlipHorizontally; else effect = SpriteEffects.None;

                spriteBatch.Draw(spriteTexture, position, rectangle, color, Rotation, origin, new Vector2(ScaleX, ScaleY), effect, Depth);
                CountSprite++;
            }
        }
        public void DrawAnimation(int animFrameMin, int animFrameMax, Vector2 position, Color color, bool FlipH = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)
        {
            int[] animFrameNumbers = new int[animFrameMax + 1 - animFrameMin];
            for (int i = 0; i <= animFrameMax - animFrameMin; ++i) animFrameNumbers[i] = animFrameMin + i;
            DrawAnimation(animFrameNumbers, position, color, Vector2.Zero, 0f, FlipH, ScaleX, ScaleY, Depth);
        }
        public void DrawAnimation(ref double total_elapsed, ref int frame, int animFrameMin, int animFrameMax, Vector2 position, Color color, bool FlipH = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)
        {
            int[] animFrameNumbers = new int[animFrameMax + 1 - animFrameMin];
            for (int i = 0; i <= animFrameMax - animFrameMin; ++i) animFrameNumbers[i] = animFrameMin + i;
            DrawAnimation(ref total_elapsed, ref frame, animFrameNumbers, position, color, Vector2.Zero, 0f, FlipH, ScaleX, ScaleY, Depth);
        }
        public void DrawAnimation(ref double total_elapsed, ref int frame, int[] animFrameNumbers, Vector2 position, Color color, bool FlipH = false, float ScaleX = 1, float ScaleY = 1, float Depth = 1)
        {
            DrawAnimation(ref total_elapsed, ref frame, animFrameNumbers, position, color, Vector2.Zero, 0f, FlipH, ScaleX, ScaleY, Depth);
        }



        #region  IntersectPixels
        /// <summary>
        /// Определяет, имеется ли перекрытие непрозрачных пикселей
        /// между двумя спрайтов.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Найти пределах прямоугольника пересечение
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Проверьте каждую точку в пределах пересечения границы
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Получение цвета обоих пикселей в этой точке
                    Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // Если оба пиксели не полностью прозрачным,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // то пересечение было обнаружено
                        return true;
                    }
                }
            }

            // Никакого пересечения не найдено
            return false;
        }
        #endregion

    }

}

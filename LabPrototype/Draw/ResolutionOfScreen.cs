using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    static class ResolutionOfScreen
    {
        static private GraphicsDeviceManager Device = null;

        static private int Width = 800; //Ширина окна
        static private int Height = 600; //Высота окна
        static private int VWidth = 1024; //Виртуальная ширина
        static private int VHeight = 768; //Виртуальная высота
        static private bool FullScreen = false; //Полный экран
        static private bool Stretch = false; //Растягивать изображение
        static private SpriteBatch SpriteBatch; //Отсылка к спрайтбатч
        static public RenderTarget2D target; //Рендертаргет
        static public float x_ratio = 1;
        static public float y_ratio = 1;
        static private bool SynchronizeWithVerticalRetrace;

        /// <summary>
        /// Инициализация и получение пораметров для работы с окном
        /// </summary>
        /// <param name="device">Ссылка на графическое устройство.</param>
        /// <param name="spriteBatch">Ссылка на SpriteBatch.</param>
        /// <param name="winWidth">Ширина окна.</param>
        /// <param name="winHeight">Высота окна.</param>
        /// <param name="virtualWidth">Виртуальная ширина окна.</param>
        /// <param name="virtualHeight">Виртуальная высота окна.</param>
        /// <param name="isFullScreen">Окно на полный экран.</param>
        /// <param name="isStretch">Растягивание изображения.</param>
        /// <param name="synchronizeWithVerticalRetrace">Вертикальная синхронизация.</param>
        static public void Init(ref GraphicsDeviceManager device, ref SpriteBatch spriteBatch, int winWidth, int winHeight, int virtualWidth, int virtualHeight, bool isFullScreen = false, bool isStretch = false, bool synchronizeWithVerticalRetrace = false) //Инициализация и получение пораметров
        {
            Init(ref device, ref spriteBatch, synchronizeWithVerticalRetrace);
            SetResolution(winWidth, winHeight, isFullScreen, isStretch);
            SetVirtualResolution(virtualWidth, virtualHeight);
        }

        static private void Init(ref GraphicsDeviceManager device, ref SpriteBatch spriteBatch, bool synchronizeWithVerticalRetrace) //Инициализация и получение пораметров
        {
            //Const.WindowsRectangle = new BoundingBox(Vector3.Zero, new Vector3(VWidth, VHeight, 0));
            Const.WindowsRectangle = new Rectangle(0, 0, VWidth, VHeight);
            Device = device;
            SpriteBatch = spriteBatch;
            Width = device.PreferredBackBufferWidth;
            Height = device.PreferredBackBufferHeight;

            SynchronizeWithVerticalRetrace = synchronizeWithVerticalRetrace;

            ApplyResolutionSettings();

        }
        static public void SetResolution(int WinWidth, int WinHeight, bool rFullScreen, bool rStretch) //Установить разрешение окна
        {
            Width = WinWidth;
            Height = WinHeight;
            FullScreen = rFullScreen;
            Stretch = rStretch;

            ApplyResolutionSettings();
        }
        static public void SetVirtualResolution(int VirtualWidth, int VirtualHeight) //Установить виртуальное разрешение
        {
            VWidth = VirtualWidth;
            VHeight = VirtualHeight;
            //Const.WindowsRectangle = new BoundingBox(Vector3.Zero, new Vector3(VWidth, VHeight, 0));
            Const.WindowsRectangle = new Rectangle(0, 0, VWidth, VHeight);
            target = new RenderTarget2D(Device.GraphicsDevice, VWidth, VHeight);
            x_ratio = Width / VWidth;
            y_ratio = Height / VHeight;
            //dirtyMatrix = true;
        }
        static private void ApplyResolutionSettings() //Приминение настроек
        {

            if (FullScreen == false) //Полноэкранное приложение
            {
                if ((Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    Device.PreferredBackBufferWidth = Width;
                    Device.PreferredBackBufferHeight = Height;
                    Device.SynchronizeWithVerticalRetrace = SynchronizeWithVerticalRetrace;
                    Device.IsFullScreen = FullScreen;



                    Device.ApplyChanges();
                }
            }
            else
            {
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    //str = string.Format("{0}, {1}", str, dm);
                    // Check the width and height of each mode against the passed values
                    if ((dm.Width == Width) && (dm.Height == Height))
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        Device.PreferredBackBufferWidth = Width;
                        Device.PreferredBackBufferHeight = Height;
                        Device.IsFullScreen = FullScreen;
                        Device.ApplyChanges();
                    }
                }
                //File.WriteAllText("C:\\Temp2\\1.txt", str);
            }

            //dirtyMatrix = true;

            Width = Device.PreferredBackBufferWidth;
            Height = Device.PreferredBackBufferHeight;
        }
        static public void BeginDraw() //Начало отрисовки
        {
            Sprite.CountSprite = 0;
            Device.GraphicsDevice.SetRenderTarget(target);
            Device.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        static public void EndDraw(float fade = 1f) //Конец отрисовки
        {
            Device.GraphicsDevice.SetRenderTarget(null);
            Device.GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            if (Stretch == false) //Растягивать изображение или нет
            {
                float yR = (1f / VWidth) * Device.PreferredBackBufferWidth;
                float xR = (1f / VWidth) * Device.PreferredBackBufferWidth;

                Vector2 TargetPos = new Vector2(0, (Device.PreferredBackBufferHeight - (VHeight * yR)) / 2);
                SpriteBatch.Draw(target, TargetPos, new Rectangle(0, 0, VWidth, VHeight), Color.White * fade, 0f, Vector2.Zero, new Vector2(xR, yR), SpriteEffects.None, 1);
            }
            else
            {
                float yR = (1f / VHeight) * Device.PreferredBackBufferHeight;
                float xR = (1f / VWidth) * Device.PreferredBackBufferWidth;

                SpriteBatch.Draw(target, Vector2.Zero, new Rectangle(0, 0, VWidth, VHeight), Color.White * fade, 0f, Vector2.Zero, new Vector2(xR, yR), SpriteEffects.None, 1);
            }
            SpriteBatch.End();
        }

        static public int GetVWidth()
        {
            return VWidth;
        }
        static public int GetVHeight()
        {
            return VHeight;
        }
        static public int GetWidth()
        {
            return Width;
        }
        static public int GetHeight()
        {
            return Height;
        }
    }
}

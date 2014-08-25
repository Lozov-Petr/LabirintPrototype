using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    class ResolutionOfScreen
    {
        static private GraphicsDeviceManager Device = null;
        static public Rectangle WindowsRectangle { get; private set; }
        static public int Width { get; private set; } //Ширина окна
        static public int Height { get; private set; } //Высота окна
        static public int VirtualWidth {get; private set;} //Виртуальная ширина
        static public int VirtualHeight { get; private set; } //Виртуальная высота
        static private bool isFullScreen = false; //Полный экран
        static private bool isStretch = false; //Растягивать изображение
        static private SpriteBatch SpriteBatch; //Отсылка к спрайтбатч
        static private RenderTarget2D target; //Рендертаргет
        static private float x_ratio;
        static private float y_ratio;
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
            WindowsRectangle = new Rectangle(0, 0, VirtualWidth, VirtualHeight);
            Device = device;
            SpriteBatch = spriteBatch;
            Width = device.PreferredBackBufferWidth;
            Height = device.PreferredBackBufferHeight;

            SynchronizeWithVerticalRetrace = synchronizeWithVerticalRetrace;
        }
        static public void SetResolution(int width, int height, bool isFullScreen, bool isStretch) //Установить разрешение окна
        {
            Width = width;
            Height = height;
            ResolutionOfScreen.isFullScreen = isFullScreen;
            ResolutionOfScreen.isStretch = isStretch;

            ApplyResolutionSettings();
        }
        static public void SetVirtualResolution(int virtualWidth, int virtualHeight) //Установить виртуальное разрешение
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            WindowsRectangle = new Rectangle(0, 0, VirtualWidth, VirtualHeight);
            target = new RenderTarget2D(Device.GraphicsDevice, VirtualWidth, VirtualHeight);
            x_ratio = (1f / VirtualHeight) * Device.PreferredBackBufferHeight;
            y_ratio = (1f / VirtualWidth) * Device.PreferredBackBufferWidth;
        }
        static private void ApplyResolutionSettings() //Приминение настроек
        {

            if (isFullScreen == false) //Полноэкранное приложение
            {
                if ((Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                    && (Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
                {
                    Device.PreferredBackBufferWidth = Width;
                    Device.PreferredBackBufferHeight = Height;
                    Device.SynchronizeWithVerticalRetrace = SynchronizeWithVerticalRetrace;
                    Device.IsFullScreen = isFullScreen;

                    Device.ApplyChanges();
                }
            }
            else
            {
                foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                {
                    if ((dm.Width == Width) && (dm.Height == Height))
                    {
                        Device.PreferredBackBufferWidth = Width;
                        Device.PreferredBackBufferHeight = Height;
                        Device.SynchronizeWithVerticalRetrace = SynchronizeWithVerticalRetrace;
                        Device.IsFullScreen = isFullScreen;
                        Device.ApplyChanges();
                    }
                }
            }

            Width = Device.PreferredBackBufferWidth;
            Height = Device.PreferredBackBufferHeight;
        }
        static public void BeginDraw() //Начало отрисовки
        {
            FrameRateCounter.BeginDraw();
            Sprite.CountDrawingSprite = 0;
            Device.GraphicsDevice.SetRenderTarget(target);
            Device.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        static public void EndDraw(float fade = 1f) //Конец отрисовки
        {
            Device.GraphicsDevice.SetRenderTarget(null);
            Device.GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            if (isStretch == false) //Растягивать изображение или нет
            {
                Vector2 TargetPos = new Vector2(0, (Device.PreferredBackBufferHeight - (VirtualHeight * x_ratio)) / 2);
                SpriteBatch.Draw(target, TargetPos, new Rectangle(0, 0, VirtualWidth, VirtualHeight), Color.White * fade, 0f, Vector2.Zero, new Vector2(x_ratio, x_ratio), SpriteEffects.None, 1);
            }
            else
            {
                SpriteBatch.Draw(target, Vector2.Zero, new Rectangle(0, 0, VirtualWidth, VirtualHeight), Color.White * fade, 0f, Vector2.Zero, new Vector2(x_ratio, y_ratio), SpriteEffects.None, 1);
            }
            SpriteBatch.End();
            FrameRateCounter.EndDraw();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LabPrototype
{
    class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Labirint labirint;
        SpriteFont debugFont;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            Primitives.Init(GraphicsDevice, Const.VirtualWidth, Const.VirtualHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadTexture.Load(Content);
            labirint = Labirint.Generate(spriteBatch);
            ResolutionOfScreen.Init(ref graphics, ref spriteBatch, 1920, 1080, Const.VirtualWidth, Const.VirtualHeight, false, false, true);
            debugFont = Content.Load<SpriteFont>(ContentNames.Font);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            FrameRateCounter.BeginUpdate();
            GetValueKeyboard();

            labirint.Update();
            FrameRateCounter.EndUpdate();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            ResolutionOfScreen.BeginDraw();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);

            labirint.Draw();

            spriteBatch.DrawString(debugFont, String.Format("FPS: {0} \nTime for drawing: {1} ms\nTime for updating: {2} ms\nSprites: {3}\nCell: {4},{5}\nShift: {6},{7} ", FrameRateCounter.FPS, FrameRateCounter.timeForDraw, FrameRateCounter.timeForUpdate, Sprite.CountDrawingSprite, labirint.player.GetCellX(), labirint.player.GetCellY(), labirint.player.GetShiftX(), labirint.player.GetShiftY()), new Vector2(20, 20), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            /////
            //foreach (DrawingObject obj in labirint.objects)
            //    Primitives.AddCircle(obj.Position - (labirint.camera.Position - new Vector2(ResolutionOfScreen.VirtualWidth / 2, ResolutionOfScreen.VirtualHeight / 2)), obj.constants.collisionRadius, Color.Pink);
            /////

            spriteBatch.End();

            Primitives.Draw(ref graphics, ref spriteBatch);

            ResolutionOfScreen.EndDraw();
            FrameRateCounter.Update(gameTime);

            base.Draw(gameTime);
        }

        void GetValueKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            Vector2 newDirection = Vector2.Zero;

            if (state.IsKeyDown(ConstControl.Rigth)) newDirection.X++;
            if (state.IsKeyDown(ConstControl.Left)) newDirection.X--;
            if (state.IsKeyDown(ConstControl.Up)) newDirection.Y--;
            if (state.IsKeyDown(ConstControl.Down)) newDirection.Y++;

            if (newDirection == Vector2.Zero) labirint.player.NewDirection = Vector2.Zero;
            else labirint.player.NewDirection = Vector2.Normalize(newDirection);

            if (state.IsKeyDown(Keys.Escape)) Exit();
        }
    }
}

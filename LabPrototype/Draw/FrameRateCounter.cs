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
    static class FrameRateCounter
    {
        static int frameRate = 0;
        static int frameCounter = 0;
        static public int FPS = 0;
        static public int MaxFPS = 0;
        static public int MinFPS = 10000;
        static TimeSpan elapsedTime = TimeSpan.Zero;

        static public void Update(GameTime gameTime)
        {

            elapsedTime += gameTime.ElapsedGameTime;
            frameCounter++;
            if (elapsedTime >= TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            if (frameRate > MaxFPS) MaxFPS = frameRate;
            if ((frameRate < MinFPS) && (frameRate != 0)) MinFPS = frameRate;
            FPS = frameRate;
        }

    }
}


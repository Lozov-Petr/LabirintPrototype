using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LabPrototype
{
    class FrameRateCounter
    {
        static int frameRate = 0;
        static int frameCounter = 0;
        static public int FPS { get; private set; }
        static public int MaxFPS { get; private set; }
        static public int MinFPS { get; private set; }
        static TimeSpan elapsedTime = TimeSpan.Zero;

        static Stopwatch timeUpdate = new Stopwatch();
        static Stopwatch timeDraw = new Stopwatch();
        static public double timeForUpdate { get; private set; }
        static public double timeForDraw { get; private set; }
        static double tempTimeForUpdate;
        static double tempTimeForDraw;

        static FrameRateCounter()
        {
            MinFPS = 10000;
        }

        static public void Update(GameTime gameTime)
        {

            elapsedTime += gameTime.ElapsedGameTime;
            frameCounter++;
            if (elapsedTime >= TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;

                timeForUpdate = tempTimeForUpdate;
                timeForDraw = tempTimeForDraw;
            }
            if (frameRate > MaxFPS) MaxFPS = frameRate;
            if ((frameRate < MinFPS) && (frameRate != 0)) MinFPS = frameRate;
            FPS = frameRate;
        }

        static public void BeginUpdate()
        {
            timeUpdate.Start();
        }
        static public void EndUpdate()
        {
            tempTimeForUpdate = timeUpdate.Elapsed.TotalMilliseconds;
            timeUpdate.Reset();
        }
        static public void BeginDraw()
        {
            timeDraw.Start();
        }
        static public void EndDraw()
        {
            tempTimeForDraw = timeDraw.Elapsed.TotalMilliseconds;
            timeDraw.Reset();
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabPrototype
{
    class Camera : ObjectWithPosition
    {
        public Camera(Vector2 position)
        {
            Position = position;
        }

        public void Update(Vector2 newPosition)
        {
            Vector2 step = (newPosition - Position) / ConstCamera.CountStep;
            if (step.Length() > Const.Epsilon) Position += step;
        }

    }
}

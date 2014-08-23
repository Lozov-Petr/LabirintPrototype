using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabPrototype
{
    class LoadTexture
    {
        public static void Load(ContentManager content)
        {
            Cell.texture = content.Load<Texture2D>(ContentNames.Blocks);
            Player.texture = content.Load<Texture2D>(ContentNames.Player);
            FirstEnemy.texture = content.Load<Texture2D>(ContentNames.Player);
            FirstItem.texture = content.Load<Texture2D>(ContentNames.Key);
        }
    }
}

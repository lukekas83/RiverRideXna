﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiverRideGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRideGame
{
    public class Su22Enemy : SceneElement
    {
        public Su22Enemy(Texture2D[] texture, string id, World w) : base(texture, id, w) { }

        public override void Update(GameTime gameTime)
        {
            int moveBy = random.Next(3, 10);

            if (Direction == Direction.LeftToRight)
                RectPosition.X += moveBy;
            else
                RectPosition.X -= moveBy;

            if (RectPosition.Intersects(RectStart))
            {
                Direction = Direction.LeftToRight;
            }
            else if (RectPosition.Intersects(RectEnd))
            {
                Direction = Direction.RightToLeft;
            }
        }
    }
}

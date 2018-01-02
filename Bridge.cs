using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiverRideGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRideGame
{
    public class Bridge : SceneElement
    {
        public Bridge(Texture2D[] texture, string id, World w) : base(texture, id, w) 
        {
            Direction = Direction.BottomTop;
            isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isActive)
            {
                foreach (SceneElement se in w.SceneElements)
                {
                    if (se is TankEnemy)
                    {
                        if ((se as TankEnemy).IsOnBridge)
                        {
                            TankEnemy te = se as TankEnemy;
                            if (te.RectPosition.Intersects(RectPosition))
                                te.IsActive = false;
                            else
                            {
                                te.RectEnd = new Rectangle(1, 1, 1, 1);
                                if (RectPosition.X > te.RectPosition.X)
                                    te.Direction = Direction.LeftToRight;
                                else
                                    te.Direction = Direction.RightToLeft;
                            }
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive)
                return;

            spriteBatch.Begin();

            Rectangle tr = new Rectangle(
                    RectPosition.X,
                    RectPosition.Y + w.offsetY,
                    RectPosition.Width,
                    RectPosition.Height);

            spriteBatch.Draw(texture[0], tr, Color.White);
            spriteBatch.End();
        }

    }
}

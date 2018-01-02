using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiverRideGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRideGame
{
    public class TankEnemy : SceneElement
    {
        private const float FIREDELAY = .5f;        
        public Bullet[] Bullets = new Bullet[10];
        public bool IsOnBridge = false;

        public TankEnemy(Texture2D[] texture, string id, World w) : base(texture, id, w) 
        {
            for (int i = 0; i < Bullets.Length; i++)
                Bullets[i] = new Bullet(RiverRide.BulletTextureH, "", w);            
        }

        public override void Update(GameTime gameTime)
        {
            if (RectEnd != new Rectangle(1, 1, 1, 1))
            {
                int moveBy = 1;

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
            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < Bullets.Length; i++)
                Bullets[i].Update(gameTime);
            
            if(isActive)
                Fire();
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            for (int i = 0; i < Bullets.Length; i++)
                Bullets[i].Draw(spriteBatch);
        }    

        private void Fire()
        {            
            if (elapsed > FIREDELAY)
            {
                for (int i = 0; i < Bullets.Length; i++)
                {
                    if (!Bullets[i].IsActive)
                    {
                        Vector2 position = Vector2.Zero;
                        position.X = RectPosition.X + RiverRide.BulletTextureH.Width;
                        position.Y = RectPosition.Y + w.offsetY;
                        Bullets[i].Position = position;
                        Bullets[i].Direction = Direction;
                        Bullets[i].IsActive = false;
                        break;
                    }
                }              

                elapsed = 0;
            }
        }
    }
}

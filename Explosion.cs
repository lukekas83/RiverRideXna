using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRideGame
{
    public class Explosion
    {
        private Texture2D texture;
        private Vector2 framePosition = Vector2.Zero;
        private Rectangle sourceRectangle = Rectangle.Empty;
        private float elapsed = 0f;
        private Rectangle position;
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        private bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Explosion(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsed > 0.05f)
            {
                if (framePosition.X != 192)
                {
                    framePosition.X += 64;
                }
                else
                {
                    framePosition.X = 0;

                    if (framePosition.Y != 192)
                    {
                        framePosition.Y += 64;
                    }
                    else
                    {
                        framePosition.Y = 0;
                    }
                }

                if (framePosition.X == 192 && framePosition.Y == 192)
                {
                    isActive = false;
                }

                elapsed = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteBlendMode.Additive);
            spriteBatch.Draw(texture, position, new Rectangle((int)framePosition.X, (int)framePosition.Y, 64, 64), Color.White);
            spriteBatch.End();
        }
    }
}

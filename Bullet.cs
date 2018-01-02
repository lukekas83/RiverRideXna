using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRideGame
{
    public class Bullet : SceneElement
    {
        private const int MOVESPEED = 700;
        private Vector2 delta = Vector2.Zero;
        private Vector2 position = Vector2.Zero;
        public Vector2 Position        
        {
            get { return position; }
            set { position = value; }
        }        

        public Bullet(Texture2D texture, string id, World w) : base(new Texture2D[] { texture }, "" , w)
        {            
            IsActive = false;
        }

        public override void Update(GameTime gameTime)
        {
            delta = Vector2.Zero;

            if (Direction == Direction.BottomTop)
            {
                delta.Y = MOVESPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(Direction == Direction.LeftToRight)
                delta.X = -MOVESPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (Direction == Direction.RightToLeft)
                delta.X = MOVESPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;

            position -= delta;

            RectPosition = new Rectangle((int)position.X, (int)position.Y,
               texture[0].Width, texture[0].Height);

            if (position.Y < 200 && Direction == Direction.BottomTop && !w.EndOfScrolling)
            {
                IsActive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Rectangle tr = new Rectangle(
                    RectPosition.X,
                    RectPosition.Y,
                    RectPosition.Width,
                    RectPosition.Height);

            spriteBatch.Draw(texture[0], tr, Color.White);
            spriteBatch.End();
        }
    }
}

//#define DRAW_COLLISIONS

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRideGame
{
    public class ScrollingBackground
    {        
        private Texture2D[] textures;        

        private Vector2 offset = Vector2.Zero;
        private Vector2 position = Vector2.Zero;

        private float elapsed = 0f;
        World w = null;
        private float scrolled = 0;
        private int Height;

        public ScrollingBackground(Texture2D[] textures, ref World w)
        {
            this.textures = textures;
            this.w = w;
            foreach (Texture2D t in this.textures)            
                Height += t.Height;
            offset.Y -= Height - RiverRide.SCREEN_HEIGHT;
        }

        public void Update(GameTime gameTime)
        {
            if (w.EndOfScrolling)
                return;

            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            offset.Y += w.MoveSpeed * elapsed;
            scrolled += w.MoveSpeed * elapsed;
            w.offsetY = (int)offset.Y;

            //System.Diagnostics.Debug.WriteLine(Math.Abs(scrolled));

            if (offset.Y > -1)
            {
                w.EndOfScrolling = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            position.Y = offset.Y;
            
            Vector2 positionA = new Vector2(position.X, 
                position.Y + textures[3].Height + textures[2].Height + textures[1].Height);
            
            Vector2 positionB = new Vector2(position.X, 
                position.Y + textures[2].Height + textures[1].Height);
            
            Vector2 positionC = new Vector2(position.X, position.Y + textures[1].Height);

            spriteBatch.Draw(textures[0], positionA, Color.White);
            spriteBatch.Draw(textures[1], positionB, Color.White);
            spriteBatch.Draw(textures[2], positionC, Color.White);
            spriteBatch.Draw(textures[3], position, Color.White);            
            
            #if DRAW_COLLISIONS
            for (int i = 0; i < w.Collisions.Length; i++)
            {                
                Rectangle tr = new Rectangle(
                    w.Collisions[i].X,
                    w.Collisions[i].Y + (int)position.Y,
                    w.Collisions[i].Width,
                    w.Collisions[i].Height);
                
                spriteBatch.Draw(RiverRide.RectDebug, tr, Color.White);
            }
            foreach (SceneElement se in w.SceneElements)
            {
                Rectangle tr = new Rectangle(
                    se.RectPosition.X,
                    se.RectPosition.Y + (int)position.Y,
                    se.RectPosition.Width,
                    se.RectPosition.Height);
                spriteBatch.Draw(RiverRide.RectDebug, tr, Color.White);
            }
            #endif

            spriteBatch.End();
        }
    }
}

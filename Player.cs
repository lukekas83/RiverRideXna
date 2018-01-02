//#define DRAW_COLLISIONS

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RiverRideGame
{
    public class Player : SceneElement
    {
        private int MOVESPEED = 100;
        private KeyboardState keyboardState;
        private const float FIREDELAY = .5f;        
        private int X, Y;
        private bool speed = false;

        public Player(Texture2D[] texture, string id,World w) : base(texture,id,w)
        {            
            this.w = w;
            Y = 510;
            X = RiverRide.SCREEN_WIDTH / 2 - texture[0].Width;
            shadowColor = new Color(new Vector4(0f, 0f, 0f, 0.2f));
        }        

        private void Fire()
        {
            if (elapsed > FIREDELAY)
            {
                for (int i = 0; i < RiverRide.Bullets.Length; i++)
                {
                    if (!RiverRide.Bullets[i].IsActive)
                    {
                        Vector2 position = Vector2.Zero;
                        position.X = RectPosition.X +(texture[0].Width / 2) - RiverRide.Bullets[0].RectPosition.Width / 2;
                        position.Y = Y;
                        RiverRide.Bullets[i].Position = position;
                        RiverRide.Bullets[i].IsActive = true;
                        RiverRide.Bullets[i].Direction = Direction.BottomTop;
                        break;
                    }
                }

                elapsed = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (RiverRide.paused)
                return;

            w.MoveSpeed = 20;
            speed = false;

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                X += (int)(-MOVESPEED * gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                X += (int)(MOVESPEED * gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                w.MoveSpeed = 100;
                speed = true;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                w.MoveSpeed = 10;
            }

            if (w.EndOfScrolling)
            {
                Y -= (int)(MOVESPEED * gameTime.ElapsedGameTime.TotalSeconds);
                RectPosition = new Rectangle(X, Y, texture[0].Width, texture[0].Height);
                if (Math.Abs(Y) < 1)
                {
                    RiverRide.win = true;
                    RiverRide.paused = true;
                }
            }
            else
                RectPosition = new Rectangle(X, Y, texture[0].Width, texture[0].Height);


            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Fire();
            }
        }

        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive)
                return;

            spriteBatch.Begin();
            #if DRAW_COLLISIONS
            spriteBatch.Draw(RiverRide.RectDebug, RectPosition, Color.White);
            #endif
            if (!speed)
            {
                spriteBatch.Draw(texture[0], RectPosition, Color.White);
                spriteBatch.Draw(texture[0], RectPosition, shadowColor);
            }
            else
            {
                spriteBatch.Draw(texture[1], RectPosition, Color.White);
                spriteBatch.Draw(texture[1], RectPosition, shadowColor);

            }
            spriteBatch.End();
        }
    }
}

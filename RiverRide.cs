using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using RiverRideGame;

namespace RiverRideGame
{
    public class RiverRide : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int SCREEN_WIDTH = 600;
        public const int SCREEN_HEIGHT = 600;

        private ScrollingBackground ScrollingWorld;
        private Player player;
        
        public static Bullet[] Bullets = new Bullet[100];
        private static Explosion[] Explosions = new Explosion[60];

        private Random random = new Random();
        
        public static Texture2D ShipEnemyTexture1; //t1
        public static Texture2D ShipEnemyTexture2;
        public static Texture2D HelicopterEnemyTexture1; //t2
        public static Texture2D HelicopterEnemyTexture2; //t2
        public static Texture2D TankEnemyTexture1; //t3
        public static Texture2D TankEnemyTexture2; //t3
        public static Texture2D Su22EnemyTexture1; //t4
        public static Texture2D Su22EnemyTexture2; //t4
        public static Texture2D BulletTexture;
        public static Texture2D BulletTextureH;
        public static Texture2D Bridge;
        public static Texture2D FuelTexture;
        public static Texture2D explosionTexture;
        private SpriteFont font;
        private int fuel = 100;
        private int points = 0;

        public static bool GameOver = false;
        public static Texture2D RectDebug;
        public static bool paused = false;
        public static bool win = false;
        private bool gameover = false;
        private bool firstrun = true;

        World w = null;

        public RiverRide()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BulletTexture = Content.Load<Texture2D>("bullets");
            BulletTextureH = Content.Load<Texture2D>("bulletsh");
            explosionTexture = Content.Load<Texture2D>("explosion");            
            RectDebug = Content.Load<Texture2D>("rect");            
            ShipEnemyTexture1 = Content.Load<Texture2D>("enemyt1");
            ShipEnemyTexture2 = Content.Load<Texture2D>("enemyt12");

            HelicopterEnemyTexture1 = Content.Load<Texture2D>("enemyt2");
            HelicopterEnemyTexture2 = Content.Load<Texture2D>("enemyt22");

            TankEnemyTexture1 = Content.Load<Texture2D>("enemyt3");
            TankEnemyTexture2 = Content.Load<Texture2D>("enemyt32");

            Su22EnemyTexture1 = Content.Load<Texture2D>("enemyt4");
            Su22EnemyTexture2 = Content.Load<Texture2D>("enemyt42");

            Bridge = Content.Load<Texture2D>("Bridge");
            FuelTexture = Content.Load<Texture2D>("fuel");
            font = Content.Load<SpriteFont>("font");
            
            initNewGame();
        }

        private void initNewGame()
        {
            w = new Initailizer().initWorld("big.png.map");
            for (int i = 0; i < Bullets.Length; i++)            
                Bullets[i] = new Bullet(BulletTexture, "", w);            

            for (int i = 0; i < Explosions.Length; i++)            
                Explosions[i] = new Explosion(explosionTexture);

            Texture2D[] playerTextures = new Texture2D[] 
            {
                Content.Load<Texture2D>("playership"),
                Content.Load<Texture2D>("playershipfast")
            };
            player = new Player(playerTextures, "", w);
            Texture2D level1_a = Content.Load<Texture2D>("level1_a");
            Texture2D level1_b = Content.Load<Texture2D>("level1_b");
            Texture2D level1_c = Content.Load<Texture2D>("level1_c");
            Texture2D level1_d = Content.Load<Texture2D>("level1_d");
            
            ScrollingWorld = new ScrollingBackground(
                new Texture2D[] 
                {
                    level1_d,
                    level1_c,
                    level1_b,
                    level1_a,
                }, ref w);
        }

        protected override void UnloadContent()
        {            
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();
            Keys[] currentKeys = currentState.GetPressedKeys();

            foreach (Keys keys in currentKeys)
            {
                if (keys == Keys.Escape)
                    this.Exit();

                if (keys == Keys.F2 && !gameover)
                {
                    if (firstrun)
                    {
                        firstrun = false;
                        paused = false;
                    }
                    else
                    {
                        if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                            paused = !paused;
                    }
                }
                if (keys == Keys.F5)
                {
                    initNewGame();
                    fuel = 100;
                    points = 0;
                    paused = false;
                    gameover = false;
                    firstrun = true;
                    win = false;
                }
            }

            if (!paused && !gameover)
                ScrollingWorld.Update(gameTime);

            for (int i = 0; i < Explosions.Length; i++)
            {
                if (Explosions[i].IsActive)
                {
                    Explosions[i].Update(gameTime);
                }
            }

            foreach(SceneElement e in w.SceneElements)
                e.Update(gameTime);

            for (int i = 0; i < w.Collisions.Length; i++)
            {
                int wo = Math.Abs(w.offsetY);
                int cy = w.Collisions[i].Y;

                Rectangle tr = new Rectangle(w.Collisions[i].X, w.Collisions[i].Y + w.offsetY,
                    w.Collisions[i].Width, w.Collisions[i].Height);

                if (player.RectPosition.Intersects(tr))
                {
                    Explosions[0].Position = player.RectPosition;
                    Explosions[0].IsActive = true;
                    player.IsActive = false;
                    gameover = true;
                }
            }

            for (int i = 0; i < Bullets.Length; i++)
            {
                if (Bullets[i].IsActive)
                {
                    Bullets[i].Update(gameTime);
                }
            }

            for (int i = 0; i < w.SceneElements.Count; i++)
            {
                if (w.SceneElements[i].IsActive)
                {
                    Rectangle tr = new Rectangle(
                                w.SceneElements[i].RectPosition.X,
                                w.SceneElements[i].RectPosition.Y + w.offsetY,
                                w.SceneElements[i].Width,
                                w.SceneElements[i].Height);

                    if (w.SceneElements[i] is TankEnemy)
                    {
                        TankEnemy te = w.SceneElements[i] as TankEnemy;
                        for (int b = 0 ; b < te.Bullets.Length; b++)
                        {
                            if (te.Bullets[b].RectPosition.Intersects(player.RectPosition))
                            {
                                player.IsActive = false;
                                gameover = true;
                            }
                        }
                    }

                    if (tr.Intersects(player.RectPosition))
                    {
                        w.SceneElements[i].IsActive = false;
                        Explosions[0].IsActive = true;
                        Explosions[0].Position = tr;
                        Explosions[1].IsActive = true;
                        Explosions[1].Position = player.RectPosition;
                        player.IsActive = false;
                        gameover = true;
                    }

                    for (int j = 0; j < Bullets.Length; j++)
                    {
                        if (Bullets[j].IsActive)
                        {
                            if (tr.Intersects(Bullets[j].RectPosition))
                            {
                                points += 10;
                                for (int k = 0; k < Explosions.Length; k++)
                                {
                                    if (!Explosions[k].IsActive)
                                    {
                                        Explosions[k].IsActive = true;
                                        Explosions[k].Position = tr;
                                        w.SceneElements[i].IsActive = false;
                                        Bullets[j].IsActive = false;
                                        break;
                                    }
                                }
                            }                           
                        }
                    }                    
                }               
            }
            for (int f = 0; f < w.Fuels.Length; f++)
            {
                Rectangle tr = new Rectangle(
                    w.Fuels[f].X,
                    w.Fuels[f].Y + w.offsetY,
                    RiverRide.FuelTexture.Width,
                    RiverRide.FuelTexture.Height);

                for (int b = 0; b < Bullets.Length; b++)
                {
                    if (Bullets[b].IsActive)
                    {
                        if (tr.Intersects(Bullets[b].RectPosition))
                        {
                            for (int k = 0; k < w.Fuels.Length; k++)
                            {
                                if (w.Fuels[k] != new Rectangle(1,1,1,1))
                                {
                                    for (int e = 0 ; e < Explosions.Length ; e++)
                                    {
                                        if (!Explosions[e].IsActive)
                                        {
                                            Explosions[e].IsActive = true;
                                            Explosions[e].Position = tr;                                            
                                            break;
                                        }
                                    }

                                    w.Fuels[k] = new Rectangle(1, 1, 1, 1);
                                    Bullets[b].IsActive = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (w.Fuels[f] != new Rectangle(1, 1, 1, 1))
                {
                    if (tr.Intersects(player.RectPosition))
                    {
                        if (gameTime.TotalGameTime.Milliseconds % 100 == 0)
                        {
                            if (fuel + 1 <= 100)
                                ++fuel; ;
                        }                        
                    }
                }
            }

            if (gameTime.TotalGameTime.Milliseconds % 200 == 0)
            {
                if (fuel - 1 >= 0)
                {
                    if (!gameover && !paused && !firstrun)
                        fuel--;
                }
                else
                {
                    Explosions[0].IsActive = true;
                    player.IsActive = false;
                    gameover = true;
                    Explosions[0].Position = player.RectPosition;
                    w.EndOfScrolling = true;
                }
            }

            if (gameover)
                return;

            player.Update(gameTime);
            
            if (firstrun)
                paused = true;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(new Color(95, 79, 223));            
            ScrollingWorld.Draw(spriteBatch);
            for (int b = 0 ; b < w.Bridges.Length; b++)
            {
                spriteBatch.Begin();
                if (w.Bridges[b] != new Rectangle(1, 1, 1, 1))
                {
                    Rectangle tr = new Rectangle(
                        w.Bridges[b].X,
                        w.Bridges[b].Y + w.offsetY,
                        w.Bridges[b].Width,
                        w.Bridges[b].Height);
                    spriteBatch.Draw(Bridge, tr, Color.White);
                }
                spriteBatch.End();
            }

            for (int f = 0; f < w.Fuels.Length; f++)
            {
                spriteBatch.Begin();
                if (w.Fuels[f] != new Rectangle(1, 1, 1, 1))
                {
                    Rectangle tr = new Rectangle(
                        w.Fuels[f].X,
                        w.Fuels[f].Y + w.offsetY,
                        w.Fuels[f].Width,
                        w.Fuels[f].Height);
                    spriteBatch.Draw(FuelTexture, tr, Color.White);
                }
                spriteBatch.End();

            }

            foreach (SceneElement e in w.SceneElements)
                e.Draw(spriteBatch);

            for (int i = 0; i < Bullets.Length; i++)
            {
                if (Bullets[i].IsActive)
                {
                    Bullets[i].Draw(spriteBatch);
                }
            }
            for (int i = 0; i < Explosions.Length; i++)
            {
                if (Explosions[i].IsActive)
                {
                    Explosions[i].Draw(spriteBatch);
                }
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "fuel " +fuel.ToString() + " %", new Vector2(30.0f, 560.0f), fuel < 30 ? Color.Red : Color.OrangeRed);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "score " + points.ToString() , new Vector2(480.0f, 560.0f), Color.OrangeRed);
            spriteBatch.End();
            player.Draw(spriteBatch);
            if (paused)
            {
                spriteBatch.Begin();
                if (win)
                    spriteBatch.DrawString(font, "  The End of River Ride. \n        (F5 - new game)", new Vector2(210, 270), Color.White);
                else
                    spriteBatch.DrawString(font, firstrun ? " River Ride XNA\n\n(Press F2 to start)" : "game paused (F2)", new Vector2(220, 270), Color.White);
                spriteBatch.End();
            }
            if (gameover)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, " Game Over !!\n(F5 - new game)", new Vector2(260, 270), fuel < 30 ? Color.Red : Color.OrangeRed);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}

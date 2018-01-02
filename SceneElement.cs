using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RiverRideGame
{
    public abstract class SceneElement
    {
        protected Random random = new Random();
        private Direction direction = Direction.LeftToRight;
        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        protected Texture2D[] texture;
        public int Width
        {
            get
            {
                return texture[0].Width;
            }
        }
        public int Height
        {
            get
            {
                return texture[0].Height;
            }
        }

        protected Color shadowColor;
        protected float elapsed = 0f;
        public Rectangle RectPosition;
        public Rectangle RectStart;
        public Rectangle RectEnd = new Rectangle(1, 1, 1, 1);

        protected bool isActive = true;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private float moveSpeed = 10f;
        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        public string id;
        protected World w = null;
        public SceneElement(Texture2D[] texture, string id, World w)
        {
            this.texture = texture;
            this.id = id;
            this.w = w;
            shadowColor = new Color(new Vector4(0f, 0f, 0f, 0.2f));
        }        
        
        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive)
                return;

            spriteBatch.Begin();

            Rectangle tr = new Rectangle(
                    RectPosition.X,
                    RectPosition.Y + w.offsetY,
                    texture[0].Width,
                    texture[0].Height);

            if(direction == Direction.LeftToRight)
                spriteBatch.Draw(texture[0], tr, Color.White);
            else
                spriteBatch.Draw(texture[1], tr, Color.White);
            spriteBatch.End();
        }
    }

    public enum Direction
    {
        LeftToRight,
        RightToLeft,
        BottomTop
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace LightGame
{
    class Player : GameObject
    {

        //created as static because of Garbage and EMField requiring movment relative to Player Velocity
        private static new Vector2 velocity;

        private Texture2D[] animation;
        private int currFrame = 0;

        public static new Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        private float initialRadius;

        /*
        Function: Player 
         */
        public Player(Texture2D[] text, Vector2 pos, float ma,  float rad, Vector2 vel)
            : base(text[0], pos, ma, rad)
        {
            animation = text;
            velocity = vel;
            initialRadius = Radius;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        public void SetPosition(Vector2 pos)
        {
            Move(pos - position);
        }

        /// <summary>
        /// Moves the player and its bounding box
        /// </summary>
        /// <param name="vel">The direction and magnitude to move the player</param>
        protected void Move(Vector2 vel)
        {
            
            Position += vel;
            
            // moves the bounding box
            bounds.X = (int)(position.X - Radius);
            bounds.Y = (int)(position.Y - Radius); 
        }

        private void Bound()
        {
            Rectangle screenBounds = Game1.bounds;
            // check right and left bounds of the screen, and reset to bounds if past
            if (Bounds.Left < screenBounds.Left)
                position.X = screenBounds.Left + Radius;
            else if (Bounds.Right > screenBounds.Right)
                position.X = screenBounds.Right - Radius;

            // check top and bottom bounds of the screen, and reset to bounds if past
            if (Bounds.Top < screenBounds.Top)
                position.Y = screenBounds.Top + Radius;
            else if (Bounds.Bottom >= screenBounds.Bottom)
                position.Y = screenBounds.Bottom - Radius;

            bounds.X = (int)(position.X - Radius);
            bounds.Y = (int)(position.Y - Radius);
        }

        /// <summary>
        /// Process the current keyboard state.
        /// </summary>
        public void GetInput()
        {
            if (Game1.keyboardInput)
            {
                KeyboardState kState = Game1.keyState;


                if (kState.IsKeyDown(Keys.Down))
                    Move(new Vector2(0, 10));

                if (kState.IsKeyDown(Keys.Up))
                    Move(new Vector2(0, -10));
                if (kState.IsKeyDown(Keys.Left))
                    Move(new Vector2(-10, 0));

                if (kState.IsKeyDown(Keys.Right))
                    Move(new Vector2(10, 0));
            }
            else
            {
                /// add in when you want mouse input
                MouseState mState = Game1.mState;
                Vector2 distToMouse = new Vector2(mState.X, mState.Y) - position;
                if (distToMouse.Length() >= initialRadius/2)
                {
                    distToMouse.Normalize();
                    Move(distToMouse * 10);
                }
            }
        }

        /*private bool checkPointInBound(Vector2 vector2)
        {
            if (vector2.X < 0 || vector2.X > Game1.bounds.Width)
                return false;
            if (vector2.Y > Game1.bounds.Height || vector2.Y < 0)
                return false;
            return true;
        }*/

        /// <summary>
        /// Updates the player
        /// </summary>
        /// <param name="dt">Amount of time since the last update</param>
        public override void Update(GameTime dt)
        {

            currFrame = currFrame >= animation.Length-1 ? 0 : currFrame + 1;
            GetInput();
            Bound();
            base.Update(dt);
        }

        /// <summary>
        /// Draws the player to the screen
        /// </summary>
        /// <param name="sBatch">An object of the spritebatch</param>
        /// <param name="dt">Amount of time since the last update</param>
        public override void Draw(SpriteBatch sBatch, GameTime dt)
        {
            sBatch.Draw(animation[currFrame], new Vector2(position.X - Radius, position.Y - Radius), null, Color.White, 0.0f, new Vector2(), Game1.scale, SpriteEffects.None, 0);
           // base.Draw(sBatch, dt);
        }
    }
}

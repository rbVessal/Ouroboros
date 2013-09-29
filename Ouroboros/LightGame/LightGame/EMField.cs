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
    class EMField : GameObject
    {

        /// <summary>
        /// Constructor for the accelerator Electro Magnetic Fields
        /// </summary>
        /// <param name="text">Texture2D</param>
        /// <param name="pos">position</param>
        /// <param name="ma">mass</param>
        /// <param name="rad">radius</param>
        public EMField(Texture2D text, Vector2 pos, float ma, float rad) : base (text, pos , ma, rad)
        {
            velocity = new Vector2(-10.0f, 0.0f);   
        }

        /// <summary>
        /// Overriden Update for the EMField
        /// 
        /// should move relative to the player's speed
        /// </summary>
        /// <param name="dt">Game Time!</param>
        public override void Update(GameTime dt)
        {
            float multiplier = (Player.Velocity.Length() > 10.0f) ? (float)((Player.Velocity.Length() / 10.0f) / 5.0f) : 1.0f;
            Position += velocity * multiplier;
            if (Position.X < -Radius)
                //Position = new Vector2(StartingPosition.X, StartingPosition.Y);
                Dead = true;
            base.Update(dt);
        }

        /// <summary>
        /// Overridden Draw for the EMField
        /// 
        /// should be handled for you in the base class
        /// </summary>
        /// <param name="sBatch">spriteBatch</param>
        /// <param name="dt">GAMETIME</param>
        public override void Draw(SpriteBatch sBatch, GameTime dt)
        {
            base.Draw(sBatch, dt);
        }

    }
}

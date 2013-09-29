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

   
    /*types of garbage we can collide with*/
    enum GarbageType { CIGARETTEBUTTS, PLASTIC, METAL, STYROPHOME, DEFAULT }

    class Garbage : GameObject
    {

        /// <summary>
        /// Type of garbage that this may be...
        /// </summary>
        GarbageType type;
     

        /// <summary>
        /// Garbage Constructor
        /// </summary>
        /// <param name="text">texture2D of the garbage</param>
        /// <param name="pos">position of the garbage</param>
        /// <param name="ma">mass of the garbage</param>
        /// <param name="rad">radius of the garbage</param>
        /// <param name="t">type of garbage</param>
        public Garbage(Texture2D text, Vector2 pos, float ma, float rad, GarbageType t)
            : base(text, pos, ma, rad)
        {
            type = t;
            velocity = new Vector2(-10.0f, 0.0f);
        }

        /// <summary>
        /// Overriden Update for garbage...
        /// should handle movement relative to the player
        /// </summary>
        /// <param name="dt">Current Game Time</param>
        public override void Update(GameTime dt)
        {

            //Position += Player.Velocity.Length() > 5 ? velocity * 2 : velocity;
            float multiplier = (Player.Velocity.Length() > 10.0f) ? (float)((Player.Velocity.Length() / 10.0f) / 5.0f) : 1.0f;
            Position += velocity * multiplier;
            // reset the position.
            if (Position.X < -Radius)
                //Position = new Vector2(StartingPosition.X, StartingPosition.Y);
                Dead = true;
            
            base.Update(dt);
        }

       /// <summary>
       /// Overridden Draw for garbage...
       /// 
       /// should be handled for you... FOR NOW
       /// (see base class)
       /// </summary>
       /// <param name="sBatch">spriteBatch</param>
       /// <param name="dt">Current Game Time</param>
        public override void Draw(SpriteBatch sBatch, GameTime dt)
        {
            base.Draw(sBatch, dt);
        }

    }
}

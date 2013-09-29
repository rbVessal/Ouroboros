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
    public class GameObject
    {
        /// <summary>
        /// 
        /// </summary>
        private Texture2D sprite;
        
        /// <summary>
        /// 
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 startingPosition; //use this to refer to when reseting positions

        protected Vector2 velocity;

        /// <summary>
        /// 
        /// </summary>
        protected Rectangle bounds;

        /// <summary>
        /// 
        /// </summary>
        private float mass;

        /// <summary>
        /// 
        /// </summary>
        private float radius;

        /// <summary>
        /// 
        /// </summary>
        private bool dead;



        #region properties

        /// <summary>
        /// 
        /// </summary>
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 StartingPosition
        {
            get { return startingPosition; }
        }


        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Rectangle Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        #endregion properties

        /// <summary>
        /// General Game Object
        /// </summary>
        /// <param name="sp">Sprite</param>
        /// <param name="pos">Position</param>
        /// <param name="ma">mass</param>
        /// <param name="rad">radius</param>
        public GameObject(Texture2D sp, Vector2 pos, float ma, float rad)
        {
            sprite = sp;
            startingPosition = pos;
            position = pos;
            mass = ma;
            radius = rad;
            bounds = new Rectangle((int)(pos.X - radius), (int)(pos.Y - radius), (int)(radius * 2), (int)(radius * 2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(GameTime dt)
        {
            //we could make this abstract... maybe...
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbatch"></param>
        /// <param name="dt"></param>
        public virtual void Draw(SpriteBatch sbatch, GameTime dt)
        {
            //sbatch.Draw(sprite, new Rectangle((int)(position.X-radius), (int)(position.Y-radius), sprite.Width, sprite.Height), Color.White);
            sbatch.Draw(sprite, new Vector2(position.X - radius, position.Y - radius), Color.White);
        }

        /// <summary>
        /// Used to find the center of gameObjects for collision purposes
        /// </summary>
        /// <returns>the center of this object</returns>
        /*public Vector2 Center()
        {
            return new Vector2(Position.X + Sprite.Width / 2, Position.Y + Sprite.Height / 2);
        }*/
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpeechGame.GameActors
{
    using Camera;
    using Level;

    class Particle : GameObject
    {

        #region Declarations

        private Vector2 acceleration;
        private float maxSpeed;
        private int initialDuration;
        private int remainingDuration;

        #endregion

        #region Properties
        
        public int ElapsedDuration
        {
            get
            {
                return initialDuration - remainingDuration;
            }
        }

        public float DurationProgress
        {
            get
            {
                return (float)ElapsedDuration / (float)initialDuration;
            }
        }

        public bool IsActive
        {
            get
            {
                return (remainingDuration > 0);
            }
        }
        #endregion

        #region Constructor

        public Particle(Vector2 location, Texture2D texture, Rectangle initialFrame,bool destructive,
            Vector2 velocity, Vector2 acceleration, float maxSpeed, int duration)
        {
            animations.Add("default", new AnimationStrip(texture, initialFrame.Width, "default"));
            animations["default"].LoopAnimation = true;

            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
            CollisionRectangle = initialFrame;
            drawDepth = 0.8f;

            enabled = true;
            this.destructive = destructive;
            PlayAnimation("default");

            tileMap = TileMap.GetInstance();
            Camera = CameraManager.GetInstance();
            worldLocation = location;
            initialDuration = duration;
            remainingDuration = duration;
            this.acceleration = acceleration;
            this.maxSpeed = maxSpeed;

        }
        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            if (remainingDuration <= 0)
            {
                Enabled = true;
            }
            else
            {
                Enabled = false;
            }

            if (!Enabled)
            {
                Velocity += acceleration;
                if (Velocity.Length() > maxSpeed)
                {
                    Vector2 vel = Velocity;
                    vel.Normalize();
                    Velocity = vel * maxSpeed;
                }
            }
            remainingDuration--;

            base.Update(gameTime);

         }

        public override void Draw(SpriteBatch spriteBatch)
        {
                base.Draw(spriteBatch);
        }

        #endregion

    }
}
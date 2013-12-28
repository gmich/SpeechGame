using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace SpeechGame.GameActors
{
    using Level;
    using Camera;
    using Scene;

    public class Player : GameObject
    {
        private Vector2 fallSpeed = new Vector2(0, 20);
        private Vector2 velocityVector = Vector2.Zero;
        private bool dead = false;
        private bool onAir = false;
        private int gust;
        private int gustBullets;
        private int livesRemaining = 999;
        private float timeSinceLastShot = 0.0f;
        #region Velocity Handler Declarations

        private Vector2 friction = Vector2.Zero;
        private Vector2 groundFriction = new Vector2(30f, 0);
        private Vector2 airFriction = new Vector2(20f, 0);
        private Vector2 accelerationAmount = new Vector2(40f, 0);
        private Vector2 sprintAccelerationAmount = new Vector2(40f, 0);
        private Vector2 currentMaxVelocity = Vector2.Zero;
        private Vector2 sprintMaxVelocity = new Vector2(250f, 0);

        bool goRight;
        bool goLeft;
        EffectsManager EffectsManager;

        #endregion

        public bool Dead
        {
            get { return dead; }
        }

        public float Friction
        {
            get { return friction.X; }
        }

        public bool Flipped
        {
            get { return flipped; }
        }
        public bool IsJumping
        {
            get { return onAir; }
        }

        public float VelocityY
        {
            get { return velocity.Y; }
        }

        public int LivesRemaining
        {
            get { return livesRemaining; }
            set { livesRemaining = value; }
        }

        #region Constructor

        public Player(ContentManager content)
        {
            animations.Add("idle",
                new AnimationStrip(content.Load<Texture2D>(@"Textures\Sprites\Player\Idle"), 50, "idle"));
            animations["idle"].LoopAnimation = true;

            animations.Add("run", new AnimationStrip(content.Load<Texture2D>(@"Textures\Sprites\Player\Run"), 50, "run"));
            animations["run"].LoopAnimation = true;

            animations.Add("jump", new AnimationStrip(content.Load<Texture2D>(@"Textures\Sprites\Player\Jump"), 50, "jump"));
            animations["jump"].LoopAnimation = false;
            animations["jump"].FrameLength = 0.08f;

            animations.Add("falling", new AnimationStrip(content.Load<Texture2D>(@"Textures\Sprites\Player\Falling"), 50, "falling"));
            animations["falling"].LoopAnimation = true;

            animations.Add("die", new AnimationStrip(content.Load<Texture2D>(@"Textures\Sprites\Player\Die"), 50, "die"));
            animations["die"].LoopAnimation = false;

            frameWidth = 48;
            frameHeight = 48;

            gustBullets = 2;
            gust = gustBullets;
            CollisionRectangle = new Rectangle(5, 3, 35, 45);
            drawDepth = 0.825f;

            enabled = true;
            PlayAnimation("idle");

            tileMap = TileMap.GetInstance();
            Camera = CameraManager.GetInstance();
            livesRemaining = 999;
            worldLocation = new Vector2(1500, 10);
            EffectsManager = EffectsManager.GetInstance();
        }

        #endregion

        #region Public Methods

        public override void Update(GameTime gameTime)
        {
            if (!Dead)
            {
                string newAnimation = "idle";

                if (onGround && currentAnimation == "jump")
                {
                    onAir = false;
                    currentAnimation = "idle";
                }

                if (onGround)
                {
                    onAir = false;
                }

                if (currentAnimation == "jump")
                    newAnimation = "jump";

                if (SpeechRecognition.Right() || Input.InputManager.IsKeyReleased(Keys.D))
                {
                    EffectsManager.AddFloatingText("right");
                    goRight = true;
                    goLeft=false;
                }
                if (goRight)
                {
                    flipped = true;
                    newAnimation = "run";
                    velocity += accelerationAmount;
                }
                if (SpeechRecognition.Left() || Input.InputManager.IsKeyReleased(Keys.A))
                {
                    EffectsManager.AddFloatingText("left");
                    goRight = false;
                    goLeft = true;

                }
                if (goLeft)
                {
                    flipped = false;
                    newAnimation = "run";
                    velocity -= accelerationAmount;
                }
                if (SpeechRecognition.Stop() || Input.InputManager.IsKeyReleased(Keys.S))
                {
                    EffectsManager.AddFloatingText("stop");
                    goRight = false;
                    goLeft = false;
                }
                if (SpeechRecognition.Jump() || Input.InputManager.IsKeyReleased(Keys.W))
                {
                    if (onGround)
                    {
                        EffectsManager.AddFloatingText("jump");
                        Jump();
                    }
                }
                if (SpeechRecognition.Gust() ||  Input.InputManager.IsKeyReleased(Keys.E))
                {
                    EffectsManager.AddFloatingText("gust");
                    gust = 0;
                }
                if ((SpeechRecognition.Shoot() || Input.InputManager.IsKeyReleased(Keys.R) || gust<gustBullets) && timeSinceLastShot>0.05f)
                {
                    gust = (int)MathHelper.Min(gust + 1, gustBullets+1);
                    timeSinceLastShot = 0.0f;

                    if (gust > gustBullets)
                        EffectsManager.AddFloatingText("shoot");
                    
                    Vector2 bulletDirection = new Vector2(-1, 0);
                    Vector2 offSet = new Vector2(frameWidth/2, 5);
                    if (Flipped)
                    {
                        offSet = new Vector2(frameWidth/2, 5);
                        bulletDirection = new Vector2(1, 0);
                    }
                    EffectsManager.AddBulletParticle(worldLocation + offSet, bulletDirection);
                }
                
                if (velocity.Y > 0)
                {
                    newAnimation = "falling";
                }
                else if (velocity.Y < 0)
                {
                    newAnimation = "jump";
                }

                if (newAnimation != currentAnimation)
                {
                    PlayAnimation(newAnimation);
                }
            }

            velocity += fallSpeed;

            HandleVelocity();
            base.Update(gameTime);

            timeSinceLastShot = MathHelper.Min(timeSinceLastShot + (float)gameTime.ElapsedGameTime.TotalSeconds, 10.0f);
            Camera.Move((float)gameTime.ElapsedGameTime.TotalSeconds, WorldLocation, velocity, accelerationAmount.X);
        }

        public void HandleVelocity()
        {
            currentMaxVelocity = sprintMaxVelocity;
            accelerationAmount = sprintAccelerationAmount;

                if (onGround)
                {
                    friction = groundFriction;
                }
                else if (onAir)
                    friction = airFriction;

                if (!flipped)
                {
                    if (velocity.X > 0)
                        velocity.X = MathHelper.Clamp(Velocity.X - friction.X / 2,
                            -currentMaxVelocity.X, +currentMaxVelocity.X);
                    else
                        velocity.X = MathHelper.Clamp(Velocity.X + friction.X, -currentMaxVelocity.X, 0);
                }
                else if (flipped)
                {
                    if (velocity.X < 0)
                        velocity.X = MathHelper.Clamp(Velocity.X + friction.X / 2,
                            -currentMaxVelocity.X, +currentMaxVelocity.X);
                    else
                        velocity.X = MathHelper.Clamp(Velocity.X - friction.X, 0, +currentMaxVelocity.X);
                }
            
        }

        public void Jump()
        {
            velocity.Y = -500;
            onAir = true;
        }

        public void Kill()
        {
            PlayAnimation("die");
            LivesRemaining--;
            velocity.X = 0;
            dead = true;
        }

        public void Revive()
        {
            PlayAnimation("idle");
            dead = false;
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);  
        }
    }
}

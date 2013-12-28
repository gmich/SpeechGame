using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpeechGame.GameActors
{
    using Camera;
    using Level;

    class Block : GameObject
    {
        #region Properties

        #endregion

        #region Constructor

        public Block(Vector2 location, Texture2D texture, Rectangle initialFrame)
        {
            animations.Add("default", new AnimationStrip(texture, initialFrame.Width, "default"));
            animations["default"].LoopAnimation = true;

            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
            CollisionRectangle = initialFrame;
            drawDepth = 0.8f;

            enabled = true;
            collidable = false;
            PlayAnimation("default");

            tileMap = TileMap.GetInstance();
            Camera = CameraManager.GetInstance();
            worldLocation = location;

        }

        #endregion

        #region Collision

        public bool Collides(Vector2 other, bool toDestruct)
        {
            bool collides = ((worldLocation.X <= other.X && worldLocation.X + collisionRectangle.Width >= other.X)
                    && (worldLocation.Y <= other.Y && worldLocation.Y + collisionRectangle.Height >= other.Y));
            
            if (toDestruct && collides)
                enabled = false;
            
            return collides;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            velocity += new Vector2(0, 10);
            
            base.Update(gameTime);
        }
        #endregion
    }
}
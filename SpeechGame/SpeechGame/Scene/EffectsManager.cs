using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpeechGame.Scene
{
    using GameActors;
    using Animations;

    public class EffectsManager
    {
        #region Declarations

        List<GameObject> Effects;
        List<Block> Blocks;
        List<FloatingText> FloatingText;
        Texture2D bullet;
        Texture2D waterDrop;
        Texture2D block;
        static SpriteFont Font;
        static Random rand;

        #endregion

        #region Instance

        static EffectsManager instance;

        public static EffectsManager GetInstance()
        {
            if (instance == null)
            {
                instance = new EffectsManager();
            }
            return instance;
        }

        private EffectsManager()
        {
            rand = new Random();
            Effects = new List<GameObject>();
            FloatingText = new List<FloatingText>();
            Blocks = new List<Block>();
        }

        #endregion

        #region Initialization

        public void Initialize(ContentManager Content)
        {
            bullet = Content.Load<Texture2D>(@"Textures\Bullet");
            waterDrop = Content.Load<Texture2D>(@"Textures\waterDrop");
            block = Content.Load<Texture2D>(@"Textures\Block");
            Font = Content.Load<SpriteFont>(@"Fonts\font1");
        }

        #endregion

        #region Add Effects Methods

        public void AddBulletParticle(Vector2 location, Vector2 velocity)
        {
            Particle particle = new Particle(location, bullet, new Rectangle(0, 0, 10, bullet.Height), true, velocity*100,new Vector2(velocity.X*150,5), 9999f, 100);
            Effects.Add(particle);
        }

        public void AddWaterDrop(Vector2 location)
        {
            Particle particle = new Particle(new Vector2(location.X,0) - randomHorizontalDirection(), waterDrop, new Rectangle(0, 0, 2,2),false, new Vector2(0,1), new Vector2(0, 10), 9999f, 100);
            Effects.Add(particle);
        }

        public void AddFloatingText(string Text)
        {
            FloatingText.Add(new FloatingText(Font, Text, 1000));
        }

        public void AddBlock(Vector2 location,Rectangle blockRectangle)
        {
            Blocks.Add(new Block(location, block, blockRectangle));
        }

        public void AddSmashedBlockParticles(Vector2 location,Rectangle rect)
        {
            int particleSize = 2;
            for(int x=(int)location.X;x<location.X+rect.Width;x+=particleSize)
                for (int y = (int)location.Y; y < location.Y + rect.Height; y += particleSize)
                {
                    Particle particle = new Particle(new Vector2(x, y), block, new Rectangle(0, 0, particleSize, particleSize), false, new Vector2(0, 1), new Vector2(0, rand.Next(10, 40)), 9999f, 30);
                    Effects.Add(particle);
                }
        }

        #endregion

        #region Helper Methods

        Vector2 randomHorizontalDirection()
        {
            return new Vector2(rand.Next(0, 2000) - 1000, 0);
        }

        Vector2 randomDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(rand.Next(0, 100) - 50, rand.Next(0, 100) - 50);
            }
            while (direction.Length() == 0);
            direction.Normalize();
            direction *= scale;

            return direction;
        }

        Vector2 randomNegativeDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(rand.Next(0, 20) - 10, rand.Next(45, 60));
            }
            while (direction.Length() == 0);
            direction.Normalize();
            direction *= scale;

            return direction;
        }

        public bool LocationIsBlocked(Vector2 location,bool destructive)
        {
            foreach(Block block in Blocks)
                if (block.Collides(location, destructive))
                    return true;

            return false;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {          
            for (int x = Effects.Count - 1; x >= 0; x--)
            {
                Effects[x].Update(gameTime);

                if (Effects[x].Enabled)
                {
                    Effects.RemoveAt(x);
                }
            }

            for (int x = Blocks.Count - 1; x >= 0; x--)
            {
                Blocks[x].Update(gameTime);               
                if (!Blocks[x].Enabled)
                {
                    AddSmashedBlockParticles(Blocks[x].WorldLocation, Blocks[x].CollisionRectangle);
                    Blocks.RemoveAt(x);
                }
            }

            for (int x = FloatingText.Count - 1; x >= 0; x--)
            {
                FloatingText[x].Update(gameTime);
                if (!FloatingText[x].IsActive)
                {
                    FloatingText.RemoveAt(x);
                }
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject effect in Effects)
            {
                effect.Draw(spriteBatch);
            }
            foreach (Block block in Blocks)
            {
                block.Draw(spriteBatch);
            }
            foreach (FloatingText floatingText in FloatingText)
            {
                floatingText.Draw(spriteBatch);
            }
        }

        #endregion
              
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpeechGame.Scene
{
    using GameActors;

    public class EffectMask
    {
        RenderTarget2D effectMask;
        RenderTarget2D scene;
        Effect effect;
        Texture2D texture;
        GraphicsDevice graphicsDevice;
        float timePassed;
        static List<LightSource> lightSources;

        public EffectMask(GraphicsDevice graphicsDevice,Texture2D texture,  Effect effect)
        {
            this.graphicsDevice = graphicsDevice;
            this.texture = texture;
            this.effect = effect;
            timePassed = 10.0f;
            var pp = graphicsDevice.PresentationParameters;
            effectMask = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            scene = new RenderTarget2D(graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            lightSources = new List<LightSource>();
        }

        public static void AddLightSource(GameObject source, int size, Color color)
        {
            lightSources.Add(new LightSource(source, size, color));
        }

        public void Update(GameTime gameTime)
        {
            timePassed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timePassed > 0.2f)
            {
                Random rand = new Random();
                foreach (LightSource source in lightSources)
                {
                    source.OffSet = new Vector2(source.Size / 2 + rand.Next(-1, 1), source.Size / 2 + rand.Next(-1, 1));
                }
                timePassed = 0.0f;
            }
        }

        #region Draw

        public void DrawEffect(SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(effectMask);
            graphicsDevice.Clear(new Color(190,190,190));

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            foreach (LightSource source in lightSources)
            {
                spriteBatch.Draw(texture, new Rectangle((int)source.MapLocation.X - (int)source.OffSet.X, (int)source.MapLocation.Y - (int)source.OffSet.Y, source.Size, source.Size), source.Color);
            }
                spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }
        
        public void DrawScene(SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(scene);
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);        
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);

            graphicsDevice.Clear(Color.Blue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect.Parameters["lightMask"].SetValue(effectMask);
            effect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(scene, new Vector2(0, 0),Color.White);
            spriteBatch.End();
        }

        #endregion
    }
}

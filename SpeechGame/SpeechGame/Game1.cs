#define USE_SPEECH_INPUT

#region Using Declarations

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#endregion

namespace SpeechGame
{
    using Level;
    using GameActors;
    using Scene;
    using Diagnostics;

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpeechRecognition speechRecognition;
        EffectsManager effectsManager;
        TileMap tileMap;
        GameObject player;
        EffectMask lightMask;
        FpsMonitor fpsMonitor;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true,
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 500
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Input.InputManager.Initialize();

            #if USE_SPEECH_INPUT
                speechRecognition = new SpeechRecognition();
            #endif
            fpsMonitor = new FpsMonitor();
            tileMap = TileMap.GetInstance();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            IsMouseVisible = true;

            effectsManager = EffectsManager.GetInstance();
            effectsManager.Initialize(Content);
            lightMask = new EffectMask(GraphicsDevice, Content.Load<Texture2D>(@"Effects\lightSource"), Content.Load<Effect>(@"Effects\effect"));
            tileMap.Initialize(Content.Load<Texture2D>(@"Textures\PlatformTiles"), 48, 48);
            tileMap.Randomize(1000,50);
            player = new Player(Content);
            EffectMask.AddLightSource(player,750,Color.White);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Input.InputManager.Update(gameTime);
            player.Update(gameTime);
            lightMask.Update(gameTime);
            effectsManager.Update(gameTime);
            fpsMonitor.Update(gameTime);
            this.Window.Title = fpsMonitor.FPS.ToString();

            for (int i = 0; i < 15; i++)
                 effectsManager.AddWaterDrop(player.WorldLocation);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            lightMask.DrawEffect(spriteBatch);
            lightMask.DrawScene(spriteBatch);

            tileMap.Draw(spriteBatch);
            player.Draw(spriteBatch);
            effectsManager.Draw(spriteBatch);

            lightMask.Draw(spriteBatch);

            base.Draw(gameTime);

            fpsMonitor.AddFrame();
        }
    }
}

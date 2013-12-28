using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace SpeechGame.Diagnostics
{
    public class FpsMonitor
    {
        public float FPS { get; private set; }
        public TimeSpan Sample { get; set; }
        private Stopwatch sw;
        private int Frames;

        public FpsMonitor()
        {
            this.Sample = TimeSpan.FromSeconds(1);
            this.FPS = 0;
            this.Frames = 0;
            this.sw = Stopwatch.StartNew();
        }

        public void Update(GameTime gameTime)
        {
            if (sw.Elapsed > Sample)
            {
                this.FPS = (float)(Frames / sw.Elapsed.TotalSeconds);
                this.sw.Reset();
                this.sw.Start();
                this.Frames = 0;
            }
        }

        public void AddFrame()
        {
            Frames++;
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpeechGame.Animations
{
    public class FloatingText
    {
        #region Properties

        SpriteFont Font
        {
            get;
            set;
        }

        Vector2 Location
        {
            get;
            set;
        }

        string Text
        {
            get;
            set;
        }

        Vector2 TextSize
        {
            get
            {
                return Font.MeasureString(Text);
            }
        }

        Vector2 Velocity
        {
            get
            {
                return new Vector2(1,0);
            }
        }

        float ViewPortWidth
        {
            get;
            set;
        }

        float Step
        {
            get
            {
                if (Location.X < (ViewPortWidth / 2) - TextSize.X/2 )
                    return Vector2.Distance(Location, new Vector2((float)(ViewPortWidth / 2), Location.Y));
                else
                    return Vector2.Distance(Location, new Vector2((float)(ViewPortWidth / 2) - TextSize.X, Location.Y));
            }
        }

        public bool IsActive
        {
            get;
            private set;
        }

        #endregion
        
        #region Public Constructor

        public FloatingText(SpriteFont font,string text,float viewPortwidth)
        {
            this.Font = font;
            this.Text = text;
            Location = new Vector2(-TextSize.X, 180);
            IsActive = true;
            this.ViewPortWidth = viewPortwidth;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            Location += Velocity * Step* gameTime.ElapsedGameTime.Milliseconds / 400;
            if (Location.X >= ViewPortWidth)
                IsActive = false;
        }
      
        #endregion

        #region Draw
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Location, Color.Black);
        }

        #endregion
    }
}
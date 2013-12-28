using System;
using Microsoft.Xna.Framework;

namespace SpeechGame.Camera
{
    public interface ICameraScript
    {
        Camera Camera
        {
            get;
        }

        void Logic(float timePassed, Vector2 otherLocation,Vector2 otherVelocity,float step);

    }
}

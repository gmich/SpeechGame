﻿using System;
using Microsoft.Xna.Framework;

namespace SpeechGame.Camera
{
    public class CameraManager
    {
        private static CameraManager managerInstance;
        ICameraScript cameraScript;

        CameraManager()
        {
            cameraScript = new ChasingCamera(new Vector2(1050, 50), new Vector2(1000, 500));
        }

        public static CameraManager GetInstance()
        {
            if (managerInstance == null)
                managerInstance = new CameraManager();

            return managerInstance;
        }

        public Rectangle WorldToScreen(Rectangle worldRect)
        {
           return cameraScript.Camera.WorldToScreen(worldRect);
        }

        public Vector2 WorldToScreen(Vector2 worldLoc)
        {
            return cameraScript.Camera.WorldToScreen(worldLoc);
        }

        public Vector2 Position
        {
            get
            {
                return cameraScript.Camera.Position;
            }
        }

        public int ViewPortWidth
        {
            get
            {
                return cameraScript.Camera.ViewPortWidth;
            }
        }

        public int ViewPortHeight
        {
            get
            {
                return cameraScript.Camera.ViewPortHeight;
            }
        }

        public void Move(float timePassed, Vector2 location, Vector2 velocity,float step)
        {
            cameraScript.Logic(timePassed, location, velocity,step);
        }
    }
}

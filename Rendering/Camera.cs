using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carmageddon1MapEditor.Rendering
{
    public abstract class Camera
    {
        protected static Camera hasMouseMove = null;

        public static readonly float NearPlane = 0.01f;
        public static readonly float FarPlane = 1000f;

        public Vector3 position;

        public Viewport Viewport { get; private set; }
        public Matrix Translation => Matrix.CreateTranslation(position);
        public abstract Matrix View { get; }
        public abstract Matrix Projection { get; }
        public bool HasInput => (hasMouseMove == this)
            || (hasMouseMove == null && Viewport.Bounds.Contains(Mouse.GetState().X, Mouse.GetState().Y));

        private int lastMouseWheel = 0;
        private bool isInMouseMove = false;
        private int lastMouseX = 0;
        private int lastMouseY = 0;

        protected Camera()
        {
            lastMouseWheel = Mouse.GetState().ScrollWheelValue;
        }

        public void Update(Rectangle viewport, float deltaTime)
        {
            Viewport = new(viewport);

            if (HasInput)
            {
                int mouseWheel = Mouse.GetState().ScrollWheelValue;
                int mouseWheelDelta = mouseWheel - lastMouseWheel;
                if (Math.Abs(mouseWheelDelta) >= 120)
                {
                    mouseWheelDelta /= 120;
                }

                float deltaX = 0f;
                float deltaY = 0f;
                if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
                {
                    if (hasMouseMove != null)
                    {
                        if (hasMouseMove == this)
                        {
                            deltaX = lastMouseX - Mouse.GetState().X;
                            deltaY = lastMouseY - Mouse.GetState().Y;
                        }
                    }
                    else
                    {
                        hasMouseMove = this;
                    }
                }
                else
                {
                    hasMouseMove = null;
                }

                HandleInput(deltaTime, mouseWheelDelta, deltaX, deltaY);
            }

            lastMouseWheel = Mouse.GetState().ScrollWheelValue;
            lastMouseX = Mouse.GetState().X;
            lastMouseY = Mouse.GetState().Y;
        }

        protected abstract void HandleInput(float deltaTime, int deltaWheel, float deltaX, float deltaY);
    }
}

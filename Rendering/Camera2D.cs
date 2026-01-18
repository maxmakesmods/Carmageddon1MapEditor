using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Carmageddon1MapEditor.Rendering
{
    public class Camera2D : Camera
    {
        private float zoom = 10f;

        private float cameraZoomSpeed = 1.2f;

        private readonly Vector3 forward;
        private readonly Vector3 up;

        public override Matrix View => Matrix.CreateLookAt(position, position + forward, up);
        public override Matrix Projection => Matrix.CreateOrthographic(Viewport.Width / zoom, Viewport.Height / zoom, NearPlane, FarPlane);

        public Camera2D(Vector3 forward, Vector3 up)
        {
            this.forward = forward;
            this.up = up;
        }

        protected override void HandleInput(float deltaTime, int deltaWheel, float deltaX, float deltaY)
        {
            if (deltaWheel > 0)
            {
                zoom *= deltaWheel * cameraZoomSpeed;
            }
            else if (deltaWheel < 0)
            {
                zoom /= -deltaWheel * cameraZoomSpeed;
            }
            position += Vector3.Cross(forward, up) * (deltaX / zoom);
            position -= up * (deltaY / zoom);
        }
    }
}

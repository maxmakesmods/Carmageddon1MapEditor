using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Carmageddon1MapEditor.Rendering
{
    public class Camera3D : Camera
    {
        public float yaw;
        public float pitch;
        public float fov = 45f;
        private float cameraSpeed = 2f;
        private float cameraRotationSpeed = 45.0f;

        public override Matrix View => Matrix.CreateLookAt(position, position + Forward, Up);
        public override Matrix Projection => Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), Viewport.AspectRatio, NearPlane, FarPlane);

        public Matrix Rotation => Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f);

        public Vector3 Forward => Vector3.Transform(Vector3.Forward, Rotation);
        public Vector3 Right => Vector3.Transform(Vector3.Right, Rotation);
        public Vector3 Up => Vector3.Transform(Vector3.Up, Rotation);

        public Camera3D()
        {
            yaw = -0.7f;
            pitch = -0.7f;

            position = - Forward * 3;
        }

        protected override void HandleInput(float deltaTime, int deltaWheel, float deltaX, float deltaY)
        {
            yaw = MathHelper.WrapAngle(yaw + MathHelper.ToRadians(deltaX));
            pitch = MathHelper.WrapAngle(pitch + MathHelper.ToRadians(deltaY));

            float speedFactor = Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? 4.0f : 1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position += Forward * cameraSpeed * deltaTime * speedFactor;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position -= Forward * cameraSpeed * deltaTime * speedFactor;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position -= Right * cameraSpeed * deltaTime * speedFactor;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position += Right * cameraSpeed * deltaTime * speedFactor;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                position += Up * cameraSpeed * deltaTime * speedFactor;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                position -= Up * cameraSpeed * deltaTime * speedFactor;
            }
        }
    }
}

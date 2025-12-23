using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Carmageddon1MapEditor.Rendering
{
    internal class Camera3D
    {
        private static readonly float NearPlane = 0.01f;
        private static readonly float FarPlane = 1000f;

        public Viewport Viewport { get; set; }

        public Matrix Rotation => Matrix.CreateFromYawPitchRoll(yaw, pitch, 0f);
        public Matrix Translation => Matrix.CreateTranslation(position);
        public Matrix View => Matrix.CreateLookAt(position, position + Forward, Up);
        public Matrix Projection => Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), Viewport.AspectRatio, NearPlane, FarPlane);

        public Vector3 Forward => Vector3.Transform(Vector3.Forward, Rotation);
        public Vector3 Right => Vector3.Transform(Vector3.Right, Rotation);
        public Vector3 Up => Vector3.Transform(Vector3.Up, Rotation);

        public Vector3 position;
        public float yaw;
        public float pitch;
        public float fov = 45f;
        private float cameraSpeed = 2f;
        private float cameraRotationSpeed = 45.0f;

        private bool isInMouseMove = false;
        private int lastMouseX = 0;
        private int lastMouseY = 0;

        public Camera3D(Vector3 worldCenter)
        {
            yaw = -0.7f;
            pitch = -0.7f;

            position = worldCenter - Forward * 3;
        }

        public void Update(Rectangle viewport, float deltaTime)
        {
            Viewport = new(viewport);

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                if (isInMouseMove)
                {
                    float deltaYaw = (lastMouseX - Mouse.GetState().X) * cameraRotationSpeed * deltaTime;
                    yaw = MathHelper.WrapAngle(yaw + MathHelper.ToRadians(deltaYaw));
                    float deltaPitch = (lastMouseY - Mouse.GetState().Y) * cameraRotationSpeed * deltaTime;
                    pitch = MathHelper.WrapAngle(pitch + MathHelper.ToRadians(deltaPitch));
                }
                else
                {
                    isInMouseMove = true;
                }
                lastMouseX = Mouse.GetState().X;
                lastMouseY = Mouse.GetState().Y;
            }
            else
            {
                isInMouseMove = false;
            }

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

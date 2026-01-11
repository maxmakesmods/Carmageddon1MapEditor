using Microsoft.Xna.Framework;

namespace Carmageddon1MapEditor.Rendering
{
    public class Camera2D : Camera
    {
        public override Matrix View => Matrix.CreateLookAt(position, position + Vector3.Forward, Vector3.Up);
        public override Matrix Projection => Matrix.CreateOrthographic(Viewport.Width, Viewport.Height, NearPlane, FarPlane);

        public override void Update(Rectangle viewport, float deltaTime)
        {
            base.Update(viewport, deltaTime);

//            if (Mouse.GetState())
        }
    }
}

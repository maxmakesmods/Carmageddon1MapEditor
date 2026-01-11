using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carmageddon1MapEditor.Rendering
{
    public abstract class Camera
    {
        public static readonly float NearPlane = 0.01f;
        public static readonly float FarPlane = 1000f;

        public Vector3 position;

        public Viewport Viewport { get; private set; }
        public Matrix Translation => Matrix.CreateTranslation(position);
        public abstract Matrix View { get; }
        public abstract Matrix Projection { get; }

        public virtual void Update(Rectangle viewport, float deltaTime)
        {
            Viewport = new(viewport);

        }
    }
}

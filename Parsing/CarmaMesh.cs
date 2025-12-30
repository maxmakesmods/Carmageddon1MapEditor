
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Carmageddon1MapEditor.Parsing
{
    public class CarmaMesh
    {
        public readonly short unknown;
        public readonly string name;

        public readonly List<Vector3> vertexPositions = [];
        public readonly List<Vector2> vertexUVs = [];
        public readonly List<CarmaFace> faces = [];
        public readonly List<string> materialNames = [];

        public CarmaMesh(short unknown, string name)
        {
            this.unknown = unknown;
            this.name = name;
        }
    }
}

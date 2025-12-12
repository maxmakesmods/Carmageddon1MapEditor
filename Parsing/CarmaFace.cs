using Microsoft.Xna.Framework;

namespace Carmageddon1MapEditor.Parsing
{
    public class CarmaFace
    {
        public CarmaTriangle indices;
        public short unknownbitflag;
        public byte unknown;

        public override string ToString()
        {
            return $"[{indices},{unknownbitflag},{unknown}]";
        }
    }
}

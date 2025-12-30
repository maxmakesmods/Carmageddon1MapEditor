
namespace Carmageddon1MapEditor.Parsing
{
    public class CarmaFace
    {
        public CarmaTriangle indices;
        public short unknownbitflag;
        public byte unknown;
        public int matIndex = -1;

        public override string ToString()
        {
            return $"[{indices},{unknownbitflag},{unknown}]";
        }
    }
}

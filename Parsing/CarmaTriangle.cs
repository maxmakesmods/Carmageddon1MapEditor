
namespace Carmageddon1MapEditor.Parsing
{
    public readonly struct CarmaTriangle(short x, short y, short z)
    {
        public readonly short x = x, y = y, z = z;

        public override string ToString()
        {
            return $"[{x},{y},{z}]";
        }
    }
}

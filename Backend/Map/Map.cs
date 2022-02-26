using Avace.Backend.Interfaces.Map;

namespace Avace.Backend.Map
{
    internal class Map : IMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public TerrainType GetTerrainAt(int x, int y)
        {
            return TerrainType.Grass;
        }
    }
}
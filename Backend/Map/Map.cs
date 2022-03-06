using System.Collections.Generic;
using Avace.Backend.Interfaces.Map;
using Avace.Backend.Interfaces.Math;

namespace Avace.Backend.Map
{
    public class Map : IMap
    {
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public IEnumerable<MapLayer> Layers => LayersInternal;
        public IEnumerable<MapArea> Areas => AreasInternal;
        public Vector2Int PlayerSpawn { get; internal set; }
        internal List<MapLayer> LayersInternal { get; } = new List<MapLayer>();
        internal List<MapArea> AreasInternal { get; } = new List<MapArea>();
    }
}

using System.Collections.Generic;
using Avace.Backend.Interfaces.Math;

namespace Avace.Backend.Interfaces.Map
{
    public interface IMap
    {
        int Width { get; }
        int Height { get; }
        IEnumerable<MapLayer> Layers { get; }
        IEnumerable<MapArea> Areas { get; }
        Vector2Int PlayerSpawn { get; }
    }
}

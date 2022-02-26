using System.Collections.Generic;
using System.Numerics;
using Avace.Backend.Interfaces.Math;
using Avace.Backend.Map;

namespace Avace.Backend.Interfaces.Map;

public interface IMap
{
    int Width { get; }
    int Height { get; }
    IEnumerable<MapLayer> Layers { get; }
    Vector2Int PlayerSpawn { get; }

    int? GetTerrainAt(int x, int y, string layer);
}
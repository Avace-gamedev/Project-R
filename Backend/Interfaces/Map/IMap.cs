using System.Collections.Generic;

namespace Avace.Backend.Interfaces.Map;

public interface IMap
{
    int Width { get; }
    int Height { get; }
        
    IEnumerable<string> Layers { get; }

    int? GetTerrainAt(int x, int y, string layer);
}
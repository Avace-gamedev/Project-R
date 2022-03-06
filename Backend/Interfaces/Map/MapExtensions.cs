using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Avace.Backend.Interfaces.Map
{
    public static class MapExtensions
    {
        public static int? GetTerrainAt(this IMap map, int x, int y, string layer)
        {
            ReadOnlyCollection<int?> layerTiles = map.GetLayer(layer);
            int index = map.CoordsToIndex(x, y);

            return layerTiles[index];
        }

        public static ReadOnlyCollection<int?> GetLayer(this IMap map, string layerName)
        {
            MapLayer[] layers = map.Layers.Where(l => l.Name == layerName).ToArray();

            return layers.Length switch
            {
                0 => throw new InvalidOperationException($"Could not find layer {layerName}"),
                1 => layers[0].Tiles,
                _ => throw new InvalidOperationException($"Found {layers.Length} layers with name {layerName}")
            };
        }

        public static int CoordsToIndex(this IMap map, int x, int y)
        {
            return x + y * map.Width;
        }
    }
}

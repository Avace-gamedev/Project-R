using System;
using System.Collections.ObjectModel;

namespace Avace.Backend.Interfaces.Map
{
    public struct MapLayer
    {
        public string Name { get; private set; }
        public int Order { get; private set; }
        public bool Collision { get; private set; }
        public ReadOnlyCollection<int?> Tiles { get; private set; }

        public MapLayer(string name, int order, int?[] tiles, bool collision = false)
        {
            Name = name;
            Order = order;
            Tiles = Array.AsReadOnly(tiles);
            Collision = collision;
        }
    }
}

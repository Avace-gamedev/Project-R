using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Configuration
{
    [CreateAssetMenu(fileName = "TilemapConfiguration", menuName = "Configuration/Tilemap")]
    public class TilemapConfiguration : ScriptableObject
    {
        public List<Tile> tiles;
    }
}

using Avace.Backend.Interfaces.Math;

namespace Avace.Backend.Interfaces.Map
{
    public struct MapArea
    {
        public string Name { get; internal set; }

        /// <summary>
        /// Shape of the area
        /// </summary>
        public Vector2Int[] Shape { get; internal set; }

        /// <summary>
        /// Max number of groups in this area
        /// </summary>
        public int MaxGroups { get; internal set; }
    }
}

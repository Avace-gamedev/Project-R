namespace Avace.Backend.Interfaces.Map
{
    public interface IMap
    {
        int Width { get; }
        int Height { get; }

        TerrainType GetTerrainAt(int x, int y);
    }
}
namespace Avace.Backend.Interfaces.Map;

public struct MapLayer
{
    public string Name { get; private set; }
    public int Order { get; private set; }
    public bool Collision { get; private set; }

    public MapLayer(string name, int order, bool collision = false)
    {
        Name = name;
        Order = order;
        Collision = collision;
    }
}
namespace Avace.Backend.Interfaces.Math;

public struct Vector2Int
{
    public int X { get; private set; }
    public int Y { get; private set; }
    
    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }
}
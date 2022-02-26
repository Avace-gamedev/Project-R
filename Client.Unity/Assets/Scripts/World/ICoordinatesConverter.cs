using UnityEngine;

namespace World
{
    public interface ICoordinatesConverter
    {
        Vector3 Convert(Vector2Int cell);
        Vector2Int Convert(Vector3 world);
    }
}
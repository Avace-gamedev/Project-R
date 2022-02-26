using UnityEngine;

namespace World
{
    public interface IPlayerStartPositionProvider
    {
        Vector2Int PlayerStartPosition { get; }
    }
}
using Avace.Backend.Interfaces.Map;
using Avace.Backend.Utils;

namespace Avace.Backend.Map;

internal class DevMapProvider : IMapProvider
{
    public IMap Get()
    {
        return TiledMap.FromPath(Ressources.MakePath("Maps/Test/test.tmx"));
    }
}
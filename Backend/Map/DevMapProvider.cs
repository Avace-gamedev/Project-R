using Avace.Backend.Interfaces.Map;

namespace Avace.Backend.Map
{
    internal class DevMapProvider : IMapProvider
    {
        public IMap Get()
        {
            return new Map(20, 20);
        }
    }
}
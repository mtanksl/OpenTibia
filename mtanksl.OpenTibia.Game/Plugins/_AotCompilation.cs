#if AOT
using System.Collections.Generic;

namespace OpenTibia.Game.Plugins
{
    public static class _AotCompilation
    {
        public static readonly Dictionary<string, Plugin> Plugins = new Dictionary<string, Plugin>()
        {
            
        };
    }
}
#endif
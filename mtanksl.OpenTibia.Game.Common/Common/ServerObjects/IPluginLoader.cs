using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPluginLoader : IDisposable
    {
        void Start();

        Type GetType(string typeName);

        IEnumerable<Type> GetTypes(Type baseClass);

        Assembly GetAssembly(string pluginName);

        IEnumerable< KeyValuePair<string, Assembly> > GetAssemblies();
    }
}
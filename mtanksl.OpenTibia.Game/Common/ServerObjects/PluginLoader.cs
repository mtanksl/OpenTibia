using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class PluginLoader : IPluginLoader
    {
        private class PluginLoadContext : AssemblyLoadContext
        {
            private AssemblyDependencyResolver resolver;

            public PluginLoadContext(string componentAssemblyPath) : base(isCollectible: true)
            {
                resolver = new AssemblyDependencyResolver(componentAssemblyPath);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);

                if (assemblyPath != null)
                {
                    return LoadFromAssemblyPath(assemblyPath);
                }

                return null;
            }

            protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
            {
                string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

                if (libraryPath != null)
                {
                    return LoadUnmanagedDllFromPath(libraryPath);
                }

                return IntPtr.Zero;
            }
        }

        private IServer server;

        public PluginLoader(IServer server)
        {
            this.server = server;
        }

        ~PluginLoader()
        {
            Dispose(false);
        }

        private Dictionary<string, (PluginLoadContext AssemblyLoadContext, Assembly Assembly)> contexts = new();

        public void Start()
        {
            foreach (var path in Directory.GetDirectories(server.PathResolver.GetFullPath("data/dlls") ) )
            {
                string pluginName = Path.GetFileName(path);

                string assemblyPath = server.PathResolver.GetFullPath("data/dlls/" + pluginName + "/" + pluginName + ".dll");

                if (File.Exists(assemblyPath) )
                {
                    var assemblyLoadContext = new PluginLoadContext(assemblyPath);

                    var assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);

                    contexts.Add(pluginName, (assemblyLoadContext, assembly) );
                }
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
      
        public Type GetType(string typeName)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(PluginLoader));
            }

            string[] split = typeName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (split.Length == 1)
            {
                return Type.GetType(split[0] );
            }

            Assembly assembly = GetAssembly(split[1] );

            if (assembly != null)
            {
                return assembly.GetType(split[0] );
            }

            return null;
        }

        /// <exception cref="ObjectDisposedException"></exception>
     
        public IEnumerable<Type> GetTypes(Type baseClass)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(PluginLoader) );
            }

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => baseClass.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                yield return type;
            }

            foreach (var context in contexts)
            {
                foreach (var type in context.Value.Assembly.GetTypes().Where(t => baseClass.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
                {
                    yield return type;
                }
            }
        }

        /// <exception cref="ObjectDisposedException"></exception>
      
        public Assembly GetAssembly(string pluginName)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(PluginLoader) );
            }

            (PluginLoadContext AssemblyLoadContext, Assembly Assembly) context;

            if (contexts.TryGetValue(pluginName, out context) )
            {
                return context.Assembly;
            }

            return null;
        }

        /// <exception cref="ObjectDisposedException"></exception>
       
        public IEnumerable< KeyValuePair<string, Assembly> > GetAssemblies()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(PluginLoader) );
            }

            foreach (var context in contexts)
            {
                yield return new KeyValuePair<string, Assembly>(context.Key, context.Value.Assembly);
            }
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    if (contexts != null)
                    {
                        foreach (var context in contexts)
                        {
                            context.Value.AssemblyLoadContext.Unload();
                        }
                    }
                }
            }
        }
    }
}
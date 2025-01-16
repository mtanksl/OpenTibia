using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Values : IValues
    {
        private IServer server;

        public Values(IServer server)
        {
            this.server = server;
        }

        ~Values()
        {
            Dispose(false);
        }
               
        private ILuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/values/config.lua"),
                server.PathResolver.GetFullPath("data/values/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );
        }

        /// <exception cref="ObjectDisposedException"></exception>

        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Config) );
            }

            return script[key];
        }

        private Dictionary<Type, Dictionary<string, object> > buckets = new Dictionary<Type, Dictionary<string, object> >();

        private T GetValue<T>(string key, Func<object, T> convert)
        {
            Dictionary<string, object> bucket;

            if ( !buckets.TryGetValue(typeof(T), out bucket) )
            {
                bucket = new Dictionary<string, object>();

                buckets.Add(typeof(T), bucket);
            }

            object value;

            if ( !bucket.TryGetValue(key, out value) )
            {
                value = convert(GetValue(key) );

                bucket.Add(key, value);
            }
            
            return (T)value;
        }

        public bool GetBoolean(string key)
        {
            return GetValue(key, value => LuaScope.GetBoolean(value) );
        }

        public ushort GetByte(string key)
        {
            return GetValue(key, value => LuaScope.GetByte(value) );
        }

        public ushort GetUInt16(string key)
        {
            return GetValue(key, value => LuaScope.GetUInt16(value) );
        }

        public int GetInt32(string key)
        {
            return GetValue(key, value => LuaScope.GetInt32(value) );
        }

        public uint GetUInt32(string key)
        {
            return GetValue(key, value => LuaScope.GetUInt32(value) );
        }

        public long GetInt64(string key)
        {
            return GetValue(key, value => LuaScope.GetInt64(value) );
        }

        public ulong GetUInt64(string key)
        {
            return GetValue(key, value => LuaScope.GetUInt64(value) );
        }

        public double GetDouble(string key)
        {
            return GetValue(key, value => LuaScope.GetDouble(value) );
        }

        public string GetString(string key)
        {
            return GetValue(key, value => LuaScope.GetString(value) );
        }

        public bool[] GetBooleanArray(string key)
        {
            return GetValue(key, value => LuaScope.GetBooleanArray(value) );
        }

        public ushort[] GetUInt16Array(string key)
        {
            return GetValue(key, value => LuaScope.GetUInt16Array(value) );
        }

        public int[] GetInt32Array(string key)
        {
            return GetValue(key, value => LuaScope.GetInt32Array(value) );
        }

        public long[] GetInt64Array(string key)
        {
            return GetValue(key, value => LuaScope.GetInt64Array(value) );
        }

        public string[] GetStringArray(string key)
        {
            return GetValue(key, value => LuaScope.GetStringArray(value) );
        }

        public List<ushort> GetUInt16List(string key)
        {
            return GetValue(key, value => LuaScope.GetUInt16List(value) );
        }

        public HashSet<ushort> GetUInt16HashSet(string key)
        {
            return GetValue(key, value => LuaScope.GetUInt16HashSet(value) );
        }

        public Dictionary<ushort, ushort> GetUInt16IUnt16Dictionary(string key)
        {
            return GetValue(key, value => LuaScope.GetUInt16IUnt16Dictionary(value) );
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
                    if (script != null)
                    {
                        script.Dispose();
                    }
                }
            }
        }
    }
}
using System;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class LuaException : Exception 
    {
        public LuaException(string chunkName, string message) : base(chunkName + ": " + message)
        {
            
        }
    }
}
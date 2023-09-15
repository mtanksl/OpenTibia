using System;

namespace OpenTibia.Game
{
    public class LuaException : Exception 
    {
        public LuaException(string chunkName, string message) : base(chunkName + ": " + message)
        {
            
        }
    }
}
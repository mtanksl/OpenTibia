using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerAdvanceLevelPlugin : PlayerAdvanceLevelPlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingPlayerAdvanceLevelPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingPlayerAdvanceLevelPlugin(ILuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/creaturescripts/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/creaturescripts/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override Promise OnAdvanceLevel(Player player, ushort fromLevel, ushort toLevel)
        {
            if (fileName != null)
            {
                return script.CallFunction("onadvancelevel", player, fromLevel, toLevel);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onadvancelevel"], player, fromLevel, toLevel);
            }           
        }

        public override void Stop()
        {
            if (fileName != null)
            {
                script.Dispose();
            }
        }       
    }
}
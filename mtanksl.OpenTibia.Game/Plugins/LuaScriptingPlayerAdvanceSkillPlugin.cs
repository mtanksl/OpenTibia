using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;

namespace OpenTibia.Game.Plugins
{
    public class LuaScriptingPlayerAdvanceSkillPlugin : PlayerAdvanceSkillPlugin
    {
        private string fileName;

        private ILuaScope script;

        private LuaTable parameters;

        public LuaScriptingPlayerAdvanceSkillPlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingPlayerAdvanceSkillPlugin(ILuaScope script, LuaTable parameters)
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

        public override Promise OnAdvanceSkill(Player player, Skill skill, byte fromLevel, byte toLevel)
        {
            if (fileName != null)
            {
                return script.CallFunction("onAdvanceSkill", player, skill, fromLevel, toLevel);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onAdvanceSkill"], player, skill, fromLevel, toLevel);
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
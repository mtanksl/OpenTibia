using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class LuaScriptingSpellPlugin : SpellPlugin
    {
        private string fileName;

        public LuaScriptingSpellPlugin(string fileName, Spell spell) : base(spell) 
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/spells/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            return script.CallFunction("oncasting", player, target, message).Then(result =>
            {
                return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            return script.CallFunction("oncast", player, target, message);
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}
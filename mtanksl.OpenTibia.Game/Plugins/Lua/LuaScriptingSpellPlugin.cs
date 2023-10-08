using mtanksl.OpenTibia.Game.Plugins;
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

            Spell.Condition = (player, message) =>
            {
                return script.CallFunction("oncasting", player, message).Then(result =>
                {
                    return Promise.FromResult( (bool)result[0] );
                } );
            };

            Spell.Callback = (player, message) =>
            {
                return script.CallFunction("oncast", player, message);
            };
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}
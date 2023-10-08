using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class LuaScriptingDialoguePlugin : DialoguePlugin
    {
        private string fileName;

        public LuaScriptingDialoguePlugin(string fileName)
        {
            this.fileName = fileName;
        }

        private LuaScope script;

        public override void Start()
        {
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/npcs/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override PromiseResult<bool> ShouldGreet(Npc npc, Player player, string message)
        {
            return script.CallFunction("shouldgreet", npc, player, message).Then(result =>
            {
                return Promise.FromResult( (bool)result[0] );
            } );
        }

        public override PromiseResult<bool> ShouldFarewell(Npc npc, Player player, string message)
        {
            return script.CallFunction("shouldfarewell", npc, player, message).Then(result =>
            {
                return Promise.FromResult( (bool)result[0] );
            } );
        }

        public override Promise OnGreet(Npc npc, Player player)
        {
            return script.CallFunction("ongreet", npc, player);
        }

        public override Promise OnBusy(Npc npc, Player player)
        {
            return script.CallFunction("onbusy", npc, player);
        }

        public override Promise OnSay(Npc npc, Player player, string message)
        {
            return script.CallFunction("onsay", npc, player, message);
        }

        public override Promise OnFarewell(Npc npc, Player player)
        {
            return script.CallFunction("onfarewell", npc, player);
        }

        public override Promise OnDismiss(Npc npc, Player player)
        {
            return script.CallFunction("ondismiss", npc, player);
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}
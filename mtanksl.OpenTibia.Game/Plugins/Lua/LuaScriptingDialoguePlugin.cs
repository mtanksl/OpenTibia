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
            script = Context.Server.LuaScripts.Create(Context.Server.PathResolver.GetFullPath("data/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), Context.Server.PathResolver.GetFullPath("data/plugins/npcs/lib.lua"), Context.Server.PathResolver.GetFullPath(fileName) );
        }

        public override PromiseResult<bool> ShouldGreet(Npc npc, Player player, string message)
        {
            return script.CallFunction("shouldgreet", npc, player, message).Then(result =>
            {
                return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
            } );
        }

        public override PromiseResult<bool> ShouldFarewell(Npc npc, Player player, string message)
        {
            return script.CallFunction("shouldfarewell", npc, player, message).Then(result =>
            {
                return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
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

        public override Promise OnBuy(Npc npc, Player player, ushort openTibiaId, byte type, byte count, int price, bool ignoreCapacity, bool buyWithBackpacks)
        {
            return script.CallFunction("onbuy", npc, player, openTibiaId, type, count, price, ignoreCapacity, buyWithBackpacks);
        }

        public override Promise OnSell(Npc npc, Player player, ushort openTibiaId, byte type, byte count, int price, bool keepEquipped)
        {
            return script.CallFunction("onsell", npc, player, openTibiaId, type, count, price, keepEquipped);
        }

        public override Promise OnCloseNpcTrade(Npc npc, Player player)
        {
            return script.CallFunction("onclosenpctrade", npc, player);
        }

        public override Promise OnFarewell(Npc npc, Player player)
        {
            return script.CallFunction("onfarewell", npc, player);
        }

        public override Promise OnDisappear(Npc npc, Player player)
        {
            return script.CallFunction("ondisappear", npc, player);
        }

        public override Promise OnEnqueue(Npc npc, Player player)
        {
            return script.CallFunction("onenqueue", npc, player);
        }

        public override Promise OnDequeue(Npc npc, Player player)
        {
            return script.CallFunction("ondequeue", npc, player);
        }

        public override void Stop()
        {
            script.Dispose();
        }
    }
}
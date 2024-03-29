using NLua;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class LuaScriptingDialoguePlugin : DialoguePlugin
    {
        private string fileName;

        private LuaScope script;

        private LuaTable parameters;

        public LuaScriptingDialoguePlugin(string fileName)
        {
            this.fileName = fileName;
        }

        public LuaScriptingDialoguePlugin(LuaScope script, LuaTable parameters)
        {
            this.script = script;

            this.parameters = parameters;
        }

        public override void Start()
        {
            if (fileName != null)
            {
                script = Context.Server.LuaScripts.LoadScript(
                    Context.Server.PathResolver.GetFullPath("data/plugins/npcs/" + fileName),
                    Context.Server.PathResolver.GetFullPath("data/plugins/npcs/lib.lua"),
                    Context.Server.PathResolver.GetFullPath("data/plugins/lib.lua"), 
                    Context.Server.PathResolver.GetFullPath("data/lib.lua") );
            }
        }

        public override PromiseResult<bool> ShouldGreet(Npc npc, Player player, string message)
        {
            if (fileName != null)
            {
                return script.CallFunction("shouldgreet", npc, player, message).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["shouldgreet"], npc, player, message).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }            
        }

        public override PromiseResult<bool> ShouldFarewell(Npc npc, Player player, string message)
        {
            if (fileName != null)
            {
                return script.CallFunction("shouldfarewell", npc, player, message).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["shouldfarewell"], npc, player, message).Then(result =>
                {
                    return (bool)result[0] ? Promise.FromResultAsBooleanTrue : Promise.FromResultAsBooleanFalse;
                } );
            }            
        }

        public override Promise OnGreet(Npc npc, Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("ongreet", npc, player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["ongreet"], npc, player);
            }
        }

        public override Promise OnBusy(Npc npc, Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("onbusy", npc, player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onbusy"], npc, player);
            }
        }

        public override Promise OnSay(Npc npc, Player player, string message)
        {
            if (fileName != null)
            {
                return script.CallFunction("onsay", npc, player, message);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onsay"], npc, player, message);
            }
        }

        public override Promise OnBuy(Npc npc, Player player, ushort openTibiaId, byte type, byte count, int price, bool ignoreCapacity, bool buyWithBackpacks)
        {
            if (fileName != null)
            {
                return script.CallFunction("onbuy", npc, player, openTibiaId, type, count, price, ignoreCapacity, buyWithBackpacks);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onbuy"], npc, player, openTibiaId, type, count, price, ignoreCapacity, buyWithBackpacks);
            }
        }

        public override Promise OnSell(Npc npc, Player player, ushort openTibiaId, byte type, byte count, int price, bool keepEquipped)
        {
            if (fileName != null)
            {
                return script.CallFunction("onsell", npc, player, openTibiaId, type, count, price, keepEquipped);     
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onsell"], npc, player, openTibiaId, type, count, price, keepEquipped);
            }
        }

        public override Promise OnCloseNpcTrade(Npc npc, Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("onclosenpctrade", npc, player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onclosenpctrade"], npc, player);
            }
        }

        public override Promise OnFarewell(Npc npc, Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("onfarewell", npc, player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onfarewell"], npc, player);
            }
        }

        public override Promise OnDisappear(Npc npc, Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("ondisappear", npc, player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["ondisappear"], npc, player);
            }
        }

        public override Promise OnEnqueue(Npc npc, Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("onenqueue", npc, player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["onenqueue"], npc, player);
            }
        }

        public override Promise OnDequeue(Npc npc, Player player)
        {
            if (fileName != null)
            {
                return script.CallFunction("ondequeue", npc, player);
            }
            else
            {
                return script.CallFunction( (LuaFunction)parameters["ondequeue"], npc, player);                
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
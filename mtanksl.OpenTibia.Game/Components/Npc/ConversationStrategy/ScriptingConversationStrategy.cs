using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class ScriptingConversationStrategy : IConversationStrategy
    {
        private string script;

        public ScriptingConversationStrategy(string script)
        {
            this.script = script;
        }

        private LuaScope lua;

        public void Start(Npc npc)
        {
            lua = Context.Current.Server.LuaScripts.Load("data/npcs/lib/npc.lua", "data/npcs/scripts/" + script);

                lua.RegisterFunction("npcsay", parameters =>
                {
                    return Context.Current.AddCommand(new NpcSayCommand(npc, (string)parameters[0] ) ).Then( () =>
                    {
                        return Promise.FromResult(Array.Empty<object>() );
                    } );
                } );

                lua.RegisterFunction("npcremoveitem", parameters =>
                {
                    Player player = (Player)parameters[0];
                    long item = (long)parameters[1];
                    long count = (long)parameters[2];

                    return Promise.FromResult(new object[] { false } );
                } );

                lua.RegisterFunction("npcaddmoney", parameters =>
                {
                    Player player = (Player)parameters[0];
                    long price = (long)parameters[1];

                    return Promise.FromResult(Array.Empty<object>() );
                } );

                lua.RegisterFunction("npcdeletemoney", parameters =>
                {
                    Player player = (Player)parameters[0];
                    long price = (long)parameters[1];

                    return Promise.FromResult(new object[] { false } );
                } );

                lua.RegisterFunction("npcadditem", parameters =>
                {
                    Player player = (Player)parameters[0];
                    long item = (long)parameters[1];
                    long count = (long)parameters[2];

                    return Promise.FromResult(Array.Empty<object>() );
                } );
        }

        public PromiseResult<bool> ShouldGreet(Npc npc, Player player, string message)
        {
            return lua.Call("shouldgreet", npc, player, message).Then(result =>
            {
                return Promise.FromResult( (bool)result[0] );
            } );
        }

        public PromiseResult<bool> ShouldFarewell(Npc npc, Player player, string message)
        {
            return lua.Call("shouldfarewell", npc, player, message).Then(result =>
            {
                return Promise.FromResult( (bool)result[0] );
            } );
        }

        public Promise OnGreet(Npc npc, Player player)
        {
            return lua.Call("ongreet", npc, player);
        }

        public Promise OnBusy(Npc npc, Player player)
        {
            return lua.Call("onbusy", npc, player);
        }

        public Promise OnSay(Npc npc, Player player, string message)
        {
            return lua.Call("onsay", npc, player, message);
        }

        public Promise OnFarewell(Npc npc, Player player)
        {
            return lua.Call("onfarewell", npc, player);
        }

        public Promise OnDismiss(Npc npc, Player player)
        {
            return lua.Call("ondismiss", npc, player);
        }

        public void Stop()
        {
            
        }
    }
}
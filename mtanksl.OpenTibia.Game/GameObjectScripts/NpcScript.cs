using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.GameObjectScripts
{
    public class NpcScript : GameObjectScript<string, Npc>
    {
        public override string Key
        {
            get
            {
                return "";
            }
        }

        public override void Start(Npc npc)
        {
            if (npc.Metadata.Sentences != null)
            {
                Context.Server.GameObjectComponents.AddComponent(npc, new CreatureTalkBehaviour(TalkType.Say, npc.Metadata.Sentences) );
            }

            Func<Npc, ConversationPlugin> factory;

            if (Context.Server.Plugins.ConversationPlugins.TryGetValue(npc.Name, out factory) )
            {
                ConversationPlugin conversationPlugin = factory(npc);

                Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(conversationPlugin, new RandomWalkStrategy(2) ) );
            }
            else
            {
                ConversationPlugin conversationPlugin = new LuaScriptingConversationPlugin(npc, "data/plugins/npcs/default.lua");

                Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(conversationPlugin, new RandomWalkStrategy(2) ) );
            }
        }

        public override void Stop(Npc npc)
        {

        }
    }
}
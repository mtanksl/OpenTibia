using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class CipfriedNpcScript : NpcScript
    {
        public override string Key
        {
            get
            {
                return "Cipfried";
            }
        }

        public override void Start(Npc npc)
        {
            base.Start(npc);

            Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(new ScriptingConversationStrategy("cipfried.lua"), new RandomWalkStrategy(2) ) );
        }

        public override void Stop(Npc npc)
        {
            base.Stop(npc);


        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class AldeeNpcScript : NpcScript
    {
        public override string Key
        {
            get
            {
                return "Aldee";
            }
        }

        public override void Start(Npc npc)
        {
            base.Start(npc);

            Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(new ScriptingConversationStrategy("aldee.lua"), new RandomWalkStrategy(2) ) );
        }

        public override void Stop(Npc npc)
        {
            base.Stop(npc);


        }
    }
}
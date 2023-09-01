using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class CaptainBluebearScript : NpcScript
    {
        public override string Key
        {
            get
            {
                return "Captain Bluebear";
            }
        }

        public override void Start(Npc npc)
        {
            base.Start(npc);

            Context.Server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(new ScriptingConversationStrategy("captain bluebear.lua"), new RandomWalkStrategy(2) ) );
        }

        public override void Stop(Npc npc)
        {
            base.Stop(npc);


        }
    }
}
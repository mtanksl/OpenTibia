using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class NpcScript : GameObjectScript<Npc>
    {
        public override void Start(Npc npc)
        {
            if (npc.Metadata.Voices != null)
            {
                Context.Server.GameObjectComponents.AddComponent(npc, new NpcTalkBehaviour(npc.Metadata.Voices) );
            }

            if (Context.Server.Config.GameplayPrivateNpcSystem)
            {
                Context.Server.GameObjectComponents.AddComponent(npc, new MultipleQueueNpcThinkBehaviour(NpcWalkStrategy.Instance) );
            }
            else
            {
                Context.Server.GameObjectComponents.AddComponent(npc, new SingleQueueNpcThinkBehaviour(NpcWalkStrategy.Instance) );
            }
        }

        public override void Stop(Npc npc)
        {

        }
    }
}
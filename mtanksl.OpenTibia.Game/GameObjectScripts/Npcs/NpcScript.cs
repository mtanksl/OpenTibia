using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class NpcScript : GameObjectScript<Npc>
    {
        public override void Start(Npc npc)
        {
            if (npc.Metadata.Sentences != null && npc.Metadata.Sentences.Length > 0)
            {
                Context.Server.GameObjectComponents.AddComponent(npc, new CreatureTalkBehaviour(TalkType.Say, npc.Metadata.Sentences) );
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
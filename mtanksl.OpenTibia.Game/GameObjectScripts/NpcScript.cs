using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;

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
            if (npc.Metadata.Sentences != null && npc.Metadata.Sentences.Length > 0)
            {
                Context.Server.GameObjectComponents.AddComponent(npc, new CreatureTalkBehaviour(TalkType.Say, npc.Metadata.Sentences) );
            }

            DialoguePlugin dialoguePlugin = Context.Server.Plugins.GetDialoguePlugin(npc.Name);

            if (dialoguePlugin != null)
            {
                if (Context.Server.Config.GameplayPrivateNpcSystem)
                {
                    Context.Server.GameObjectComponents.AddComponent(npc, new MultipleQueueNpcThinkBehaviour(dialoguePlugin, NpcWalkStrategy.Instance) );
                }
                else
                {
                    Context.Server.GameObjectComponents.AddComponent(npc, new SingleQueueNpcThinkBehaviour(dialoguePlugin, NpcWalkStrategy.Instance) );
                }
            }
        }

        public override void Stop(Npc npc)
        {

        }
    }
}
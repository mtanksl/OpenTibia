using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseNpcsChannelCommand : IncomingCommand
    {
        public ParseCloseNpcsChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GameplayPrivateNpcSystem)
            {
                List<Promise> promises = new List<Promise>();

                foreach (var npc in Context.Server.Map.GetObserversOfTypeNpc(Player.Tile.Position) )
                {
                    MultipleQueueNpcThinkBehaviour npcThinkBehaviour = Context.Server.GameObjectComponents.GetComponent<MultipleQueueNpcThinkBehaviour>(npc);

                    if (npcThinkBehaviour != null)
                    {
                        promises.Add(npcThinkBehaviour.CloseNpcsChannel(Player) );
                    }
                }

                return Promise.WhenAll(promises.ToArray() );
            }
             
            return Promise.Completed;
        }
    }
}
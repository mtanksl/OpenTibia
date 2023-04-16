using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStartAttackCommand : Command
    {
        public ParseStartAttackCommand(Player player, uint creatureId, uint nonce)
        {
            Player = player;

            CreatureId = creatureId;

            Nonce = nonce;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public uint Nonce { get; set; }

        public override Promise Execute()
        {
            Creature creature = Context.Server.GameObjects.GetCreature(CreatureId);
                
            if (creature != null && creature != Player)
            {
                PlayerAttackAndFollowBehaviour playerAttackAndFollowThinkBehaviour = Context.Server.Components.GetComponent<PlayerAttackAndFollowBehaviour>(Player);

                if (creature is Npc)
                {
                    if (playerAttackAndFollowThinkBehaviour != null)
                    {
                        playerAttackAndFollowThinkBehaviour.Stop();
                    }

                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature),

                                                                new StopAttackAndFollowOutgoingPacket(0) );
                }
                else
                {
                    if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                    {
                        if (playerAttackAndFollowThinkBehaviour != null)
                        {
                            playerAttackAndFollowThinkBehaviour.Attack(creature);
                        }
                    }
                    else
                    {
                        if (playerAttackAndFollowThinkBehaviour != null)
                        {
                            playerAttackAndFollowThinkBehaviour.AttackAndFollow(creature);
                        }
                    }

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}
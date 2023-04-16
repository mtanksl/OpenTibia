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
                PlayerAttackAndFollowExternalBehaviour playerAttackAndFollowBehaviour = Context.Server.Components.GetComponent<PlayerAttackAndFollowExternalBehaviour>(Player);

                if (creature is Npc)
                {
                    if (playerAttackAndFollowBehaviour != null)
                    {
                        playerAttackAndFollowBehaviour.Stop();
                    }

                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature),

                                                                new StopAttackAndFollowOutgoingPacket(0) );
                }
                else
                {
                    if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                    {
                        if (playerAttackAndFollowBehaviour != null)
                        {
                            playerAttackAndFollowBehaviour.Attack(creature);
                        }
                    }
                    else
                    {
                        if (playerAttackAndFollowBehaviour != null)
                        {
                            playerAttackAndFollowBehaviour.AttackAndFollow(creature);
                        }
                    }

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}
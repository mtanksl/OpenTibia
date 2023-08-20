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
            Creature target = Context.Server.GameObjects.GetCreature(CreatureId);
                
            if (target != null && target != Player)
            {
                PlayerAttackAndFollowBehaviour playerAttackAndFollowBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerAttackAndFollowBehaviour>(Player);

                if (target is Npc || (target is Player player && player.Vocation == Vocation.Gamemaster) )
                {
                    if (playerAttackAndFollowBehaviour != null)
                    {
                        playerAttackAndFollowBehaviour.StopAttackAndFollow();
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
                            playerAttackAndFollowBehaviour.Attack(target);
                        }
                    }
                    else
                    {
                        if (playerAttackAndFollowBehaviour != null)
                        {
                            playerAttackAndFollowBehaviour.AttackAndFollow(target);
                        }
                    }

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}
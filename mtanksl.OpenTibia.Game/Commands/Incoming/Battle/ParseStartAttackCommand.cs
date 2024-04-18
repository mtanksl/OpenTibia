using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseStartAttackCommand : IncomingCommand
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
                PlayerThinkBehaviour playerThinkBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerThinkBehaviour>(Player);

                if (target is Npc || target is Player || target.Tile.ProtectionZone || Player.Tile.ProtectionZone)
                {
                    if (playerThinkBehaviour != null)
                    {
                        playerThinkBehaviour.StopAttackAndFollow();
                    }

                    Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature) );

                    Context.AddPacket(Player, new StopAttackAndFollowOutgoingPacket(0) );
                }
                else
                {
                    if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                    {
                        if (playerThinkBehaviour != null)
                        {
                            playerThinkBehaviour.Attack(target);
                        }
                    }
                    else
                    {
                        if (playerThinkBehaviour != null)
                        {
                            playerThinkBehaviour.AttackAndFollow(target);
                        }
                    }

                    return Promise.Completed;
                }
            }

            return Promise.Break;
        }
    }
}
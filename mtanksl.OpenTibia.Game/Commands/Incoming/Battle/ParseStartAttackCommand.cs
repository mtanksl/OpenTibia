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
            return Promise.Run( (resolve, reject) =>
            {
                Creature creature = context.Server.GameObjects.GetCreature(CreatureId);
                
                if (creature != null && creature != Player)
                {
                    AttackAndFollowBehaviour component = context.Server.Components.GetComponent<AttackAndFollowBehaviour>(Player);

                    if (creature is Npc)
                    {
                        context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature),

                                                                    new StopAttackAndFollowOutgoingPacket(0) );

                        component.Stop();
                    }
                    else
                    {
                        if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                        {
                            component.Attack(creature);
                        }
                        else
                        {
                            component.AttackAndFollow(creature);
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}
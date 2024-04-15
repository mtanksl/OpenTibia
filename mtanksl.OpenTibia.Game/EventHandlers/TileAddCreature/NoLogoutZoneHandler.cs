using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.CommandHandlers
{
    public class NoLogoutZoneHandler : EventHandler<TileAddCreatureEventArgs>
    {
        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.Creature is Player player)
            {
                if (e.ToTile.NoLogoutZone)
                {
                    if ( !player.HasSpecialCondition(SpecialCondition.ProtectionZoneBlock) )
                    {
                        player.AddSpecialCondition(SpecialCondition.ProtectionZoneBlock);

                        Context.AddPacket(player, new SetSpecialConditionOutgoingPacket(player.SpecialConditions) );
                    }
                }
                else
                {
                    if (player.HasSpecialCondition(SpecialCondition.ProtectionZoneBlock) )
                    {
                        player.RemoveSpecialCondition(SpecialCondition.ProtectionZoneBlock);

                        Context.AddPacket(player, new SetSpecialConditionOutgoingPacket(player.SpecialConditions) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ProtectionZoneBlockHandler : CommandHandler<CreatureMoveCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureMoveCommand command)
        {
            if (command.Creature is Player player && command.ToTile.ProtectionZone && player.HasSpecialCondition(SpecialCondition.ProtectionZoneBlock) )
            {
                Context.AddPacket(player, new ShowWindowTextOutgoingPacket(MessageMode.Failure, Constants.YouCanNotEnterAProtectionZoneAfterAttackingAnotherPlayer) );

                Context.AddPacket(player, new StopWalkOutgoingPacket(player.Direction) );

                return Promise.Break;
            }

            return next();
        }
    }
}
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DispayCooldownEffectHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            int id;

            if (command.Message.StartsWith("/ce ") && int.TryParse(command.Message.Substring(4), out id) && id >= 1 && id <= 255)
            {
                if (Context.Server.Features.HasFeatureFlag(FeatureFlag.CooldownBar) )
                {
                    Context.AddPacket(command.Player, new SendSpellCooldownOutgoingPacket( (byte)id, 1000) );
                }

                return Promise.Completed;
            }

            return next();
        }
    }
}
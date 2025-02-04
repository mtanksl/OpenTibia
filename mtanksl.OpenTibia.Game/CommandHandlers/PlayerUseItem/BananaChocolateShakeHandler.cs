using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BananaChocolateShakeHandler : CommandHandler<PlayerUseItemCommand>
    {
        private readonly HashSet<ushort> bananaChocolateShake;

        public BananaChocolateShakeHandler()
        {
            bananaChocolateShake = Context.Server.Values.GetUInt16HashSet("values.items.bananaChocolateShake");
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (bananaChocolateShake.Contains(command.Item.Metadata.OpenTibiaId) )
            {                  
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You don't really know what this did to you, but suddenly you feel very happy.") );

                return Context.AddCommand(new ItemDecrementCommand(command.Item, 1) ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "Slurp.") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Hearts) );
                } );
            }

            return next();
        }
    }
}
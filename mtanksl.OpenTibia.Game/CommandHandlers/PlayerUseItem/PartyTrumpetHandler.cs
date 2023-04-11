using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PartyTrumpetHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> partyTrumpets = new Dictionary<ushort, ushort>() 
        {
            { 6572, 6573 }
        };

        private Dictionary<ushort, ushort> decay = new Dictionary<ushort, ushort>() 
        {
            { 6573, 6572 }
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (partyTrumpets.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                switch (command.Item.Root() )
                {
                    case Tile tile:

                        Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.GreenNotes) );

                        break;

                    case Inventory inventory:

                        Context.AddCommand(new ShowMagicEffectCommand(inventory.Player.Tile.Position, MagicEffectType.GreenNotes) );

                        break;
                }
                               
                Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "TOOOOOOT!") );

                Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) ).Then( (item) =>
                {
                    return Context.AddCommand(new ItemDecayTransformCommand(item, 2000, decay[item.Metadata.OpenTibiaId], 1) );                      
                } );

                return Promise.Completed;
            }

            return next();
        }
    }
}
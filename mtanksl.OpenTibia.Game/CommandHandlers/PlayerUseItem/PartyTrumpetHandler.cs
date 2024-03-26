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
                return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.PartyAnimal, 200, "Party Animal") ).Then( (Func<Promise>)(() =>
                {
                    Position position = null;
                                        
                    switch (command.Item.Root() )
                    {
                        case Tile tile:

                            position = tile.Position;

                            break;

                        case Inventory inventory:

                            position = inventory.Player.Tile.Position;

                            break;

                        case LockerCollection safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    if (position != null)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.GreenNotes) );
                    }

                    return Promise.Completed;

                }) ).Then( () =>
                {
                    return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, "TOOOOOOT!") );

                } ).Then( () =>
                {
                    return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );

                } ).Then( (item) =>
                {
                    _ = Context.AddCommand(new ItemDecayTransformCommand(item, TimeSpan.FromSeconds(2), decay[item.Metadata.OpenTibiaId], 1) );

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}
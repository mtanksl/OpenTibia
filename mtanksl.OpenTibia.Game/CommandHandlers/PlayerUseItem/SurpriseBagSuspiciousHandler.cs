using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class SurpriseBagSuspiciousHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> surpriseBags = new HashSet<ushort>() { 9108 };

        private List< (ushort OpenTibiaId, byte Count) > prizes = new List< (ushort OpenTibiaId, byte Count) >()
        {
            (2148, 50),
            (2667, 1),
            (2114, 1),
            (1689, 1),
            (7735, 1),
            (8110, 1),
            (6394, 1),
            (7377, 1),
            (6574, 1),
            (7487, 1),
            (9693, 1)
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (surpriseBags.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(1, 5);

                if (value == 1)
                {
                    value = Context.Server.Randomization.Take(0, prizes.Count - 1);

                    return Promise.Completed.Then( (Func<Promise>)(() =>
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
                            return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.GiftWraps) );
                        }

                        return Promise.Completed;

                    }) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, prizes[value].OpenTibiaId, prizes[value].Count) );
                    } );
                }
                else
                {
                    return Promise.Completed.Then( (Func<Promise>)(() =>
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
                            return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.GiftWraps) );
                        }

                        return Promise.Completed;

                    }) ).Then( () =>
                    {
                        return Context.AddCommand(new ItemDestroyCommand(command.Item) );
                    } );
                }
            }

            return next();
        }
    }
}
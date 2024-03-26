using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class MusicalInstrumentHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> green = new HashSet<ushort>() { 2070, 2071, 2073, 2075, 2076, 2078, 2367, 2374 };

        private HashSet<ushort> purple = new HashSet<ushort>() { 2079 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (green.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Promise.Completed.Then( () =>
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

                        case Safe safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    if (position != null)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.GreenNotes) );
                    }

                    return Promise.Completed;
                } );
            }
            else if (purple.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                return Promise.Completed.Then( () =>
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

                        case Safe safe:

                            position = safe.Player.Tile.Position;

                            break;
                    }

                    if (position != null)
                    {
                        return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.PurpleNotes) );
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}
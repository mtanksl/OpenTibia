using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PiggyBankHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> piggyBanks = new Dictionary<ushort, ushort>()
        {
            { 2114, 2115 }
        };

        private ushort platinumCoin = 2152;

        private ushort goldCoin = 2148;

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (piggyBanks.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                int value = Context.Server.Randomization.Take(1, 5);

                if (value == 1)
                {
                    switch (command.Item.Root() )
                    {
                        case Tile tile:

                            return Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.YellowSpark) ).Then( () =>
                            {
                                return Context.AddCommand(new PlayerCreateItemCommand(command.Player, goldCoin, 1) );

                            } ).Then( () =>
                            {
                                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                            } );

                        case Inventory inventory:
                        case null:

                            return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.YellowSpark) ).Then( () =>
                            {
                                return Context.AddCommand(new PlayerCreateItemCommand(command.Player, goldCoin, 1) );

                            } ).Then( () =>
                            {
                                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                            } );
                    }
                }
                else
                {
                    switch (command.Item.Root() )
                    {
                        case Tile tile:

                            return Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.YellowNotes) ).Then( () =>
                            {
                                return Context.AddCommand(new PlayerCreateItemCommand(command.Player, platinumCoin, 1) );

                            } );

                        case Inventory inventory:
                        case null:

                            return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.YellowNotes) ).Then( () =>
                            {
                                return Context.AddCommand(new PlayerCreateItemCommand(command.Player, goldCoin, 1) );

                            } );
                    }
                }
            }

            return next();
        }
    }
}
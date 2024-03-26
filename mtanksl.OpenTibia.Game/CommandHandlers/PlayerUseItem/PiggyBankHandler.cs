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
                    return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.AllowanceCollector, 50, "Allowance Collector") ).Then( (Func<Promise>)(() =>
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
                            return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.YellowSpark) );
                        }

                        return Promise.Completed;

                    }) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, goldCoin, 1) );

                    } ).Then( () =>
                    {
                        return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
                    } );
                }
                else
                {
                    return Context.AddCommand(new PlayerAchievementCommand(command.Player, AchievementConstants.AllowanceCollector, 50, "Allowance Collector") ).Then( (Func<Promise>)(() =>
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
                            return Context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.YellowNotes) );
                        }

                        return Promise.Completed;

                    }) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerCreateItemCommand(command.Player, platinumCoin, 1) );
                    } );
                }
            }

            return next();
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LetterHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private ushort letter = 2597;

        private HashSet<ushort> mailbox = new HashSet<ushort>() { 2593, 3981 };

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.Item.Metadata.OpenTibiaId == letter && command.ToContainer is Tile toTile && toTile.TopItem != null && mailbox.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                ReadableItem letterItem = (ReadableItem)command.Item;

                if (letterItem.Text != null)
                {
                    string[] text = letterItem.Text.Split('\n');

                    if (text.Length >= 2)
                    {
                        string townName = text[1];

                        Town town = Context.Server.Map.GetTown(townName);

                        if (town != null)
                        {                                
                            //TODO: Transform into stamped letter

                            string playerName = text[0];

                            Player player = Context.Server.GameObjectPool.GetPlayer(playerName);

                            if (player != null)
                            {
                                Locker locker = Context.Server.Lockers.GetLocker(player.DatabasePlayerId, (ushort)town.Id);

                                if (locker.Count < locker.Metadata.Capacity)
                                {
                                    switch (letterItem.Parent)
                                    {
                                        case Tile tile:

                                            return Context.AddCommand(new TileRemoveItemCommand(tile, letterItem) ).Then( () =>
                                            {
                                                return Context.AddCommand(new ContainerAddItemCommand(locker, letterItem) );
                                            } );

                                        case Inventory inventory:

                                            return Context.AddCommand(new InventoryRemoveItemCommand(inventory, letterItem) ).Then( () =>
                                            {
                                                return Context.AddCommand(new ContainerAddItemCommand(locker, letterItem) );
                                            } );

                                        case Container container:

                                            return Context.AddCommand(new ContainerRemoveItemCommand(container, letterItem) ).Then( () =>
                                            {
                                                return Context.AddCommand(new ContainerAddItemCommand(locker, letterItem) );
                                            } );
                                    }
                                }
                                else
                                {
                                    //TODO: When target opens locker, show queue

                                    switch (letterItem.Parent)
                                    {
                                        case Tile tile:

                                            return Context.AddCommand(new TileRemoveItemCommand(tile, letterItem) ).Then( () =>
                                            {
                                                Context.Server.Inboxes.AddItem(player.DatabasePlayerId, (ushort)town.Id, letterItem);
                                            } );

                                        case Inventory inventory:

                                            return Context.AddCommand(new InventoryRemoveItemCommand(inventory, letterItem) ).Then( () =>
                                            {
                                                Context.Server.Inboxes.AddItem(player.DatabasePlayerId, (ushort)town.Id, letterItem);
                                            } );

                                        case Container container:

                                            return Context.AddCommand(new ContainerRemoveItemCommand(container, letterItem) ).Then( () =>
                                            {
                                                Context.Server.Inboxes.AddItem(player.DatabasePlayerId, (ushort)town.Id, letterItem);
                                            } );
                                    }
                                }
                            }
                            else
                            {
                                DbPlayer dbPlayer = Context.Database.PlayerRepository.GetPlayerByName(playerName);

                                if (dbPlayer != null)
                                {
                                    //TODO: When target opens locker, show queue

                                    switch (letterItem.Parent)
                                    {
                                        case Tile tile:

                                            return Context.AddCommand(new TileRemoveItemCommand(tile, letterItem) ).Then( () =>
                                            {
                                                Context.Server.Inboxes.AddItem(dbPlayer.Id, (ushort)town.Id, letterItem);
                                            } );

                                        case Inventory inventory:

                                            return Context.AddCommand(new InventoryRemoveItemCommand(inventory, letterItem) ).Then( () =>
                                            {
                                                Context.Server.Inboxes.AddItem(dbPlayer.Id, (ushort)town.Id, letterItem);
                                            } );

                                        case Container container:

                                            return Context.AddCommand(new ContainerRemoveItemCommand(container, letterItem) ).Then( () =>
                                            {
                                                Context.Server.Inboxes.AddItem(dbPlayer.Id, (ushort)town.Id, letterItem);
                                            } );
                                    }
                                }
                            }
                        }
                    }                
                }
            }

            return next();
        }
    }
}
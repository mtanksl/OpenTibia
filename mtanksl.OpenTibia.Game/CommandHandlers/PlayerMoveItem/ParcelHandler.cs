using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class ParcelHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private ushort parcel = 2595;

        private ushort label = 2599;

        private HashSet<ushort> mailbox = new HashSet<ushort>() { 2593, 3981 };

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.Item.Metadata.OpenTibiaId == parcel && command.ToContainer is Tile toTile && toTile.TopItem != null && mailbox.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                Container parcelItem = (Container)command.Item;

                ReadableItem labelItem = (ReadableItem)parcelItem.GetItems().Where(i => i.Metadata.OpenTibiaId == label).FirstOrDefault();

                if (labelItem != null)
                {
                    if (labelItem.Text != null)
                    {
                        string[] text = labelItem.Text.Split('\n');

                        if (text.Length >= 2)
                        {
                            string townName = text[1];

                            Town town = Context.Server.Map.GetTown(townName);

                            if (town != null)
                            {
                                //TODO: Close parcel and subcontainers

                                //TODO: Transform into stamped parcel

                                string playerName = text[0];

                                Player player = Context.Server.GameObjectPool.GetPlayer(playerName);

                                if (player != null)
                                {
                                    Locker locker = Context.Server.Lockers.GetLocker(player.DatabasePlayerId, (ushort)town.Id);

                                    if (locker.Count < locker.Metadata.Capacity)
                                    {
                                        switch (parcelItem.Parent)
                                        {
                                            case Tile tile:

                                                return Context.AddCommand(new TileRemoveItemCommand(tile, parcelItem) ).Then( () =>
                                                {
                                                    return Context.AddCommand(new ContainerAddItemCommand(locker, parcelItem) );
                                                } );

                                            case Inventory inventory:

                                                return Context.AddCommand(new InventoryRemoveItemCommand(inventory, parcelItem) ).Then( () =>
                                                {
                                                    return Context.AddCommand(new ContainerAddItemCommand(locker, parcelItem) );
                                                } );

                                            case Container container:

                                                return Context.AddCommand(new ContainerRemoveItemCommand(container, parcelItem) ).Then( () =>
                                                {
                                                    return Context.AddCommand(new ContainerAddItemCommand(locker, parcelItem) );
                                                } );
                                        }
                                    }
                                    else
                                    {
                                        //TODO: When target opens locker, show queue

                                        switch (parcelItem.Parent)
                                        {
                                            case Tile tile:

                                                return Context.AddCommand(new TileRemoveItemCommand(tile, parcelItem) ).Then( () =>
                                                {
                                                    Context.Server.Inboxes.AddItem(player.DatabasePlayerId, (ushort)town.Id, parcelItem);
                                                } );

                                            case Inventory inventory:

                                                return Context.AddCommand(new InventoryRemoveItemCommand(inventory, parcelItem) ).Then( () =>
                                                {
                                                    Context.Server.Inboxes.AddItem(player.DatabasePlayerId, (ushort)town.Id, parcelItem);
                                                } );

                                            case Container container:

                                                return Context.AddCommand(new ContainerRemoveItemCommand(container, parcelItem) ).Then( () =>
                                                {
                                                    Context.Server.Inboxes.AddItem(player.DatabasePlayerId, (ushort)town.Id, parcelItem);
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

                                        switch (parcelItem.Parent)
                                        {
                                            case Tile tile:

                                                return Context.AddCommand(new TileRemoveItemCommand(tile, parcelItem) ).Then( () =>
                                                {
                                                    Context.Server.Inboxes.AddItem(dbPlayer.Id, (ushort)town.Id, parcelItem);
                                                } );

                                            case Inventory inventory:

                                                return Context.AddCommand(new InventoryRemoveItemCommand(inventory, parcelItem) ).Then( () =>
                                                {
                                                    Context.Server.Inboxes.AddItem(dbPlayer.Id, (ushort)town.Id, parcelItem);
                                                } );

                                            case Container container:

                                                return Context.AddCommand(new ContainerRemoveItemCommand(container, parcelItem) ).Then( () =>
                                                {
                                                    Context.Server.Inboxes.AddItem(dbPlayer.Id, (ushort)town.Id, parcelItem);
                                                } );
                                        }
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
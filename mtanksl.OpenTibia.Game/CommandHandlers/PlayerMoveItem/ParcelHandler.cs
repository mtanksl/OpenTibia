using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class ParcelHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly ushort parcel;
        private readonly ushort label;
        private readonly HashSet<ushort> mailboxes;
        private readonly ushort stampedParcel;

        public ParcelHandler()
        {
            parcel = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.parcel") );
            label = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.label") );
            mailboxes = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.mailboxes") );
            stampedParcel = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.stampedParcel") );
        }

        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.Item.Metadata.OpenTibiaId == parcel && command.ToContainer is Tile toTile && toTile.TopItem != null && mailboxes.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
            {
                Container parcelItem = (Container)command.Item;

                ReadableItem labelItem = (ReadableItem)parcelItem.GetItems()
                    .Where(i => i.Metadata.OpenTibiaId == label)
                    .FirstOrDefault();

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
                                string playerName = text[0];

                                Player player = Context.Server.GameObjectPool.GetPlayerByName(playerName);

                                if (player != null)
                                {
                                    Locker locker = (Locker)player.Lockers.GetContent(town.Id);

                                    if (locker == null)
                                    {
                                        locker = (Locker)Context.Server.ItemFactory.Create(Constants.LockerOpenTibiaItemId, 1);

                                        locker.TownId = town.Id;

                                        Context.Server.ItemFactory.Attach(locker);

                                        Item depot = Context.Server.ItemFactory.Create(Constants.DepotOpenTibiaItemId, 1);

                                        Context.Server.ItemFactory.Attach(depot);

                                        locker.AddContent(depot);
                                                       
                                        player.Lockers.AddContent(locker, locker.TownId);
                                    }

                                    if (locker.Count < locker.Metadata.Capacity)
                                    {
                                        return Context.AddCommand(new ItemMoveCommand(parcelItem, locker, 0) ).Then( () =>
                                        {
                                            return Context.AddCommand(new ItemTransformCommand(parcelItem, stampedParcel, 1) );
                                        } );
                                    }
                                }
                                else
                                {
                                    //TODO: Load player from database, add parcel to locker
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
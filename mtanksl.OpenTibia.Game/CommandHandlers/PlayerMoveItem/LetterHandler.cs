using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LetterHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private readonly ushort letter;
        private readonly HashSet<ushort> mailboxes;
        private readonly ushort stampedLetter;

        public LetterHandler()
        {
            letter = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.letter") );
            mailboxes = LuaScope.GetInt16HashSet(Context.Server.Values.GetValue("values.items.mailboxes") );
            stampedLetter = LuaScope.GetInt16(Context.Server.Values.GetValue("values.items.stampedLetter") );
        }
            
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            if (command.Item.Metadata.OpenTibiaId == letter && command.ToContainer is Tile toTile && toTile.TopItem != null && mailboxes.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
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
                                    return Context.AddCommand(new ItemTransformCommand(letterItem, stampedLetter, 1) ).Then( (item) =>
                                    {
                                        return Context.AddCommand(new ItemMoveCommand(item, locker, 0) );
                                    } );
                                }
                            }
                            else
                            {
                                //TODO: Load player from database, add letter to locker
                            }
                        }
                    }                
                }
            }

            return next();
        }
    }
}
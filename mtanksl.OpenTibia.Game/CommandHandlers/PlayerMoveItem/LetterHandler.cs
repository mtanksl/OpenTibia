using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class LetterHandler : CommandHandler<PlayerMoveItemCommand>
    {
        private ushort letter = 2597;

        private HashSet<ushort> mailbox = new HashSet<ushort>() { 2593, 3981 };

        private ushort stampedLetter = 2598;

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
                            string playerName = text[0];

                            Player player = Context.Server.GameObjectPool.GetPlayer(playerName);

                            if (player != null)
                            {
                                Locker locker = (Locker)player.Lockers.GetContent(town.Id);

                                if (locker == null)
                                {
                                    locker = (Locker)Context.Server.ItemFactory.Create(2591, 1);

                                    locker.Town = town;

                                    Context.Server.ItemFactory.Attach(locker);

                                    Item depot = Context.Server.ItemFactory.Create(2594, 1);

                                    Context.Server.ItemFactory.Attach(depot);

                                    locker.AddContent(depot);
              
                                    player.Lockers.AddContent(locker, locker.Town.Id);
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
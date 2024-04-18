using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemChestHandler : CommandHandler<PlayerUseItemCommand>
    {
        private static HashSet<ushort> chests = new HashSet<ushort>() { 1740, 1747, 1748, 1749 };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (command.Item.UniqueId > 0 && chests.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int value;

                if (command.Player.Storages.TryGetValue(command.Item.UniqueId, out value) && value > 0)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "It is empty.") );

                    return Promise.Completed;
                }

                command.Player.Storages.SetValue(command.Item.UniqueId, 1);

                List<Promise> promises = new List<Promise>();

                StringBuilder builder = new StringBuilder();

                foreach (var item in ( (Container)command.Item).GetItems() )
                {
                    string name;

                    if (item is StackableItem stackableItem && stackableItem.Count > 1)
                    {
                        if (item.Metadata.Plural != null)
                        {
                            name = stackableItem.Count + " " + item.Metadata.Plural;
                        }
                        else
                        {
                            if (item.Metadata.Name != null)
                            {
                                name = stackableItem.Count + " " + item.Metadata.Name;
                            }
                            else
                            {
                                name = "nothing special";
                            }
                        }
                    }
                    else
                    {
                        if (item.Metadata.Name != null)
                        {
                            if (item.Metadata.Article != null)
                            {
                                name = item.Metadata.Article + " " + item.Metadata.Name;
                            }
                            else
                            {
                                name = item.Metadata.Name;
                            }
                        }
                        else
                        {
                            name = "nothing special";
                        }
                    }

                    builder.Append(name + ", ");

                    promises.Add(Context.AddCommand(new ItemCloneCommand(item, true) ).Then( (clone) =>
                    {
                        return Context.AddCommand(new PlayerAddItemCommand(command.Player, clone) );
                    } ) );
                }

                if (builder.Length > 2)
                {
                    builder.Remove(builder.Length - 2, 2);
               
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You have found " + builder.ToString() + ".") );
                }

                return Promise.WhenAll(promises.ToArray() );                
            }

            return next();
        }
    }
}
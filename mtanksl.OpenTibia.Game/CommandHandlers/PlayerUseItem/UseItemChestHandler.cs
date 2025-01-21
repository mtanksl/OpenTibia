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
        private readonly HashSet<ushort> chests;

        public UseItemChestHandler()
        {
            chests = Context.Server.Values.GetUInt16HashSet("values.items.chests");
        }

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
                    builder.Append(item.Metadata.GetDescription(item is StackableItem stackableItem ? stackableItem.Count : (byte)1) + ", ");

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
                else
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "It is empty.") );
                }

                return Promise.WhenAll(promises.ToArray() );                
            }

            return next();
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class EditHouseDoorHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("aleta grav") )
            {
                if (command.Player.Tile is HouseTile houseTile && (houseTile.House.IsOwner(command.Player.Name) || houseTile.House.IsSubOwner(command.Player.Name) ) && houseTile.TopItem != null && houseTile.TopItem is DoorItem doorItem)
                {
                    foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                    {
                        command.Player.Client.Windows.CloseWindow(pair.Key);
                    }

                    Window window = new Window();

                    window.House = houseTile.House;

                    window.DoorId = doorItem.DoorId;

                    uint windowId = command.Player.Client.Windows.OpenWindow(window);

                    Context.AddPacket(command.Player, new OpenEditListDialogOutgoingPacket(0x00, windowId, houseTile.House.GetDoorList(window.DoorId).Text) );

                    return Promise.Completed;
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}
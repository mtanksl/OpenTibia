using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class EditHouseGuestHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("aleta sio") )
            {
                if (command.Player.Tile is HouseTile houseTile && (houseTile.House.IsOwner(command.Player.Name) || houseTile.House.IsSubOwner(command.Player.Name) ) )
                {
                    foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                    {
                        command.Player.Client.Windows.CloseWindow(pair.Key);
                    }

                    Window window = new Window();

                    window.House = houseTile.House;

                    window.DoorId = 0xFF;

                    uint windowId = command.Player.Client.Windows.OpenWindow(window);

                    Context.AddPacket(command.Player, new OpenEditListDialogOutgoingPacket(0x00, windowId, houseTile.House.GetGuestsList().Text) );

                    return Promise.Completed;
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }   
}
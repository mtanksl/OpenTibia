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
        private HashSet<ushort> openDoors = new HashSet<ushort>()
        {
            // Brick
            5100,
            5102,

            5109,
            5111, 
            
            // Framework
            1214,
            1222,
            5139,
                      
            1211,
            1220,
            5142,
                  
            // Pyramid
            1236,
            1240,

            1233,
            1238,
                  
            // White stone
            1254,
            5518,
                               
            1251,
            5516,

            // Stone
            5118,
            5120,
            5136,

            5127,
            5129,
            5145,

            // Stone
            6254,
            6258,
                           
            6251,
            6256,

            // Fence
            1542,

            1540

            //TODO: More items
        };

        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("aleta grav") )
            {
                if (command.Player.Tile is HouseTile houseTile && (houseTile.House.IsOwner(command.Player.Name) || houseTile.House.IsSubOwner(command.Player.Name) ) && houseTile.TopItem != null && openDoors.Contains(houseTile.TopItem.Metadata.OpenTibiaId) )
                {
                    foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                    {
                        command.Player.Client.Windows.CloseWindow(pair.Key);
                    }

                    Window window = new Window();

                    window.House = houseTile.House;

                    window.DoorId = 0x03;

                    window.DoorPosition = command.Player.Tile.Position;

                    uint windowId = command.Player.Client.Windows.OpenWindow(window);

                    Context.AddPacket(command.Player, new OpenEditListDialogOutgoingPacket(window.DoorId, windowId, houseTile.House.GetDoorList(window.DoorPosition).Text) );

                    return Promise.Completed;
                }

                return Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );
            }

            return next();
        }
    }
}
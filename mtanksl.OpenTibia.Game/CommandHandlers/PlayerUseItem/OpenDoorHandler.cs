using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class OpenDoorHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> doors = new Dictionary<ushort, ushort>()
        {
            // Brick
            { 5099, 5100 },
            { 5101, 5102 },
            { 5108, 5109 },
            { 5110, 5111 },

            // Framework
            { 1210, 1211 },
            { 1213, 1214 },
            { 1219, 1220 },
            { 1221, 1222 },
            { 5138, 5139 },
            { 5141, 5142 },

            // Pyramid
            { 1232, 1233 },
            { 1235, 1236 },
            { 1237, 1238 },
            { 1239, 1240 },

            // White stone
            { 1250, 1251 },
            { 1253, 1254 },
            { 5515, 5516 },
            { 5517, 5518 },

            // Stone
            { 5117, 5118 },
            { 5119, 5120 },
            { 5126, 5127 },
            { 5128, 5129 },
            { 5135, 5136 },
            { 5144, 5145 },
                        
            // Stone
            { 6250, 6251 },
            { 6253, 6254 },
            { 6255, 6256 },
            { 6257, 6258 },

            //Fence
            { 1539, 1540 },
            { 1541, 1542 },

            //TODO: More items
        };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            ushort toOpenTibiaId;

            if (doors.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                if (command.Item.Parent is HouseTile houseTile && command.Item is DoorItem doorItem && !houseTile.House.CanOpenDoor(command.Player.Name, doorItem) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                    return Promise.Break;
                }

                return Context.AddCommand(new ItemTransformCommand(command.Item, toOpenTibiaId, 1) );
            }

            return next();
        }
    }
}
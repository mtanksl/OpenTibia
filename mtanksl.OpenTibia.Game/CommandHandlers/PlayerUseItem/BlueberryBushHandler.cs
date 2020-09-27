using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class BlueberryBushHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, ushort> blueberryBushs = new Dictionary<ushort, ushort>() 
        {
            { 2785, 2786 }
        };

        private ushort blueberry = 2677;

        private ushort toOpenTibiaId;

        public override bool CanHandle(PlayerUseItemCommand command, Server server)
        {
            if (blueberryBushs.TryGetValue(command.Item.Metadata.OpenTibiaId, out toOpenTibiaId) )
            {
                return true;
            }

            return false;
        }

        public override Command Handle(PlayerUseItemCommand command, Server server)
        {
            return new SequenceCommand(

                new ItemTransformCommand(command.Item, toOpenTibiaId), 
                
                new TileCreateItemCommand( (Tile)command.Item.Container, blueberry, 3) );
        }
    }
}
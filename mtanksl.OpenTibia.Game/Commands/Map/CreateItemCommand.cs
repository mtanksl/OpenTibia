using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreateItemCommand : Command
    {
        public CreateItemCommand(ushort openTibiaId, Position toPosition)
        {
            OpenTibiaId = openTibiaId;

            ToPosition = toPosition;
        }

        public ushort OpenTibiaId { get; set; }

        public Position ToPosition { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Item item = server.ItemFactory.Create(OpenTibiaId);

            if (item != null)
            {
                Tile toTile = server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    //Act

                    byte toIndex = toTile.AddContent(item);

                    //Notify

                    foreach (var observer in server.Map.GetPlayers() )
                    {
                        if (observer.Tile.Position.CanSee(toTile.Position) )
                        {
                            context.Write(observer.Client.Connection, new ThingAddOutgoingPacket(toTile.Position, toIndex, item) );
                        }
                    }

                    base.Execute(server, context);
                }
            }
        }
    }
}
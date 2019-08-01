using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class TileRemoveCreatureCommand : Command
    {
        public TileRemoveCreatureCommand(Tile tile, byte index)
        {
            Tile = tile;

            Index = index;
        }

        public Tile Tile { get; set; }

        public byte Index { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Creature creature = Tile.GetContent(Index) as Creature;

            //Act

            Tile.RemoveContent(Index);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(Tile.Position) )
                {
                    context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(Tile.Position, Index) );
                }
            }

            foreach (var script in server.Scripts.TileRemoveCreatureScripts)
            {
                script.OnTileRemoveCreature(creature, Tile, Index, server, context);
            }

            base.Execute(server, context);
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateLightCommand : Command
    {
        public CreatureUpdateLightCommand(Creature creature, Light light)
        {
            Creature = creature;

            Light = light;
        }

        public Creature Creature { get; set; }

        public Light Light { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                if (Creature.Light != Light)
                {
                    Creature.Light = Light;

                    Tile fromTile = Creature.Tile;

                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        if (observer.Tile.Position.CanSee(fromTile.Position) )
                        {
                            Context.AddPacket(observer.Client.Connection, new SetLightOutgoingPacket(Creature.Id, Creature.Light) );
                        }
                    }
                }

                resolve();
            } );
        }
    }
}
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

        public override void Execute(Context context)
        {
            if (Creature.Light != Light)
            {
                Tile fromTile = Creature.Tile;

                Creature.Light = Light;

                foreach (var observer in context.Server.GameObjects.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new SetLightOutgoingPacket(Creature.Id, Creature.Light) );
                    }
                }
            }

            base.Execute(context);
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowProjectileCommand : Command
    {
        public ShowProjectileCommand(IContent fromContent, IContent toContent, ProjectileType projectileType)
        {
            Position fromPosition = null;

            switch (fromContent)
            {
                case Item item:

                    switch (item.Root() )
                    {
                        case Tile tile:

                            fromPosition = tile.Position;

                            break;

                        case Inventory inventory:

                            fromPosition = inventory.Player.Tile.Position;

                            break;

                        case LockerCollection safe:

                            fromPosition = safe.Player.Tile.Position;

                            break;
                    }

                    break;

                case Creature creature:

                    fromPosition = creature.Tile.Position;

                    break;
            }

            Position toPosition = null;

            switch (toContent)
            {
                case Item item:

                    switch (item.Root() )
                    {
                        case Tile tile:

                            toPosition = tile.Position;

                            break;

                        case Inventory inventory:

                            toPosition = inventory.Player.Tile.Position;

                            break;

                        case LockerCollection safe:

                            toPosition = safe.Player.Tile.Position;

                            break;
                    }

                    break;

                case Creature creature:

                    toPosition = creature.Tile.Position;

                    break;
            }

            FromPosition = fromPosition;

            ToPosition = toPosition;

            ProjectileType = projectileType;
        }

        public ShowProjectileCommand(Position fromPosition, Position toPosition, ProjectileType projectileType)
        {
            FromPosition = fromPosition;

            ToPosition = toPosition;

            ProjectileType = projectileType;
        }

        public Position FromPosition { get; set; }

        public Position ToPosition { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public override Promise Execute()
        {
            if (FromPosition != null && ToPosition != null)
            {
                ShowProjectileOutgoingPacket showProjectileOutgoingPacket = new ShowProjectileOutgoingPacket(FromPosition, ToPosition, ProjectileType);

                foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(ToPosition) )
                {
                    if (observer.Tile.Position.CanSee(ToPosition) )
                    {
                        Context.AddPacket(observer, showProjectileOutgoingPacket);
                    }
                }
            }

            return Promise.Completed;
        }
    }
}
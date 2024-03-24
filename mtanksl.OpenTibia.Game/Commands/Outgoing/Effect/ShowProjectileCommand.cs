using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ShowProjectileCommand : Command
    {
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
            ShowProjectileOutgoingPacket showProjectileOutgoingPacket = new ShowProjectileOutgoingPacket(FromPosition, ToPosition, ProjectileType);

            foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(ToPosition) )
            {
                if (observer.Tile.Position.CanSee(ToPosition) )
                {
                    Context.AddPacket(observer, showProjectileOutgoingPacket);
                }
            }

            return Promise.Completed;
        }
    }
}
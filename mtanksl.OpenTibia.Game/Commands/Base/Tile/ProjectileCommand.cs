using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ProjectileCommand : Command
    {
        public ProjectileCommand(Position fromPosition, Position toPosition, ProjectileType projectileType)
        {
            FromPosition = fromPosition;

            ToPosition = toPosition;

            ProjectileType = projectileType;
        }

        public Position FromPosition { get; set; }

        public Position ToPosition { get; set; }

        public ProjectileType ProjectileType { get; set; }

        public override void Execute(Context context)
        {
            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer.Tile.Position.CanSee(FromPosition) || observer.Tile.Position.CanSee(ToPosition) )
                {
                    context.WritePacket(observer.Client.Connection, new ShowProjectileOutgoingPacket(FromPosition, ToPosition, ProjectileType) );
                }
            }

            base.OnCompleted(context);
        }
    }
}
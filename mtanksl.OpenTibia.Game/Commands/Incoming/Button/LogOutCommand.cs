using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        public LogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Context context)
        {
            Tile fromTile = Player.Tile;

            byte fromIndex = fromTile.GetIndex(Player);

            fromTile.RemoveContent(fromIndex);

            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (observer == Player)
                {
                    context.Disconnect(observer.Client.Connection);
                }
                else
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, fromIndex),
                            
                                                                      new ShowMagicEffectOutgoingPacket(fromTile.Position, MagicEffectType.Puff) );
                    }
                }
            }

            context.Server.PlayerFactory.Destroy(Player);

            OnComplete(context);
        }
    }
}
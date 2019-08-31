using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class LogOutCommand : Command
    {
        public LogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            if ( !server.Scripts.PlayerLogoutScripts.Any(script => script.OnPlayerLogout(Player, Player.Tile, server, context) ) )
            {
                Tile fromTile = Player.Tile;

                Position fromPosition = fromTile.Position;

                byte fromIndex = fromTile.GetIndex(Player);

                //Act

                fromTile.RemoveContent(fromIndex);

                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer == Player)
                    {
                        context.Disconnect(observer.Client.Connection);
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(fromPosition) )
                        {
                            context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex),
                            
                                                                      new ShowMagicEffectOutgoingPacket(fromPosition, MagicEffectType.Puff) );
                        }
                    }
                }

                server.Map.RemoveCreature(Player);

                foreach (var component in Player.GetComponents<Behaviour>() )
                {
                    component.Stop(server);
                }

                //Event

                if (server.Events.Logout != null)
                {
                    server.Events.Logout(this, new LogoutEventArgs(Player, fromTile, fromIndex, server, context) );
                }

                if (server.Events.TileRemoveCreature != null)
                {
                    server.Events.TileRemoveCreature(this, new TileRemoveCreatureEventArgs(Player, fromTile, fromIndex, server, context) );
                }
            }

            base.Execute(server, context);
        }
    }
}
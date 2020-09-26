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

        public override void Execute(Context context)
        {
            //Arrange

            if ( !context.Server.Scripts.PlayerLogoutScripts.Any(script => script.OnPlayerLogout(Player, Player.Tile, context) ) )
            {
                Tile fromTile = Player.Tile;

                Position fromPosition = fromTile.Position;

                byte fromIndex = fromTile.GetIndex(Player);

                //Act

                fromTile.RemoveContent(fromIndex);

                //Notify

                foreach (var observer in context.Server.Map.GetPlayers() )
                {
                    if (observer == Player)
                    {
                        context.Disconnect(observer.Client.Connection);
                    }
                    else
                    {
                        if (observer.Tile.Position.CanSee(fromPosition) )
                        {
                            context.AddPacket(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromPosition, fromIndex),
                            
                                                                      new ShowMagicEffectOutgoingPacket(fromPosition, MagicEffectType.Puff) );
                        }
                    }
                }

                context.Server.Map.RemoveCreature(Player);

                foreach (var component in Player.GetComponents<Behaviour>() )
                {
                    component.Stop(context.Server);
                }

                //Event

                if (context.Server.Events.TileRemoveCreature != null)
                {
                    context.Server.Events.TileRemoveCreature(this, new TileRemoveCreatureEventArgs(fromTile, Player, fromIndex) );
                }
            }

            base.Execute(context);
        }
    }
}
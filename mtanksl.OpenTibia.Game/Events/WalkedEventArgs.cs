using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Events
{
    public class WalkedEventArgs : EventArgs, IEvent
    {
        private Server server;

        public WalkedEventArgs(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Tile FromTile { get; set; }

        public byte FromIndex { get; set; }

        public Tile ToTile { get; set; }

        public byte ToIndex { get; set; }

        public void Execute(Context context)
        {
            foreach (var observer in server.CreatureCollection.GetPlayers() )
            {
                if (observer != Player)
                {
                    bool canSeeFromPosition = observer.Tile.Position.CanSee(FromTile.Position);

                    bool canSeeToPosition = observer.Tile.Position.CanSee(ToTile.Position);

                    if (canSeeFromPosition && canSeeToPosition)
                    {
                        context.Response.Write(observer.Client.Connection, new Walk(FromTile.Position, FromIndex, ToTile.Position) );
                    }
                    else if (canSeeFromPosition)
                    {
                        context.Response.Write(observer.Client.Connection, new ThingRemove(FromTile.Position, FromIndex) );
                    }
                    else if (canSeeToPosition)
                    {
                        uint removeId;

                        if (observer.Client.IsKnownCreature(Player.Id, out removeId) )
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(ToTile.Position, ToIndex, Player) );
                        }
                        else
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(ToTile.Position, ToIndex, removeId, Player) );
                        }
                    }
                }
            }
                
            context.Response.Write(Player.Client.Connection, new Walk(FromTile.Position, FromIndex, ToTile.Position) );

            int deltaZ = ToTile.Position.Z - FromTile.Position.Z;

            if (deltaZ == -1)
            {
                context.Response.Write(Player.Client.Connection, new SendMapUp(server.Map, Player.Client, FromTile.Position) );
            }
            else if (deltaZ == 1)
            {
                context.Response.Write(Player.Client.Connection, new SendMapDown(server.Map, Player.Client, FromTile.Position) );
            }

            int deltaY = ToTile.Position.Y - FromTile.Position.Y;
                
            if (deltaY == -1)
            {
                context.Response.Write(Player.Client.Connection, new SendMapNorth(server.Map, Player.Client, FromTile.Position.Offset(0, 0, deltaZ) ) );
            }
            else if (deltaY == 1)
            {
                context.Response.Write(Player.Client.Connection, new SendMapSouth(server.Map, Player.Client, FromTile.Position.Offset(0, 0, deltaZ) ) );
            }

            int deltaX = ToTile.Position.X - FromTile.Position.X;
                
            if (deltaX == -1)
            {
                context.Response.Write(Player.Client.Connection, new SendMapWest(server.Map, Player.Client, FromTile.Position.Offset(0, 0, deltaZ) ) );
            }
            else if (deltaX == 1)
            {
                context.Response.Write(Player.Client.Connection, new SendMapEast(server.Map, Player.Client, FromTile.Position.Offset(0, 0, deltaZ) ) );
            }
        }
    }
}
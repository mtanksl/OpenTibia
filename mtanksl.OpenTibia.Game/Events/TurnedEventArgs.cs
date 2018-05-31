using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Events
{
    public class TurnedEventArgs : EventArgs, IEvent
    {
        private Server server;

        public TurnedEventArgs(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Tile FromTile { get; set; }

        public byte FromIndex { get; set; }

        public Direction FromDirection { get; set; }

        public Direction ToDirection { get; set; }

        public void Execute(Context context)
        {
            foreach (var observer in server.CreatureCollection.GetPlayers() )
            {
                if (observer != Player)
                {
                    if (observer.Tile.Position.CanSee(FromTile.Position) )
                    {
                        
                    }
                }
            }
        }
    }
}
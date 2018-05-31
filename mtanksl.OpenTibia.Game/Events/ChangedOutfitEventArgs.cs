using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Events
{
    public class ChangedOutfitEventArgs : EventArgs, IEvent
    {
        private Server server;

        public ChangedOutfitEventArgs(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Tile FromTile { get; set; }

        public byte FromIndex { get; set; }

        public Outfit FromOutfit { get; set; }

        public Outfit ToOutfit { get; set; }
        
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
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;
using System;

namespace OpenTibia.Game.Events
{
    public class LoggedInEventArgs : EventArgs, IEvent
    {
        private Server server;

        public LoggedInEventArgs(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Tile FromTile { get; set; }

        public byte FromIndex { get; set; }

        public void Execute(Context context)
        {
            foreach (var observer in server.CreatureCollection.GetPlayers() )
            {
                if (observer != Player)
                {
                    if (observer.Tile.Position.CanSee(Player.Tile.Position) )
                    {
                        uint removeId;

                        if (observer.Client.IsKnownCreature(Player.Id, out removeId) )
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(FromTile.Position, FromIndex, Player) );
                            
                            context.Response.Write(observer.Client.Connection, new ShowMagicEffect(FromTile.Position, MagicEffectType.Teleport) );
                        }
                        else
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(FromTile.Position, FromIndex, removeId, Player) );

                            context.Response.Write(observer.Client.Connection, new ShowMagicEffect(FromTile.Position, MagicEffectType.Teleport) );
                        }
                    }
                }
            }

            context.Response.Write(Player.Client.Connection, new SendInfo(Player.Id, Player.CanReportBugs) );
            
            context.Response.Write(Player.Client.Connection, new SetSpecialCondition(SpecialCondition.None) );
            
            context.Response.Write(Player.Client.Connection, new SendStatus(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, 0, 0, Player.Soul, Player.Stamina) );
            
            context.Response.Write(Player.Client.Connection, new SendSkills(10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0) );
            
            context.Response.Write(Player.Client.Connection, new SetEnvironmentLight(Light.Day) );
            
            context.Response.Write(Player.Client.Connection, new SendTiles(server.Map, Player.Client, FromTile.Position) );
            
            context.Response.Write(Player.Client.Connection, new ShowMagicEffect(FromTile.Position, MagicEffectType.Teleport) );
        }
    }
}
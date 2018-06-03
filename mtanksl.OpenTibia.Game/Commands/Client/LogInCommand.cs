using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Web;

namespace OpenTibia.Game.Commands
{
    public class LogInCommand : Command
    {
        private Server server;

        public LogInCommand(Server server)
        {
            this.server = server;
        }

        public Player Player { get; set; }

        public Position Position { get; set; }

        public override void Execute(Context context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(Position);

            //Act

            server.Map.AddCreature(Player);

            byte fromIndex = fromTile.AddContent(Player);

            server.QueueForExecution(Constants.PlayerPingSchedulerEvent(Player), 10000, context, new PingCommand(server) { Player = Player }, null);

            //Notify

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (observer != Player)
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                        uint removeId;

                        if (observer.Client.IsKnownCreature(Player.Id, out removeId) )
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(fromTile.Position, fromIndex, Player) )

                                .Write(observer.Client.Connection, new ShowMagicEffect(fromTile.Position, MagicEffectType.Teleport) );
                        }
                        else
                        {
                            context.Response.Write(observer.Client.Connection, new ThingAdd(fromTile.Position, fromIndex, removeId, Player) )

                                .Write(observer.Client.Connection, new ShowMagicEffect(fromTile.Position, MagicEffectType.Teleport) );
                        }
                    }
                }
            }

            context.Response.Write(Player.Client.Connection, new SendInfo(Player.Id, Player.CanReportBugs) )   
                
                .Write(Player.Client.Connection, new SetSpecialCondition(SpecialCondition.None) )            
                
                .Write(Player.Client.Connection, new SendStatus(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, 0, 0, Player.Soul, Player.Stamina) )            
                
                .Write(Player.Client.Connection, new SendSkills(10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0) )                

                .Write(Player.Client.Connection, new SetEnvironmentLight(Light.Day) )            
                
                .Write(Player.Client.Connection, new SendTiles(server.Map, Player.Client, fromTile.Position) )            
                
                .Write(Player.Client.Connection, new ShowMagicEffect(fromTile.Position, MagicEffectType.Teleport) );
        }
    }
}
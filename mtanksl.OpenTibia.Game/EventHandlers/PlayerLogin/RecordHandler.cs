using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class RecordHandler : EventHandlers.EventHandler<PlayerLoginEventArgs>
    {
        public override async Promise Handle(PlayerLoginEventArgs e)
        {
            uint count = (uint)Context.Server.GameObjects.GetPlayers().Count();

            if (count > Context.Server.Statistics.PlayersPeek)
            {
                Context.Server.Statistics.PlayersPeek = count;

                using (var database = Context.Server.DatabaseFactory.Create() )
                {
                    DbServerStorage playersPeek = await database.ServerStorageRepository.GetServerStorageByKey("PlayersPeek");

                    if (playersPeek == null)
                    {
                        playersPeek = new DbServerStorage()
                        {
                            Key = "PlayersPeek",

                            Value = Context.Server.Statistics.PlayersPeek.ToString()
                        };

                        database.ServerStorageRepository.AddServerStorage(playersPeek);
                    }
                    else
                    {
                        playersPeek.Value = Context.Server.Statistics.PlayersPeek.ToString();
                    }

                    await database.Commit();
                }

                foreach (var plugin in Context.Server.Plugins.GetServerRecordPlugins() )
                {
                    await plugin.OnRecord(count);
                }
            }
        }
    }    
}
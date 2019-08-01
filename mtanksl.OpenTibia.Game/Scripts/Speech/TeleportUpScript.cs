using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class TeleportUpScript : ISpeechScript
    {
        public void Start(Server server)
        {
            server.Scripts.SpeechScripts.Add("/up", this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnSpeech(Player player, string parameters, Server server, Context context)
        {
            Tile toTile = server.Map.GetTile( player.Tile.Position.Offset(0, 0, -1) );

            if (toTile != null)
            {
                new CreatureMoveCommand(player, toTile).Execute(server, context);

                return true;
            }

            return false;
        }
    }
}
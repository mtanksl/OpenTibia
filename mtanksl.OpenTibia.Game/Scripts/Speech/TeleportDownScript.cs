using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class TeleportDownScript : ISpeechScript
    {
        public void Start(Server server)
        {
            server.Scripts.SpeechScripts.Add("/down", this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnSpeech(Player player, string parameters, Context context)
        {
            Tile toTile = context.Server.Map.GetTile( player.Tile.Position.Offset(0, 0, 1) );

            if (toTile != null)
            {
                new CreatureMoveCommand(player, toTile).Execute(context);

                return true;
            }

            return false;
        }
    }
}
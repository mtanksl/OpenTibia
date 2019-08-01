using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class CreateItemScript : ISpeechScript
    {
        public void Start(Server server)
        {
            server.Scripts.SpeechScripts.Add("/i", this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnSpeech(Player player, string parameters, Server server, Context context)
        {
            ushort openTibiaId;

            if ( ushort.TryParse(parameters, out openTibiaId) )
            {
                new ItemCreateCommand(openTibiaId, player.Tile.Position.Offset(player.Direction) ).Execute(server, context);

                return true;
            }

            return false;
        }
    }
}
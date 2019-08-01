using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class CreateCreatureScript : ISpeechScript
    {
        public void Register(Server server)
        {
            server.Scripts.SpeechScripts.Add("/m", this);
        }

        public bool OnSpeech(Player player, string parameters, Server server, CommandContext context)
        {
            new CreatureCreateCommand(parameters, player.Tile.Position.Offset(player.Direction) ).Execute(server, context);

            return true;
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class CreateCreatureScript : SpeechScript
    {
        public override void Register(Server server)
        {
            server.SpeechScripts.Add("/m", this);
        }

        public override bool Execute(Player player, string parameters, Server server, CommandContext context)
        {
            new MonsterCreateCommand(parameters, player.Tile.Position.Offset(player.Direction) ).Execute(server, context);

            return true;
        }
    }
}
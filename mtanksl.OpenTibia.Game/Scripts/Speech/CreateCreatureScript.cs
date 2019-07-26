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

        public override bool Execute(Player player, string message, Server server, CommandContext context)
        {
            TileCreateMonsterCommand command = new TileCreateMonsterCommand(message, player.Tile.Position.Offset(player.Direction) );

            command.Execute(server, context);

            return true;
        }
    }
}
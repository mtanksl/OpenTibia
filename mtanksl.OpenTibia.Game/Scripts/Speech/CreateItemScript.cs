using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class CreateItemScript : SpeechScript
    {
        public override void Register(Server server)
        {
            server.SpeechScripts.Add("/i", this);
        }

        public override bool Execute(Player player, string message, Server server, CommandContext context)
        {
            ushort openTibiaId;

            if ( ushort.TryParse(message, out openTibiaId) )
            {
                TileCreateItemCommand command = new TileCreateItemCommand(openTibiaId, player.Tile.Position.Offset(player.Direction) );

                command.Execute(server, context);
            }

            return true;
        }
    }
}
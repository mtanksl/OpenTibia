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

        public override bool Execute(Player player, string parameters, Server server, CommandContext context)
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
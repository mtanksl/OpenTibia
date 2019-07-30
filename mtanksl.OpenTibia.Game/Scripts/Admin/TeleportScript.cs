using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class TeleportScript : ISpeechScript
    {
        public void Register(Server server)
        {
            server.SpeechScripts.Add("/a", this);
        }

        public bool Execute(Player player, string parameters, Server server, CommandContext context)
        {
            int count;

            if (int.TryParse(parameters, out count) && count > 0)
            {
                Position toPosition;

                switch (player.Direction)
                {
                    case Direction.East:

                        toPosition = new Position(player.Tile.Position.X + count, player.Tile.Position.Y, player.Tile.Position.Z);

                        break;

                    case Direction.North:

                        toPosition = new Position(player.Tile.Position.X, player.Tile.Position.Y - count, player.Tile.Position.Z);

                        break;

                    case Direction.West:

                        toPosition = new Position(player.Tile.Position.X - count, player.Tile.Position.Y, player.Tile.Position.Z);

                        break;

                    default:

                        toPosition = new Position(player.Tile.Position.X, player.Tile.Position.Y + count, player.Tile.Position.Z);

                        break;
                }

                Tile toTile = server.Map.GetTile(toPosition);

                if (toTile != null)
                {
                    new CreatureMoveCommand(player, toTile).Execute(server, context);

                    return true;
                }
            }

            return false;
        }
    }
}
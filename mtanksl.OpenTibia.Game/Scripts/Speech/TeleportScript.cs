using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class TeleportScript : ISpeechScript
    {
        public void Start(Server server)
        {
            server.Scripts.SpeechScripts.Add("/a", this);
        }

        public void Stop(Server server)
        {

        }

        public bool OnSpeech(Player player, string parameters, Context context)
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

                Tile toTile = context.Server.Map.GetTile(toPosition);

                if (toTile != null)
                {
                    new CreatureMoveCommand(player, toTile).Execute(context);

                    return true;
                }
            }

            return false;
        }
    }
}
﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Scripts.Speech
{
    public class TeleportUpScript : ISpeechScript
    {
        public void Register(Server server)
        {
            server.SpeechScripts.Add("/up", this);
        }

        public bool Execute(Player player, string parameters, Server server, CommandContext context)
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
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CommandsHandler : CommandHandler<PlayerSayCommand>
    {
        private readonly List<string> commands;

        private readonly string message;

        public CommandsHandler()
        {
            commands = new List<string>()
            {
                "/at n - Display nth animated text",
                "/a n - Jump n tiles",
                "/ban ip_address - Ban ip address",
                "/ban player_name - Ban player",
                "/ban account_name - Ban account",
                "/c player_name - Teleport player",
                "/down - Go down one floor",
                "/ghost - Invisible",
                "/goto player_name - Go to player",
                "/i item_id n - Create an item with n count",
                "/kick player_name - Kick player",
                "/me n - Display nth magic effect",
                "/m monster_name - Create a monster",
                "/n npc_name - Create a NPC",
                "/pe n - Display nth projectile effect",
                "/raid raid_name - Start raid",
                "/r - Destroy monster, NPC or item",
                "/t - Go to home town",
                "/t player_name - Teleport player to home town",
                "/town town_name - Go to town",
                "/unban ip_address - UnBan ip address",
                "/unban player_name - UnBan player",
                "/unban account_name - UnBan account",
                "/up - Go up one floor",
                "/w waypoint_name - Go to waypont",
                "#b message - Broadcast",
                "#c message - Channel broadcast",
                "#d message - Channel broadcast anonymous",
                "@player_name@ message - Send message to player"
            };

            message = string.Join("\n", commands);
        }

        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("!commands") )
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, command.Message) );

                foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                {
                    command.Player.Client.Windows.CloseWindow(pair.Key);
                }

                Window window = new Window();

                uint windowId = command.Player.Client.Windows.OpenWindow(window);

                Context.AddPacket(command.Player, new OpenShowOrEditTextDialogOutgoingPacket(windowId, 2819, (ushort)message.Length, message, null, null) );

                return Promise.Completed;
            }

            return next();
        }
    }
}
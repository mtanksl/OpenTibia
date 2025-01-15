using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class RulesHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("!rules") ) 
            {
                Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.PurpleDefault, command.Message) );

                foreach (var pair in command.Player.Client.Windows.GetIndexedWindows() )
                {
                    command.Player.Client.Windows.CloseWindow(pair.Key);
                }

                Window window = new Window();

                uint windowId = command.Player.Client.Windows.OpenWindow(window);

                Context.AddPacket(command.Player, new OpenShowOrEditTextDialogOutgoingPacket(windowId, 2819, (ushort)Context.Server.Config.Rules.Length, Context.Server.Config.Rules, null, null) );

                return Promise.Completed;
            }

            return next();
        }
    }
}
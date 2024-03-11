using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseCloseNpcsChannelCommand : Command
    {
        public ParseCloseNpcsChannelCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.Config.GamePrivateNpcSystem)
            {
                //TODO: onclosenpcchannel
            }
             
            return Promise.Completed;
        }
    }
}
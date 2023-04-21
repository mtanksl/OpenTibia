using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerDestroyCommand : Command
    {
        public PlayerDestroyCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.PlayerFactory.Detach(Player) )
            {
                if (Player.Health == 0)
                {
                    Context.AddPacket(Player.Client.Connection, new OpenYouAreDeathDialogOutgoingPacket() );

                    Context.AddEvent(new PlayerDeathEventArgs(Player) );
                }
                else
                {
                    Context.Disconnect(Player.Client.Connection);

                    Context.AddEvent(new PlayerLogoutEventArgs(Player) );
                }

                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.PlayerFactory.Destroy(Player);

                    return Context.AddCommand(new TileRemoveCreatureCommand(Player.Tile, Player) );
                } );
            }

            return Promise.Completed;
        }        
    }
}
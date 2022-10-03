using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Components
{
    public class CheckConnectionBehaviour : Behaviour
    {
        private Player player;

        public override void Start(Server server)
        {
            player = (Player)GameObject;

            server.QueueForExecution(ctx =>
            {
                Ping(ctx);
            } );
        }

        private void Ping(Context context)
        {
            context.AddCommand(new DelayCommand(Constants.PlayerCheckConnectionSchedulerEvent(player), Constants.PlayerCheckConnectionSchedulerEventInterval) ).Then(ctx =>
            {
                ctx.AddPacket(player.Client.Connection, new PingOutgoingPacket() );

            } ) .Then(ctx =>
            {
                Ping(ctx);
            } );
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(Constants.PlayerCheckConnectionSchedulerEvent(player) );
        } 
    }
}
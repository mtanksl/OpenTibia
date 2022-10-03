using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public class ItemDecayBehaviour : Behaviour
    {
        private int executeInMilliseconds;

        private ushort openTibiaId;
        
        private byte count;

        public ItemDecayBehaviour(int executeInMilliseconds, ushort openTibiaId, byte count)
        {
            this.executeInMilliseconds = executeInMilliseconds;

            this.openTibiaId = openTibiaId;

            this.count = count;
        }

        private Item item;

        public override void Start(Server server)
        {
            item = (Item)GameObject;

            server.QueueForExecution(Constants.ItemDecaySchedulerEvent(item), executeInMilliseconds, ctx =>
            {
                ctx.AddCommand(new ItemTransformCommand(item, openTibiaId, count) );
            } );
        }

        public override void Stop(Server server)
        {
            server.CancelQueueForExecution(Constants.ItemDecaySchedulerEvent(item) );
        }
    }
}
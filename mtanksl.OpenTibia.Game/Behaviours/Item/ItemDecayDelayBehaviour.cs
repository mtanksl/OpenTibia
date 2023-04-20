namespace OpenTibia.Game.Components
{
    public class ItemDecayDelayBehaviour : DelayBehaviour
    {
        public ItemDecayDelayBehaviour(int executeInMilliseconds) : base(executeInMilliseconds)
        {
            
        }

        public override bool IsUnique
        {
            get
            {
                return true;
            }
        }

        public override void Start(Server server)
        {
            base.Start(server);
        }

        public override void Stop(Server server)
        {
            base.Stop(server);
        }
    }
}
namespace OpenTibia.Game.Components
{
    public class ItemDecayDelayBehaviour : DelayBehaviour
    {
        public ItemDecayDelayBehaviour(int executeInMilliseconds) : base("ItemDecayBehaviour", executeInMilliseconds)
        {
            
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
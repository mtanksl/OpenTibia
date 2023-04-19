namespace OpenTibia.Game.Components
{
    public class ItemDecayDelayBehaviour : DelayBehaviour
    {
        public ItemDecayDelayBehaviour(int executeInMilliseconds) : base(executeInMilliseconds)
        {
            
        }

        public override void Start(Server server)
        {
            key = "ItemDecayDelayBehaviour" + GameObject.Id;

            base.Start(server);
        }

        public override void Stop(Server server)
        {
            base.Stop(server);
        }
    }
}
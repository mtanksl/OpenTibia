namespace OpenTibia.Game.Components
{
    public class PlayerActionDelayBehaviour : DelayBehaviour
    {
        public PlayerActionDelayBehaviour() : base("PlayerActionBehaviour", 200)
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
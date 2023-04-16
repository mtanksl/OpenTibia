namespace OpenTibia.Game.Components
{
    public class PlayerWalkDelayBehaviour : DelayBehaviour
    {
        public PlayerWalkDelayBehaviour(int executeInMilliseconds) : base("PlayerWalkBehaviour", executeInMilliseconds)
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
namespace OpenTibia.Game.Components
{
    public class PlayerWalkDelayBehaviour : DelayBehaviour
    {
        public PlayerWalkDelayBehaviour(int executeInMilliseconds) : base(executeInMilliseconds)
        {
            
        }

        public override void Start(Server server)
        {
            key = "PlayerWalkDelayBehaviour" + GameObject.Id;

            base.Start(server);
        }

        public override void Stop(Server server)
        {
            base.Stop(server);
        }
    }
}
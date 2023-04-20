namespace OpenTibia.Game.Components
{
    public class PlayerWalkDelayBehaviour : DelayBehaviour
    {
        public PlayerWalkDelayBehaviour(int executeInMilliseconds) : base(executeInMilliseconds)
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
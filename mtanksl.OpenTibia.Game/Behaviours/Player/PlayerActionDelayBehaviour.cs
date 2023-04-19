namespace OpenTibia.Game.Components
{
    public class PlayerActionDelayBehaviour : DelayBehaviour
    {
        public PlayerActionDelayBehaviour() : base(200)
        {
            
        }

        public override void Start(Server server)
        {
            key = "PlayerActionDelayBehaviour" + GameObject.Id;

            base.Start(server);
        }

        public override void Stop(Server server)
        {
            base.Stop(server);
        }
    }
}
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerActionDelayBehaviour : DelayBehaviour
    {
        public PlayerActionDelayBehaviour() : base(TimeSpan.FromMilliseconds(200) )
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
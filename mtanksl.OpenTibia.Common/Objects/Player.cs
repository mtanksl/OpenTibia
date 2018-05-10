namespace OpenTibia.Common.Objects
{
    public class Player : Creature
    {
        private IClient client;

        public IClient Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;

                if (client != null && client.Player != this)
                {
                    client.Player = this;
                }
            }
        }
    }
}
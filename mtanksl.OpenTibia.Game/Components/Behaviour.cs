using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public abstract class Behaviour : Component
    {
        public bool Enabled { get; set; }

        public abstract void Start(Server server);

        public abstract void Update(Server server, Context context);
    }
}
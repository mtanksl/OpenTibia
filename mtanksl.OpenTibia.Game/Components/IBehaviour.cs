using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public interface IBehaviour : IComponent
    {
        void Start(Server server);

        void Update(Server server, CommandContext context);
    }
}
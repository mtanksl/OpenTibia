using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Scripts
{
    public interface IItemRotateScript : IScript
    {
        bool OnItemRotate(Player player, Item item, Server server, Context context);
    }
}
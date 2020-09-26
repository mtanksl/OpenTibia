using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Scripts
{
    public interface IItemUseScript : IScript
    {
        bool OnItemUse(Player player, Item item, Context context);
    }
}
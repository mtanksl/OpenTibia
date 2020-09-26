using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Scripts
{
    public interface IItemUseWithItemScript : IScript
    {
        bool NextTo { get; }
        
        bool OnItemUseWithItem(Player player, Item item, Item toItem, Context context);
    }
}
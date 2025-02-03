using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class DesintegrateRunePlugin : RunePlugin
    {
        public DesintegrateRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            if (toTile == null || toTile.Ground == null || toTile.TopItem == null || toTile.TopItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) || !player.Tile.Position.IsNextTo(toTile.Position) || (toTile.TopItem is Container container && container.Count > 0) || toTile.TopItem.UniqueId > 0)
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile toTile, Item rune)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Puff) ).Then( () =>
            {
                return Context.AddCommand(new ItemDestroyCommand(toTile.TopItem) );
            } );
        }
    }
}

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

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            if (tile == null || tile.Ground == null || tile.TopItem == null || tile.TopItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) || !player.Tile.Position.IsNextTo(tile.Position) )
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            return Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.Puff) ).Then( () =>
            {
                return Context.AddCommand(new ItemDestroyCommand(tile.TopItem) );
            } );
        }
    }
}

using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.GameData.Plugins.Runes
{
    public class DestroyFieldRunePlugin : RunePlugin
    {
        public DestroyFieldRunePlugin(Rune rune) : base(rune)
        {

        }

        private HashSet<ushort> fields = new HashSet<ushort>() { 1492, 1493, 1494, 1495, 1496 };

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            if (tile == null || tile.Ground == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) || tile.TopItem == null || !fields.Contains(tile.TopItem.Metadata.OpenTibiaId) )
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

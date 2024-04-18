using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.GameData.Plugins.Runes
{
    public class DestroyFieldRunePlugin : RunePlugin
    {
        private static HashSet<ushort> fields = new HashSet<ushort>() { 1492, 1493, 1494, 1495, 1496 };

        public DestroyFieldRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            if (tile == null || tile.Ground == null || tile.TopItem == null || !fields.Contains(tile.TopItem.Metadata.OpenTibiaId) )
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

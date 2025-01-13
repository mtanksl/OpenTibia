using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.Runes
{
    public class DestroyFieldRunePlugin : RunePlugin
    {
        private readonly HashSet<ushort> fields;

        public DestroyFieldRunePlugin(Rune rune) : base(rune)
        {
            fields = Context.Server.Values.GetUInt16HashSet("values.items.fields");
        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            if (toTile == null || toTile.Ground == null || toTile.TopItem == null || !fields.Contains(toTile.TopItem.Metadata.OpenTibiaId) )
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

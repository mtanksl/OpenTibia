using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Plugins.Runes
{
    public class ChamaleonRunePlugin : RunePlugin
    {
        public ChamaleonRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile toTile, Item rune)
        {
            if (toTile == null || toTile.Ground == null || toTile.TopItem == null || toTile.TopItem.Metadata.Flags.Is(ItemMetadataFlags.NotMoveable) || !player.Tile.Position.IsNextTo(toTile.Position) )
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile toTile, Item rune)
        {
            ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByOpenTibiaId(toTile.TopItem.Metadata.OpenTibiaId);

            return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.BlueShimmer) ).Then( () =>
            {
                return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.BlueShimmer) );
                
            } ).Then( () =>
            {
                return Context.AddCommand(new CreatureAddConditionCommand(player, 
                            
                    new OutfitCondition(new Outfit(itemMetadata.TibiaId), new TimeSpan(0, 3, 20) ) ) );
            } );
        }
    }
}
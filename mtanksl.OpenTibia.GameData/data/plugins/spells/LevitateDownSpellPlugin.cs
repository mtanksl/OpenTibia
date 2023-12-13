using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class LevitateDownSpellPlugin : SpellPlugin
    {
        public LevitateDownSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override void Start()
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            Tile next = Context.Server.Map.GetTile(player.Tile.Position.Offset(player.Direction) );

            Tile toTile = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, 1).Offset(player.Direction) );

            if (next != null || toTile == null || toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            Tile toTile = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, 1).Offset(player.Direction) );

            return Context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.Teleport) ).Then( () =>
            {
                return Context.AddCommand(new CreatureWalkCommand(player, toTile) );

            } ).Then( () =>
            {
                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
            } );
        }
             
        public override void Stop()
        {
            
        }
    }
}
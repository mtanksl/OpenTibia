using OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System.Linq;

namespace OpenTibia.GameData.Plugins.Spells
{
    public class LevitateUpSpellPlugin : SpellPlugin
    {
        public LevitateUpSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override void Start()
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            Tile up = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, -1) );

            Tile toTile = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, -1).Offset(player.Direction) );

            if (up != null || toTile == null || toTile.Ground == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) )
            {
                return Promise.FromResult(false);
            }

            return Promise.FromResult(true);
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            Tile toTile = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 0, -1).Offset(player.Direction) );

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
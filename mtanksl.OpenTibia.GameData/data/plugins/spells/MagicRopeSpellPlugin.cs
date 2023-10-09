using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using System.Collections.Generic;

namespace mtanksl.OpenTibia.GameData.Plugins.Spells
{
    public class MagicRopeSpellPlugin : SpellPlugin
    {
        private static HashSet<ushort> ropeSpots = new HashSet<ushort> { 384, 418 };

        public MagicRopeSpellPlugin(Spell spell) : base(spell)
        {

        }

        public override void Start()
        {

        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message)
        {
            if (ropeSpots.Contains(player.Tile.Ground.Metadata.OpenTibiaId) )
            {
                return Promise.FromResult(true);
            }

            return Promise.FromResult(false);
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            Tile toTile = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 1, -1) );

            return Context.AddCommand(new ShowMagicEffectCommand(player.Tile.Position, MagicEffectType.Teleport) ).Then( () =>
            {
                return Context.AddCommand(new CreatureWalkCommand(player, toTile, Direction.South) );

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
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.GameData.Plugins.Spells
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
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnCast(Player player, Creature target, string message)
        {
            Tile toTile = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 1, -1) );

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Teleport) ).Then( () =>
            {
                return Context.AddCommand(new CreatureMoveCommand(player, toTile, Direction.South) );

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
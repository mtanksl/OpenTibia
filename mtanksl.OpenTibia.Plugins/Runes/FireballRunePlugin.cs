﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Plugins.Runes
{
    public class FireballRunePlugin : RunePlugin
    {
        public FireballRunePlugin(Rune rune) : base(rune)
        {

        }

        public override PromiseResult<bool> OnUsingRune(Player player, Creature target, Tile tile, Item item)
        {
            if (tile == null || tile.Ground == null || tile.NotWalkable)
            {
                return Promise.FromResultAsBooleanFalse;
            }

            return Promise.FromResultAsBooleanTrue;
        }

        public override Promise OnUseRune(Player player, Creature target, Tile tile, Item item)
        {
            Offset[] area = new Offset[]
            {
                                    new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2),
                new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1),
                new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),
                new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),
                                    new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2)
            };

            var formula = GenericFormula(player.Level, player.Skills.MagicLevel, 1.81, 10, 3, 18);

            return Context.AddCommand(new CreatureAttackAreaCommand(player, false, tile.Position, area, ProjectileType.Fire, MagicEffectType.FireArea,

                new SimpleAttack(null, null, AnimatedTextColor.Orange, formula.Min, formula.Max) ) );
        }
    }
}

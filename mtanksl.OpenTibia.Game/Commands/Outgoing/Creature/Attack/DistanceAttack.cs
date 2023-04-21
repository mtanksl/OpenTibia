﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class DistanceAttack : Attack
    {
        public DistanceAttack(ProjectileType projectileType)
        {
            ProjectileType = projectileType;
        }

        public ProjectileType ProjectileType { get; set; }

        public override int Calculate(Creature attacker, Creature target)
        {
            //TODO: Calculate distance damage

            return -Context.Current.Server.Randomization.Take(0, 30);
        }

        public override Promise Missed(Creature attacker, Creature target)
        {
            return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.Puff) );
        }

        public override Promise Hit(Creature attacker, Creature target, int damage)
        {
            return Context.Current.AddCommand(new ShowProjectileCommand(attacker.Tile.Position, target.Tile.Position, ProjectileType) ).Then( () =>
            {
                return Context.Current.AddCommand(new ShowMagicEffectCommand(target.Tile.Position, MagicEffectType.RedSpark) );

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new ShowAnimatedTextCommand(target.Tile.Position, AnimatedTextColor.DarkRed, (-damage).ToString() ) );

            } ).Then( () =>
            {
                return Context.Current.AddCommand(new CreatureUpdateHealthCommand(target, target.Health + damage) );
            } );
        }
    }
}
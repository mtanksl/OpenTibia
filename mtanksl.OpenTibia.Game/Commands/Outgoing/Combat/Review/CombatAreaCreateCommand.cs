using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class CombatAreaCreateCommand : Command
    {
        public CombatAreaCreateCommand(Creature attacker, Position position, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Func<Creature, Creature, int> formula)
        {
            Attacker = attacker;

            Position = position;

            Area = area;

            ProjectileType = projectileType;

            MagicEffectType = magicEffectType;

            OpenTibiaId = openTibiaId;

            Count = count;

            Formula = formula;
        }

        public Creature Attacker { get; set; }

        public Position Position { get; set; }

        public Offset[] Area { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public Func<Creature, Creature, int> Formula { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                if (ProjectileType != null)
                {
                    context.AddCommand(new ShowProjectileCommand(Attacker.Tile.Position, Position, ProjectileType.Value) );
                }

                foreach (var offset in Area)
                {
                    Position position = Position.Offset(offset);

                    if (MagicEffectType != null)
                    {
                        context.AddCommand(new ShowMagicEffectCommand(position, MagicEffectType.Value) );
                    }

                    Tile tile = context.Server.Map.GetTile(position);

                    if (tile != null)
                    {
                        foreach (var target in tile.GetMonsters().Concat<Creature>(tile.GetPlayers() ).ToList() )
                        {
                            int health = Formula(Attacker, target);

                            if (target != Attacker || health > 0)
                            {
                                context.AddCommand(new CombatChangeHealthCommand(Attacker, target, MagicEffectType.ToAnimatedTextColor(), health) );
                            }
                        }

                        if ( tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) )
                        {

                        }
                        else
                        {
                            context.AddCommand(new TileCreateItemCommand(tile, OpenTibiaId, Count) ).Then( (ctx, item) =>
                            {
                                return ctx.AddCommand(new ItemDecayDestroyCommand(item, 10000) );
                            } );
                        }
                    }
                }

                resolve(context);
            } );
        }
    }
}
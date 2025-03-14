﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;

namespace OpenTibia.Plugins.Spells
{
    public class MagicRopeSpellPlugin : SpellPlugin
    {
        private readonly HashSet<ushort> ropeSpots;

        public MagicRopeSpellPlugin(Spell spell) : base(spell)
        {
            ropeSpots = Context.Server.Values.GetUInt16HashSet("values.items.ropeSpots");
        }

        public override PromiseResult<bool> OnCasting(Player player, Creature target, string message, string parameter)
        {
            if (ropeSpots.Contains(player.Tile.Ground.Metadata.OpenTibiaId) )
            {
                return Promise.FromResultAsBooleanTrue;
            }

            return Promise.FromResultAsBooleanFalse;
        }

        public override Promise OnCast(Player player, Creature target, string message, string parameter)
        {
            Tile toTile = Context.Server.Map.GetTile(player.Tile.Position.Offset(0, 1, -1) );

            return Context.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Teleport) ).Then( () =>
            {
                return Context.AddCommand(new CreatureMoveCommand(player, toTile) );

            } ).Then( () =>
            {
                return Context.AddCommand(new CreatureUpdateDirectionCommand(player, Direction.South) );

            } ).Then( () =>
            {
                return Context.AddCommand(new ShowMagicEffectCommand(toTile.Position, MagicEffectType.Teleport) );
            } );
        }
    }
}
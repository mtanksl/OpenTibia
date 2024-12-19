﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateHealthCommand : Command
    {
        public CreatureUpdateHealthCommand(Creature creature, int health) : this(creature, health, creature.MaxHealth)
        {
            
        }

        public CreatureUpdateHealthCommand(Creature creature, int health, int maxHealth)
        {
            Creature = creature;

            Health = (ushort)Math.Max(0, Math.Min(creature.MaxHealth, health) );

            MaxHealth = (ushort)Math.Max(0, Math.Min(creature.MaxHealth, maxHealth) );
        }

        public Creature Creature { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public override Promise Execute()
        {
            if ( !(Creature is Npc || (Creature is Player player && (player.Rank == Rank.Gamemaster || player.Rank == Rank.AccountManager) ) ) )
            {
                if (Creature.Health != Health || Creature.MaxHealth != MaxHealth)
                {
                    Creature.Health = Health;

                    Creature.MaxHealth = MaxHealth;

                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                    {
                        if (observer == Creature)
                        {
                            Context.AddPacket(observer, new SendStatusOutgoingPacket(observer.Health, observer.MaxHealth, observer.Capacity, observer.Experience, observer.Level, observer.LevelPercent, observer.Mana, observer.MaxMana, observer.Skills.MagicLevel, observer.Skills.MagicLevelPercent, observer.Soul, observer.Stamina) );
                        }

                        byte clientIndex;

                        if (observer.Client.TryGetIndex(Creature, out clientIndex) )
                        {
                            Context.AddPacket(observer, new SetHealthOutgoingPacket(Creature.Id, Creature.HealthPercentage) );
                        }
                    }

                    Context.AddEvent(new CreatureUpdateHealthEventArgs(Creature, Health) );

                    if (Creature.Health == 0)
                    {
                        return Context.AddCommand(new CreatureDestroyCommand(Creature) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}
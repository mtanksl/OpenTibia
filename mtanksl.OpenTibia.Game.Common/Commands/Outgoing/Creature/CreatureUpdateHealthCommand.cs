using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateHealthCommand : Command
    {
        public CreatureUpdateHealthCommand(Creature creature, int health)
        {
            Creature = creature;

            Health = (ushort)Math.Max(0, Math.Min(creature.MaxHealth, health) );
        }

        public Creature Creature { get; set; }

        public ushort Health { get; set; }

        public override Promise Execute()
        {
            if ( !(Creature is Npc || (Creature is Player player && (player.Rank == Rank.Gamemaster || player.Rank == Rank.AccountManager) ) || Creature.IsDestroyed) )
            {
                if (Creature.Health != Health)
                {
                    Creature.Health = Health;

                    foreach (var observer in Context.Server.Map.GetObserversOfTypePlayer(Creature.Tile.Position) )
                    {
                        if (observer == Creature)
                        {
                            Context.AddPacket(observer, new SendStatusOutgoingPacket(
                                observer.Health, observer.MaxHealth,
                                observer.Capacity, observer.MaxCapacity,
                                observer.Experience, observer.Level, observer.LevelPercent,
                                observer.Mana, observer.MaxMana,
                                observer.Skills.GetClientSkillLevel(Skill.MagicLevel), observer.Skills.GetSkillLevel(Skill.MagicLevel), observer.Skills.GetSkillPercent(Skill.MagicLevel),
                                observer.Soul,
                                observer.Stamina,
                                observer.BaseSpeed) );
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
                        return Context.AddCommand(new CreatureDeathCommand(Creature) );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}
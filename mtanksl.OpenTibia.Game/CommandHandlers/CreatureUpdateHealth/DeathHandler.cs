using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class DeathHandler : CommandHandler<CreatureUpdateHealthCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureUpdateHealthCommand command)
        {
            return next().Then( () =>
            {
                if (command.Creature.Health == 0)
                {
                    switch (command.Creature) 
                    {
                        case Monster monster:

                            return Context.AddCommand(new MonsterDestroyCommand(monster) );

                        case Npc npc:

                            return Context.AddCommand(new NpcDestroyCommand(npc) );

                        case Player player:

                            return Context.AddCommand(new PlayerDestroyCommand(player) );
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class ExperienceHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature.Health == 0)
            {
                return next().Then( () =>
                {
                    if (command.Creature is Monster monster && monster.Metadata.Experience > 0)
                    {
                        //TODO: Display experience on player per player per attack percentage

                        return Context.Current.AddCommand(new ShowAnimatedTextCommand(monster, AnimatedTextColor.White, (Context.Server.Config.GameplayExperienceRate * monster.Metadata.Experience).ToString() ) );
                    }

                    return Promise.Completed;
                } );
            }

            return next();            
        }
    }
}
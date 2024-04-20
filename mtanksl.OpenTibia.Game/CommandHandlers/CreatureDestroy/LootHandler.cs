using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class LootHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            if (command.Creature.Health == 0)
            {
                return next().Then( () =>
                {
                    if (command.Creature is Monster monster)
                    {
                        return Context.AddCommand(new TileCreateMonsterCorpseCommand(command.Creature.Tile, monster.Metadata) ).Then( (item) =>
                        {                                    
                            _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(30) ) );

                            return Promise.Completed;
                        } );
                    }
                    else if (command.Creature is Player player)
                    {
                        return Context.AddCommand(new TileCreateItemCommand(command.Creature.Tile, 2317, 1) ).Then( (item) =>
                        {
                            _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(30) ) );

                            return Promise.Completed;
                        } );
                    }

                    return Promise.Completed;
                } );
            }

            return next();            
        }
    }
}
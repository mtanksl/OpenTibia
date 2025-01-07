using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreatureDestroyLootHandler : CommandHandler<CreatureDestroyCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureDestroyCommand command)
        {
            return next().Then( () =>
            {
                if (command.Creature.Health == 0)
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
                        return Context.AddCommand(new TileCreatePlayerCorpseCommand(command.Creature.Tile, player) ).Then( (item) =>
                        {
                            _ = Context.AddCommand(new ItemDecayDestroyCommand(item, TimeSpan.FromSeconds(30) ) );

                            return Promise.Completed;
                        } );
                    }
                }

                return Promise.Completed;
            } );
        }
    }
}
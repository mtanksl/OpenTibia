using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class CreatureDestroyCommand : Command
    {
        public CreatureDestroyCommand(Creature creature)
        {
            Creature = creature;
        }

        public Creature Creature { get; set; }

        public override Promise Execute()
        {
            switch (Creature)
            {
                case Monster monster:

                    if (Context.Server.MonsterFactory.Detach(monster) )
                    {
                        Context.Server.QueueForExecution( () =>
                        {
                            Context.Server.MonsterFactory.ClearComponentsAndEventHandlers(monster);

                            return Context.AddCommand(new TileRemoveCreatureCommand(monster.Tile, monster) );
                        } );
                    }

                    break;

                case Npc npc: 
                    
                    if (Context.Server.NpcFactory.Detach(npc) )
                    {
                        Context.Server.QueueForExecution( () =>
                        {
                            Context.Server.NpcFactory.ClearComponentsAndEventHandlers(npc);

                            return Context.AddCommand(new TileRemoveCreatureCommand(npc.Tile, npc) );
                        } );
                    }

                    break;

                case Player player:

                    if (Context.Server.PlayerFactory.Detach(player) )
                    {
                        Context.Server.QueueForExecution( () =>
                        {
                            Context.Server.PlayerFactory.ClearComponentsAndEventHandlers(player);

                            return Context.AddCommand(new PlayerLogoutCommand(player) ).Then( () =>
                            {
                                return Context.AddCommand(new TileRemoveCreatureCommand(player.Tile, player) );
                            } );
                        } );
                    }

                    break;
            }

            return Promise.Completed;
        }
    }
}
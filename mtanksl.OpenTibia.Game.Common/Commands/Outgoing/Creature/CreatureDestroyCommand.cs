using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class CreatureDestroyCommand : Command
    {
        public CreatureDestroyCommand(Creature creature) : this(creature, false)
        {

        }

        public CreatureDestroyCommand(Creature creature, bool death)
        {
            Creature = creature;

            Death = death;
        }

        public Creature Creature { get; set; }

        public bool Death { get; set; }

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
                        return Context.AddCommand(new PlayerLogoutCommand(player, Death) ).Then( () =>
                        {
                            Context.Server.QueueForExecution( () =>
                            {
                                Context.Server.PlayerFactory.ClearComponentsAndEventHandlers(player);
                                                        
                                return Context.AddCommand(new TileRemoveCreatureCommand(player.Tile, player) );;
                            } );

                            return Promise.Completed;
                        } );
                    }

                    break;
            }

            return Promise.Completed;
        }
    }
}
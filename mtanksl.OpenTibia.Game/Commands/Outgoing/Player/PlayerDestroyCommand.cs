using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerDestroyCommand : Command
    {
        public PlayerDestroyCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.PlayerFactory.Detach(Player) )
            {
                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.PlayerFactory.ClearComponentsAndEventHandlers(Player);

                    return Context.AddCommand(new PlayerLogoutCommand(Player) ).Then( () =>
                    {
                        return Context.AddCommand(new TileRemoveCreatureCommand(Player.Tile, Player) );
                    } );
                } );
            }

            return Promise.Completed;
        }
    }
}
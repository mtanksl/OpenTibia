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
            Tile fromTile = Player.Tile;

            return Context.AddCommand(new TileRemoveCreatureCommand(fromTile, Player) ).Then( () =>
            {
                //TODO: Save to database

                Context.Server.PlayerFactory.Destroy(Player);

                Context.Disconnect(Player.Client.Connection);

                Context.Server.Logger.WriteLine(Player.Name + " disconneced.", LogLevel.Information);

                return Promise.Completed;
            } );
        }
    }
}
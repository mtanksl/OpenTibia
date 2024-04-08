using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveCreatureCommand : Command
    {
        public PlayerMoveCreatureCommand(IncomingCommand source, Player player, Creature creature, Tile toTile)
        {
            Source = source;

            Player = player;

            Creature = creature;

            ToTile = toTile;
        }

        public IncomingCommand Source { get; set; }

        public Player Player { get; set; }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        public override Promise Execute()
        {
            return Context.AddCommand(new CreatureMoveCommand(Creature, ToTile) );
        }
    }
}
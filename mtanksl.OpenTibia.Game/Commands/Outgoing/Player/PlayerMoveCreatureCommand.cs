using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerMoveCreatureCommand : Command
    {
        public PlayerMoveCreatureCommand(Player player, Creature item, Tile toTile)
        {
            Player = player;

            Creature = item;

            ToTile = toTile;
        }

        public Player Player { get; set; }

        public Creature Creature { get; set; }

        public Tile ToTile { get; set; }

        public override Promise Execute()
        {
            return Promise.Run( (resolve, reject) =>
            {
                context.AddCommand(new CreatureUpdateParentCommand(Creature, ToTile) );

                resolve(context);
            } );
        }
    }
}
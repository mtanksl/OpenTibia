using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public abstract class LookCommand : Command
    {
        public LookCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected void LookAtItem(Context context, Item item)
        {
            context.AddCommand(new PlayerLookAtItemCommand(Player, item), ctx =>
            {
                OnComplete(ctx);
            } );
        }

        protected void LookAtCreature(Context context, Creature creature)
        {
            context.AddCommand(new PlayerLookAtCreatureCommand(Player, creature), ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}
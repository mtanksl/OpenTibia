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
            context.AddCommand(new PlayerLookItemCommand(Player, item) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }

        protected void LookAtCreature(Context context, Creature creature)
        {
            context.AddCommand(new PlayerLookCreatureCommand(Player, creature) ).Then(ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}
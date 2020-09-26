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

        protected void LookAtItem(Item item, Context context)
        {
            Command command = context.AddCommand(new PlayerLookAtItemCommand(Player, item) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }

        protected void LookAtCreature(Creature creature, Context context)
        {
            Command command = context.AddCommand(new PlayerLookAtCreatureCommand(Player, creature) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}
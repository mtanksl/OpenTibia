using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseLogOutCommand : Command
    {
        public ParseLogOutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            return Context.AddCommand(new ShowMagicEffectCommand(Player, MagicEffectType.Puff) ).Then( () =>
            {
                return Context.AddCommand(new CreatureDestroyCommand(Player) );
            } );
        }
    }
}
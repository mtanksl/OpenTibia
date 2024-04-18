using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseSelectedOutfitCommand : IncomingCommand
    {
        public ParseSelectedOutfitCommand(Player player, Outfit outfit)
        {
            Player = player;

            Outfit = outfit;
        }

        public Player Player { get; set; }

        public Outfit Outfit { get; set; }

        public override Promise Execute()
        {
            return Context.AddCommand(new CreatureUpdateOutfitCommand(Player, Outfit, Outfit) );
        }
    }
}
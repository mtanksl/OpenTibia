using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseSelectedOutfitCommand : Command
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
            bool isInvisible = (Player.Outfit == Outfit.Invisible);

            bool isSwimming = (Player.Outfit == Outfit.Swimming);

            return Context.AddCommand(new CreatureUpdateOutfitCommand(Player, Outfit, (isInvisible || isSwimming) ? Player.Outfit : Outfit) );
        }
    }
}
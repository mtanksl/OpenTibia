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
            return context.AddCommand(new CreatureUpdateOutfit(Player, Outfit) );
        }
    }
}
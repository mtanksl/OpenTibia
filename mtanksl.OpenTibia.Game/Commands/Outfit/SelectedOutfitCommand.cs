using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class SelectedOutfitCommand : Command
    {
        public SelectedOutfitCommand(Player player, Outfit outfit)
        {
            Player = player;

            Outfit = outfit;
        }

        public Player Player { get; set; }

        public Outfit Outfit { get; set; }

        public override void Execute(Context context)
        {
            Command command = context.TransformCommand(new CreatureUpdateOutfitCommand(Player, Outfit) );

            command.Completed += (s, e) =>
            {
                base.OnCompleted(context);
            };

            command.Execute(context);
        }
    }
}
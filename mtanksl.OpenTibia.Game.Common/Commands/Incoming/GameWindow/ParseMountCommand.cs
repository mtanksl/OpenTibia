using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseMountCommand : IncomingCommand
    {
        public ParseMountCommand(Player player, bool isMounted)
        {
            Player = player;

            IsMounted = isMounted;
        }

        public Player Player { get; set; }

        public bool IsMounted { get; set; }

        public override Promise Execute()
        {
            if ( !Context.Server.Config.GameplayAllowChangeOutfit)
            {
                return Promise.Break;
            }

            return Context.AddCommand(new CreatureUpdateOutfitCommand(Player, Player.BaseOutfit, Player.ConditionOutfit, Player.Swimming, Player.ConditionStealth, Player.ItemStealth, IsMounted) );
        }
    }
}
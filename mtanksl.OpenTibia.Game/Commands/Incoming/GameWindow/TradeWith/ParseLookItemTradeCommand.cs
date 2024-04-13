using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseLookItemTradeCommand : IncomingCommand
    {
        public ParseLookItemTradeCommand(Player player, byte windowId, byte index)
        {
            Player = player;

            WindowId = windowId;

            Index = index;
        }

        public Player Player { get; set; }

        public byte WindowId { get; set; }

        public byte Index { get; set; }

        public override Promise Execute()
        {
            Trading trading = Context.Server.Tradings.GetTradingByOfferPlayer(Player);

            if (trading != null)
            {
                if (WindowId == 0)
                {
                    return Context.AddCommand(new PlayerLookItemCommand(Player, trading.OfferIncludes[Index] ) );
                }
                else
                {
                    return Context.AddCommand(new PlayerLookItemCommand(Player, trading.CounterOfferIncludes[Index] ) );
                }
            }
            else
            {
                trading = Context.Server.Tradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    if (WindowId == 0)
                    {
                        return Context.AddCommand(new PlayerLookItemCommand(Player, trading.CounterOfferIncludes[Index] ) );
                    }
                    else
                    {
                        return Context.AddCommand(new PlayerLookItemCommand(Player, trading.OfferIncludes[Index] ) );
                    }
                }
            }

            return Promise.Break;
        }
    }
}
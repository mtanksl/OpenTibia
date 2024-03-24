using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class ParseAcceptTradeCommand : Command
    {
        public ParseAcceptTradeCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            Trading trading = Context.Server.Tradings.GetTradingByOfferPlayer(Player);

            if (trading != null)
            {
                trading.OfferPlayerAccepted = true;

                if (trading.OfferPlayerAccepted && trading.CounterOfferPlayerAccepted)
                {
                    Context.AddPacket(trading.OfferPlayer, new CloseTradeOutgoingPacket() );

                    Context.AddPacket(trading.CounterOfferPlayer, new CloseTradeOutgoingPacket() );

                    Context.Server.Tradings.RemoveTrading(trading);

                    return Context.AddCommand(new ItemTradeCommand(trading) );
                }
            }
            else
            {
                trading = Context.Server.Tradings.GetTradingByCounterOfferPlayer(Player);

                if (trading != null)
                {
                    trading.CounterOfferPlayerAccepted = true;

                    if (trading.OfferPlayerAccepted && trading.CounterOfferPlayerAccepted)
                    {
                        Context.AddPacket(trading.OfferPlayer, new CloseTradeOutgoingPacket() );

                        Context.AddPacket(trading.CounterOfferPlayer, new CloseTradeOutgoingPacket() );

                        Context.Server.Tradings.RemoveTrading(trading);

                        return Context.AddCommand(new ItemTradeCommand(trading) );
                    }
                }
            }

            return Promise.Break;
        }
    }
}
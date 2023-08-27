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
                    //TODO: Move items

                    Context.AddPacket(trading.OfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                    Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                    Context.Server.Tradings.RemoveTrading(trading);

                    return Promise.Completed;
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
                        //TODO: Move items

                        Context.AddPacket(trading.OfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                        Context.AddPacket(trading.CounterOfferPlayer.Client.Connection, new CloseTradeOutgoingPacket() );

                        Context.Server.Tradings.RemoveTrading(trading);

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}
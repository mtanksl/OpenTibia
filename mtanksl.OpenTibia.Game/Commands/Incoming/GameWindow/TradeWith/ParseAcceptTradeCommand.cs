using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseAcceptTradeCommand : IncomingCommand
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

                    return Swap(trading);
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

                        return Swap(trading);
                    }
                }
            }

            return Promise.Break;
        }

        private Promise Swap(Trading trading)
        {
            List<Promise> promises = new List<Promise>();

            switch (trading.Offer.Parent)
            {
                case Tile fromTile:

                    promises.Add(Context.AddCommand(new TileRemoveItemCommand(fromTile, trading.Offer) ) );

                    break;

                case Inventory fromInventory:

                    promises.Add(Context.AddCommand(new InventoryRemoveItemCommand(fromInventory, trading.Offer) ) );

                    break;

                case Container fromContainer:

                    promises.Add(Context.AddCommand(new ContainerRemoveItemCommand(fromContainer, trading.Offer) ) );

                    break;

                default:

                    throw new NotImplementedException();
            }

            switch (trading.CounterOffer.Parent)
            {
                case Tile fromTile:

                    promises.Add(Context.AddCommand(new TileRemoveItemCommand(fromTile, trading.CounterOffer) ) );

                    break;

                case Inventory fromInventory:

                    promises.Add(Context.AddCommand(new InventoryRemoveItemCommand(fromInventory, trading.CounterOffer) ) );

                    break;

                case Container fromContainer:

                    promises.Add(Context.AddCommand(new ContainerRemoveItemCommand(fromContainer, trading.CounterOffer) ) );

                    break;

                default:

                    throw new NotImplementedException();
            }

            promises.Add(Context.AddCommand(new PlayerAddItemCommand(trading.OfferPlayer, trading.CounterOffer) ) );

            promises.Add(Context.AddCommand(new PlayerAddItemCommand(trading.CounterOfferPlayer, trading.Offer) ) );

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}
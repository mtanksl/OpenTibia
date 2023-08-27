using OpenTibia.Common.Objects;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ItemTradeCommand : Command
    {
        private Trading trading;

        public ItemTradeCommand(Trading trading)
        {
            this.trading = trading;
        }

        public override Promise Execute()
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

            promises.Add(Context.AddCommand(new PlayerInventoryContainerTileAddItemCommand(trading.OfferPlayer, trading.CounterOffer) ) );

            promises.Add(Context.AddCommand(new PlayerInventoryContainerTileAddItemCommand(trading.CounterOfferPlayer, trading.Offer) ) );

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}
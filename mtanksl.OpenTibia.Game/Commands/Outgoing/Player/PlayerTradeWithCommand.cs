using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class PlayerTradeWithCommand : Command
    {
        public PlayerTradeWithCommand(Player player, Item item, Player toPlayer)
        {
            Player = player;

            Item = item;

            ToPlayer = toPlayer;
        }

        public Player Player { get; }

        public Item Item { get; }

        public Player ToPlayer { get; }

        public override Promise Execute()
        {
            bool CanTrade(Item item, List<Item> items)
            {
                Trading trading = Context.Server.Tradings.GetTradingByOffer(item) ?? Context.Server.Tradings.GetTradingByCounterOffer(item);

                if (trading != null)
                {
                    return false;
                }

                items.Add(item);

                if (item is Container container)
                {
                    foreach (var child in container.GetItems() )
                    {
                        if ( !CanTrade(child, items) )
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            //TODO: If trading a container, check if items were added

            //TODO: Check if items where refreshed

            if ( !Player.Tile.Position.IsInRange(ToPlayer.Tile.Position, 2) )
            {
                Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, ToPlayer.Name + " tells you to move closer.") );

                return Promise.Break;
            }

            Trading trading = Context.Server.Tradings.GetTradingByOfferPlayer(Player);

            if (trading != null)
            {
                Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreAlreadyTrading) );

                return Promise.Break;
            }
           
            trading = Context.Server.Tradings.GetTradingByOfferPlayer(ToPlayer);

            if (trading == null)
            {
                List<Item> items = new List<Item>();

                if ( !CanTrade(Item, items) )
                {
                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisItemIsAlreadyBeingTraded) );

                    return Promise.Break;                    
                }

                if (items.Count > 100)
                {
                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTradeMoreThan100Items) );

                    return Promise.Break;
                }

                trading = new Trading()
                {
                    OfferPlayer = Player,

                    Offer = Item,

                    OfferIncludes = items,

                    CounterOfferPlayer = ToPlayer
                };

                Context.Server.Tradings.AddTrading(trading);

                Context.AddPacket(Player.Client.Connection, new InviteTradeOutgoingPacket(trading.OfferPlayer.Name, trading.OfferIncludes) );

                Context.AddPacket(ToPlayer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " wants to trade with you.") );

                return Promise.Completed;
            }
            else
            {
                if (trading.CounterOfferPlayer != Player)
                {
                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisPlayerIsAlreadyTrading) );

                    return Promise.Break;
                }
                
                List<Item> items = new List<Item>();

                if ( !CanTrade(Item, items) )
                {
                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisItemIsAlreadyBeingTraded) );

                    return Promise.Break;                    
                }

                if (items.Count > 100)
                {
                    Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotTradeMoreThan100Items) );

                    return Promise.Break;
                }

                trading.CounterOffer = Item;

                trading.CounterOfferIncludes = items;

                Context.AddPacket(ToPlayer.Client.Connection, new JoinTradeOutgoingPacket(trading.CounterOfferPlayer.Name, trading.CounterOfferIncludes) );

                Context.AddPacket(Player.Client.Connection, new InviteTradeOutgoingPacket(trading.CounterOfferPlayer.Name, trading.CounterOfferIncludes) );

                Context.AddPacket(Player.Client.Connection, new JoinTradeOutgoingPacket(trading.OfferPlayer.Name, trading.OfferIncludes) );
                        
                return Promise.Completed;                
            }
        }
    }
}
using OpenTibia.Common.Events;
using System;

namespace OpenTibia.Game
{
    public class EventsCollection
    {
        public EventHandler<LoginEventArgs> Login;

        public EventHandler<LogoutEventArgs> Logout;

        public EventHandler<TileAddCreatureEventArgs> TileAddCreature;

        public EventHandler<TileRemoveCreatureEventArgs> TileRemoveCreature;

        public EventHandler<TileAddItemEventArgs> TileAddItem;

        public EventHandler<TileRemoveItemEventArgs> TileRemoveItem;

        public EventHandler<InventoryAddItemEventArgs> InventoryAddItem;

        public EventHandler<InventoryRemoveItemEventArgs> InventoryRemoveItem;
    }
}
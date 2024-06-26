﻿using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerMoveItemPlugin : Plugin
    {
        public abstract PromiseResult<bool> OnMoveItem(Player player, Item item, IContainer toContainer, byte toIndex, byte count);
    }
}
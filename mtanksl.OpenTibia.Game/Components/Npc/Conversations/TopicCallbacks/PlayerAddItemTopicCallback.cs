using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerAddItem : TopicCallback
    {
        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            ushort openTibiaId = (ushort)(int)conversation.Data["Type"];

            byte count = conversation.Data.ContainsKey("Data") ? (byte)(int)conversation.Data["Data"] : (byte)1;

            int amount = (int)conversation.Data["Amount"];

            ItemMetadata itemMetadata = Context.Current.Server.ItemFactory.GetItemMetadata(openTibiaId);

            if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                amount = amount * count;

                while (amount > 0)
                {
                    byte _count = (byte)Math.Min(100, amount);

                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, openTibiaId, _count) );

                    amount -= _count;
                }
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, openTibiaId, count) );
                }
            }
        }
    }
}
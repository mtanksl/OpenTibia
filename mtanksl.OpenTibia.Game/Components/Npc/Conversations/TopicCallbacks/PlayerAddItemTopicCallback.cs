using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components.Conversations
{
    public class PlayerAddItem : TopicCallback
    {
        public override async Promise Handle(Conversation conversation, Npc npc, Player player)
        {
            ushort openTibiaId = (ushort)(int)conversation.Data["Type"];

            int amount = (int)conversation.Data["Amount"];

            ItemMetadata itemMetadata = Context.Current.Server.ItemFactory.GetItemMetadata(openTibiaId);

            if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                while (amount > 0)
                {
                    if (amount > 100)
                    {
                        await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, openTibiaId, 100) );

                        amount -= 100;
                    }
                    else
                    {
                        await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, openTibiaId, (byte)amount) );

                        amount = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    await Context.Current.AddCommand(new PlayerInventoryContainerTileCreateItem(player, openTibiaId, 1) );
                }
            }
        }
    }
}
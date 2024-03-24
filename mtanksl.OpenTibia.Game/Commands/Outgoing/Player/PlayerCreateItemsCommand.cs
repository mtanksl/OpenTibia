using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerCreateItemsCommand : Command
    {
        public PlayerCreateItemsCommand(Player player, ushort openTibiaId, byte type, int count)
        {
            Player = player;

            OpenTibiaId = openTibiaId;

            Type = type;

            Count = count;
        }

        public Player Player { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Type { get; set; }

        public int Count { get; set; }

        public override async Promise Execute()
        {
            ItemMetadata itemMetadata = Context.Server.ItemFactory.GetItemMetadataByOpenTibiaId(OpenTibiaId);

            if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                while (Count > 0)
                {
                    byte stack = (byte)Math.Min(100, Count);

                    await Context.AddCommand(new PlayerCreateItemCommand(Player, OpenTibiaId, stack) );

                    Count -= stack;
                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    await Context.AddCommand(new PlayerCreateItemCommand(Player, OpenTibiaId, Type) );
                }
            }            
        }
    }
}
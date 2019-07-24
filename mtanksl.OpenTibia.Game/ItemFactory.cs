using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Xml.Items;
using System.Collections.Generic;
using System.Linq;
using Item = OpenTibia.Common.Objects.Item;
using ItemFlags = OpenTibia.FileFormats.Dat.ItemFlags;

namespace OpenTibia.Game
{
    public class ItemFactory
    {
        public ItemFactory(OtbFile otbFile, DatFile datFile, ItemsFile itemsFile)
        {
            metadatas = new Dictionary<ushort, ItemMetadata>(datFile.Items.Count);

            foreach (var otbItem in otbFile.Items)
            {
                if (otbItem.Group != ItemGroup.Deprecated)
                {
                    metadatas.Add(otbItem.OpenTibiaId, new ItemMetadata()
                    {
                        TibiaId = otbItem.TibiaId,

                        OpenTibiaId = otbItem.OpenTibiaId
                    } );
                }
            }

            var lookup = otbFile.Items.ToLookup(otbItem => otbItem.TibiaId, otbItem => otbItem.OpenTibiaId);

            foreach (var datItem in datFile.Items)
            {
                foreach (var openTibiaId in lookup[datItem.TibiaId] )
                {
                    ItemMetadata metadata = metadatas[openTibiaId];

                    if (datItem.Flags.Is(ItemFlags.IsGround) )
                    {
                        metadata.TopOrder = TopOrder.Ground;
                    }
                    else if (datItem.Flags.Is(ItemFlags.AlwaysOnTop1) )
                    {
                        metadata.TopOrder = TopOrder.HighPriority;
                    }
                    else if (datItem.Flags.Is(ItemFlags.AlwaysOnTop2) )
                    {
                        metadata.TopOrder = TopOrder.MediumPriority;
                    }
                    else if (datItem.Flags.Is(ItemFlags.AlwaysOnTop3) )
                    {
                        metadata.TopOrder = TopOrder.LowPriority;
                    }
                    else
                    {
                        metadata.TopOrder = TopOrder.Other;
                    }

                    metadata.Flags = (ItemMetadataFlags)datItem.Flags;

                    metadata.Speed = datItem.Speed;

                    metadata.Light = new Light( (byte)datItem.LightLevel, (byte)datItem.LightColor );
                }
            }

            foreach (var xmlItem in itemsFile.Items)
            {
                if (xmlItem.OpenTibiaId < 20000)
                {
                    ItemMetadata metadata = metadatas[xmlItem.OpenTibiaId];

                    metadata.Name = xmlItem.Name;

                    metadata.Capacity = xmlItem.ContainerSize;
                }
            }
        }

        private Dictionary<ushort, ItemMetadata> metadatas;

        public Item Create(ushort openTibiaId)
        {
            ItemMetadata metadata;

            if ( !metadatas.TryGetValue(openTibiaId, out metadata) )
            {
                return null;
            }

            if (metadata.Flags.Is(ItemMetadataFlags.IsContainer) )
            {
                return new Container(metadata);
            }
            else if (metadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                return new StackableItem(metadata);
            }

            return new Item(metadata);
        }
    }
}
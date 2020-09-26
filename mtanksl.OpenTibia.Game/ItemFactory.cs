using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Xml.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Item = OpenTibia.Common.Objects.Item;
using ItemFlags = OpenTibia.FileFormats.Dat.ItemFlags;

namespace OpenTibia.Game
{
    public class ItemFactory
    {
        private GameObjectCollection gameObjectCollection;

        public ItemFactory(GameObjectCollection gameObjectCollection, OtbFile otbFile, DatFile datFile, ItemsFile itemsFile)
        {
            this.gameObjectCollection = gameObjectCollection;

            metadatas = new Dictionary<ushort, ItemMetadata>(datFile.Items.Count);

            foreach (var otbItem in otbFile.Items)
            {
                if (otbItem.Group != ItemGroup.Deprecated)
                {
                    ItemMetadata metadata = new ItemMetadata()
                    {
                        TibiaId = otbItem.TibiaId,

                        OpenTibiaId = otbItem.OpenTibiaId,
                    };

                    if (otbItem.Flags.Is(FileFormats.Otb.ItemFlags.Readable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Readable;
                    }

                    metadatas.Add(otbItem.OpenTibiaId, metadata);
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

                    if (datItem.Flags.Is(ItemFlags.IsContainer) )
                    {
                        metadata.Flags |= ItemMetadataFlags.IsContainer;
                    }

                    if (datItem.Flags.Is(ItemFlags.Stackable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Stackable;
                    }

                    if (datItem.Flags.Is(ItemFlags.Useable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Useable;
                    }

                    if (datItem.Flags.Is(ItemFlags.IsFluid) || datItem.Flags.Is(ItemFlags.IsSplash) )
                    {
                        metadata.Flags |= ItemMetadataFlags.IsFluid;
                    }

                    if (datItem.Flags.Is(ItemFlags.NotWalkable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.NotWalkable;
                    }

                    if (datItem.Flags.Is(ItemFlags.NotMoveable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.NotMoveable;
                    }

                    if (datItem.Flags.Is(ItemFlags.BlockProjectile) )
                    {
                        metadata.Flags |= ItemMetadataFlags.BlockProjectile;
                    }

                    if (datItem.Flags.Is(ItemFlags.BlockPathFinding) )
                    {
                        metadata.Flags |= ItemMetadataFlags.BlockPathFinding;
                    }

                    if (datItem.Flags.Is(ItemFlags.Pickupable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Pickupable;
                    }

                    if (datItem.Flags.Is(ItemFlags.Rotatable) )
                    {
                        metadata.Flags |= ItemMetadataFlags.Rotatable;
                    }

                    if (datItem.ItemHeight > 0)
                    {
                        metadata.Flags |= ItemMetadataFlags.HasHeight;
                    }

                    metadata.Speed = datItem.Speed;

                    if (datItem.LightLevel > 0 || datItem.LightColor > 0)
                    {
                        metadata.Light = new Light( (byte)datItem.LightLevel, (byte)datItem.LightColor);
                    }
                }
            }

            foreach (var xmlItem in itemsFile.Items)
            {
                if (xmlItem.OpenTibiaId < 20000)
                {
                    ItemMetadata metadata = metadatas[xmlItem.OpenTibiaId];

                    metadata.Name = xmlItem.Name;

                    metadata.Capacity = xmlItem.ContainerSize;

                    metadata.FloorChange = xmlItem.FloorChange;
                }
            }
        }

        private Dictionary<ushort, ItemMetadata> metadatas;

        public Item Create(ushort openTibiaId, Action<Item> initialize = null)
        {
            ItemMetadata metadata;

            if ( !metadatas.TryGetValue(openTibiaId, out metadata) )
            {
                return null;
            }

            Item item = null;

            if (openTibiaId == 1387)
            {
                TeleportItem teleportItem = new TeleportItem(metadata);

                item = teleportItem;
            }

            if (metadata.Flags.Is(ItemMetadataFlags.IsContainer) )
            {
                Container container = new Container(metadata);

                item = container;
            }

            if (metadata.Flags.Is(ItemMetadataFlags.Stackable) )
            {
                StackableItem stackableItem = new StackableItem(metadata);

                stackableItem.Count = 1;

                item = stackableItem;
            }

            if (metadata.Flags.Is(ItemMetadataFlags.IsFluid) )
            {
                FluidItem fluidItem = new FluidItem(metadata);

                fluidItem.FluidType = FluidType.Empty;

                item = fluidItem;
            }

            if (metadata.Flags.Is(ItemMetadataFlags.Readable) )
            {
                ReadableItem readableItem = new ReadableItem(metadata);

                readableItem.Text = "";

                item = readableItem;
            }

            if (item == null)
            {
                item = new Item(metadata);
            }

            if (initialize != null)
            {
                initialize(item);
            }

            gameObjectCollection.AddGameObject(item);

            return item;
        }

        public void Destroy(Item item)
        {
            gameObjectCollection.RemoveGameObject(item);
        }
    }
}
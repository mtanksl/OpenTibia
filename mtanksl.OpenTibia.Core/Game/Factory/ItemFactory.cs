using System.Linq;
using System.Collections.Generic;

using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.Common.Objects;

namespace OpenTibia
{
    public class ItemFactory
    {
        private Dictionary<ushort, ItemMetadata> metadatas;

        public ItemFactory(OtbFile otbFile, DatFile datFile, ItemsFile itemsFile)
        {
            metadatas = new Dictionary<ushort, ItemMetadata>(datFile.Items.Count);

            foreach ( var otbItem in otbFile.Items )
            {
                if (otbItem.Group != ItemGroup.Deprecated)
                {
                    metadatas.Add(otbItem.ServerId, new ItemMetadata()
                        {
                            ClientId = otbItem.ClientId,

                            ServerId = otbItem.ServerId
                        }
                    );
                }
            }

            var lookup = otbFile.Items.ToLookup(otbItem => otbItem.ClientId, item => item.ServerId);

            foreach ( var datItem in datFile.Items )
            {
                foreach ( var serverId in lookup[datItem.Id] )
                {
                    ItemMetadata metadata = metadatas[serverId];
                    
                    metadata.TopOrder = datItem.IsGround ? TopOrder.Ground : datItem.AlwaysOnTop1 ? TopOrder.HighPriority : datItem.AlwaysOnTop2 ? TopOrder.MediumPriority : datItem.AlwaysOnTop3 ? TopOrder.LowPriority : TopOrder.Other;

                    metadata.Speed = datItem.Speed;

                    if (datItem.IsContainer)
                    {
                        metadata.Flags |= ItemMetadataFlags.IsContainer;
                    }

                    if (datItem.Stackable)
                    {
                        metadata.Flags |= ItemMetadataFlags.Stackable;
                    }

                    if (datItem.Useable)
                    {
                        metadata.Flags |= ItemMetadataFlags.Useable;
                    }

                    if (datItem.IsFluid)
                    {
                        metadata.Flags |= ItemMetadataFlags.IsFluid;
                    }

                    if (datItem.IsSplash)
                    {
                        metadata.Flags |= ItemMetadataFlags.IsSplash;
                    }

                    if (datItem.NotWalkable)
                    {
                        metadata.Flags |= ItemMetadataFlags.NotWalkable;
                    }

                    if (datItem.NotMoveable)
                    {
                        metadata.Flags |= ItemMetadataFlags.NotMoveable;
                    }

                    if (datItem.BlockProjectile)
                    {
                        metadata.Flags |= ItemMetadataFlags.BlockProjectile;
                    }

                    if (datItem.BlockPathFinding)
                    {
                        metadata.Flags |= ItemMetadataFlags.BlockPathFinding;
                    }

                    if (datItem.Pickupable)
                    {
                        metadata.Flags |= ItemMetadataFlags.Pickupable;
                    }

                    if (datItem.Hangable)
                    {
                        metadata.Flags |= ItemMetadataFlags.Hangable;
                    }

                    if (datItem.Horizontal)
                    {
                        metadata.Flags |= ItemMetadataFlags.Horizontal;
                    }

                    if (datItem.Vertical)
                    {
                        metadata.Flags |= ItemMetadataFlags.Vertical;
                    }

                    if (datItem.Rotatable)
                    {
                        metadata.Flags |= ItemMetadataFlags.Rotatable;
                    }

                    if (datItem.ItemHeight != 0)
                    {
                        metadata.Flags |= ItemMetadataFlags.HasHeight;
                    }

                    metadata.Light = new Light( (byte)datItem.LightLevel, (byte)datItem.LightColor );
                }
            }

            foreach ( var xmlItem in itemsFile.Items )
            {
                if (xmlItem.Id < 20000)
                {
                    ItemMetadata metadata = metadatas[xmlItem.Id];

                    metadata.Article = xmlItem.Article;

                    metadata.Name = xmlItem.Name;

                    metadata.Plural = xmlItem.Plural;

                    metadata.Weight = xmlItem.Weight;

                    metadata.FloorChange = xmlItem.FloorChange;
                }
            }
        }

        public Item Create(ushort itemId)
        {
            ItemMetadata metadata;

            if ( !metadatas.TryGetValue(itemId, out metadata) )
            {
                return null;
            }

            ItemMetadataFlags flags = metadata.Flags;

            if ( flags.Any(ItemMetadataFlags.IsContainer) )
            {
                return new Container(metadata);
            }
            else if ( flags.Any(ItemMetadataFlags.Stackable) || flags.Any(ItemMetadataFlags.IsFluid) || flags.Any(ItemMetadataFlags.IsSplash) )
            {
                return new Stackable(metadata);
            }
            else if (metadata.ServerId == 1777)
            {
                return new Teleport(metadata);
            }
            
            return new Item(metadata);
        }
    }
}
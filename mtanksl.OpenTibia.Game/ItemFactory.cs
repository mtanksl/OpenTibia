using OpenTibia.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class ItemFactory
    {
        public ItemFactory(OpenTibia.FileFormats.Otb.OtbFile otbFile, OpenTibia.FileFormats.Dat.DatFile datFile, OpenTibia.FileFormats.Xml.Items.ItemsFile itemsFile)
        {
            metadatas = new Dictionary<ushort, ItemMetadata>(datFile.Items.Count);

            foreach (var otbItem in otbFile.Items)
            {
                if (otbItem.Group != FileFormats.Otb.ItemGroup.Deprecated)
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

                    metadata.TopOrder = datItem.IsGround ? TopOrder.Ground 
                        
                                                         : datItem.AlwaysOnTop1 ? TopOrder.HighPriority 
                                                          
                                                                                : datItem.AlwaysOnTop2 ? TopOrder.MediumPriority 
                                                          
                                                                                                       : datItem.AlwaysOnTop3 ? TopOrder.LowPriority 
                                                          
                                                                                                                              : TopOrder.Other;

                    metadata.Speed = datItem.Speed;

                    metadata.IsContainer = datItem.IsContainer;

                    metadata.Stackable = datItem.Stackable;

                    //TODO: Set other properties
                }
            }

            foreach (var xmlItem in itemsFile.Items)
            {
                if (xmlItem.OpenTibiaId < 20000)
                {
                    ItemMetadata metadata = metadatas[xmlItem.OpenTibiaId];

                    metadata.Name = xmlItem.Name;

                    metadata.Capacity = xmlItem.ContainerSize;

                    //TODO: Set other properties
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

            if (metadata.IsContainer)
            {
                return new Container(metadata);
            }
            else if (metadata.Stackable)
            {
                return new StackableItem(metadata);
            }

            return new Item(metadata);
        }
    }
}
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
                    metadatas.Add(otbItem.ServerId, new ItemMetadata()
                    {
                        ClientId = otbItem.ClientId,

                        ServerId = otbItem.ServerId
                    } );
                }
            }

            var lookup = otbFile.Items.ToLookup(otbItem => otbItem.ClientId, otbItem => otbItem.ServerId);

            foreach (var datItem in datFile.Items)
            {
                foreach (var serverId in lookup[datItem.Id] )
                {
                    ItemMetadata metadata = metadatas[serverId];

                    metadata.TopOrder = datItem.IsGround ? TopOrder.Ground 
                        
                                                         : datItem.AlwaysOnTop1 ? TopOrder.HighPriority 
                                                          
                                                                                : datItem.AlwaysOnTop2 ? TopOrder.MediumPriority 
                                                          
                                                                                                       : datItem.AlwaysOnTop3 ? TopOrder.LowPriority 
                                                          
                                                                                                                              : TopOrder.Other;

                    metadata.Speed = datItem.Speed;

                    //TODO: Set other properties
                }
            }

            foreach (var xmlItem in itemsFile.Items)
            {
                if (xmlItem.Id < 20000)
                {
                    ItemMetadata metadata = metadatas[xmlItem.Id];

                    //TODO: Set other properties
                }
            }
        }

        private Dictionary<ushort, ItemMetadata> metadatas;

        public Item Create(ushort itemId)
        {
            ItemMetadata metadata;

            if ( !metadatas.TryGetValue(itemId, out metadata) )
            {
                return null;
            }
            
            return new Item(metadata);
        }
    }
}
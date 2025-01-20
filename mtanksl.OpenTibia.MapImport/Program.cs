using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Otbm;
using System.Collections.Generic;
using System.Linq;
using OtbmItem = OpenTibia.FileFormats.Otbm.Item;

namespace mtanksl.OpenTibia.MonsterImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var otbPath = @"";

            var otbmFromPath = @"";

            var otbmToPath = @"";

            // Convert map from 7.72 to 8.60
            /*
            var tibiaMetadatas = new Dictionary<ushort, List<ItemMetadata>>();

            ItemMetadata GetItemMetadataByTibiaId(ushort tibiaId)
            {                
                List<ItemMetadata> metadatas;

                if (tibiaMetadatas.TryGetValue(tibiaId, out metadatas))
                {
                    return metadatas.FirstOrDefault();
                }

                return null;
            }

            var otbFile = OtbFile.Load(otbPath);

            foreach (var otbItem in otbFile.Items)
            {
                if (otbItem.Group != ItemGroup.Deprecated)
                {
                    ItemMetadata metadata = new ItemMetadata()
                    {
                        TibiaId = otbItem.TibiaId,

                        OpenTibiaId = otbItem.OpenTibiaId,
                    };

                    List<ItemMetadata> metadatas;

                    if ( !tibiaMetadatas.TryGetValue(otbItem.TibiaId, out metadatas) )
                    {
                        metadatas = new List<ItemMetadata>();

                        tibiaMetadatas.Add(otbItem.TibiaId, metadatas);                        
                    }

                    metadatas.Add(metadata);
                }
            }

            var otbmFile = OtbmFile.Load(otbmFromPath);

            otbmFile.OtbmInfo.TibiaVersion = TibiaVersion.Version8602;

            foreach (var area in otbmFile.Areas)
            {
                foreach (var tile in area.Tiles)
                {
                    tile.Flags = tile.Flags & ~TileFlags.Refresh;

                    if (tile.OpenTibiaItemId > 0)
                    {
                        tile.OpenTibiaItemId = GetItemMetadataByTibiaId(tile.OpenTibiaItemId).OpenTibiaId;
                    }

                    if (tile.OpenTibiaItemId == 4347)
                    {
                        tile.OpenTibiaItemId = 919; // Mountain
                    }
                    else if (tile.OpenTibiaItemId == 4626 || 
                             tile.OpenTibiaItemId == 4627 || 
                             tile.OpenTibiaItemId == 4628 || 
                             tile.OpenTibiaItemId == 4629 || 
                             tile.OpenTibiaItemId == 4630 || 
                             tile.OpenTibiaItemId == 4631 ||
                             tile.OpenTibiaItemId == 4841 ||
                             tile.OpenTibiaItemId == 4844)
                    {
                        tile.OpenTibiaItemId = 4608; // Shallow water
                    }
                    else if (tile.OpenTibiaItemId == 4348)
                    {
                        tile.OpenTibiaItemId = 1742; // Wooden coffin
                    }
                    else if (tile.OpenTibiaItemId == 4349)
                    {
                        tile.OpenTibiaItemId = 1744; // Wooden coffin
                    }

                    void Loop(List<OtbmItem> items)
                    {
                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                if (item.OpenTibiaId > 0)
                                {
                                    item.OpenTibiaId = GetItemMetadataByTibiaId(item.OpenTibiaId).OpenTibiaId;
                                }

                                Loop(item.Items);
                            }
                        }
                    }

                    Loop(tile.Items);                                        
                }
            }

            OtbmFile.Save(otbmFile, otbmToPath);
            */
        }
    }
}
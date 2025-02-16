using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Xml.Items;
using Item = OpenTibia.Common.Objects.Item;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IItemFactory
    {
        void Start(OtbFile otbFile, DatFile datFile, ItemsFile itemsFile);

        ItemMetadata GetItemMetadataByOpenTibiaId(ushort openTibiaId);

        ItemMetadata GetItemMetadataByTibiaId(ushort tibiaId);

        Item Create(ushort openTibiaId, byte typeCount);

        void Attach(Item item);

        bool Detach(Item item);

        void ClearComponentsAndEventHandlers(Item item);
    }
}
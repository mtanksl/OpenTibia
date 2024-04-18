using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Dat;
using OpenTibia.FileFormats.Otb;
using OpenTibia.FileFormats.Xml.Items;
using OpenTibia.Game.GameObjectScripts;
using Item = OpenTibia.Common.Objects.Item;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IItemFactory
    {
        void Start(OtbFile otbFile, DatFile datFile, ItemsFile itemsFile);

        ItemMetadata GetItemMetadataByOpenTibiaId(ushort openTibiaId);

        ItemMetadata GetItemMetadataByTibiaId(ushort tibiaId);

        GameObjectScript<ushort, Item> GetItemGameObjectScript(ushort openTibiaId);

        Item Create(ushort openTibiaId, byte count);

        void Attach(Item item);

        bool Detach(Item item);

        void ClearComponentsAndEventHandlers(Item item);
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Xml.Monsters;
using Monster = OpenTibia.Common.Objects.Monster;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IMonsterFactory
    {
        void Start(MonsterFile monsterFile);

        MonsterMetadata GetMonsterMetadata(string name);

        Monster Create(string name, Tile spawn);

        void Attach(Monster monster);

        bool Detach(Monster monster);

        void ClearComponentsAndEventHandlers(Monster monster);
    }
}
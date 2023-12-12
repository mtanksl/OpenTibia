#if AOT
using OpenTibia.Common.Objects;

namespace OpenTibia.Game.GameObjectScripts
{
    public static class _AotCompilation
    {
        public static readonly GameObjectScript<ushort, Item>[] Items = new GameObjectScript<ushort, Item>[]
        {
            new ItemScript(),
            new StreetLampSwitchOffItemScript(),
            new StreetLampSwitchOnItemScript(),
        };

        public static readonly GameObjectScript<string, Monster>[] Monsters = new GameObjectScript<string, Monster>[]
        {
            new MonsterScript(),
            new AmazonMonsterScript(),
            new DeerMonsterScript(),
            new DogMonsterScript(),
            new ValkyrieMonsterScript(),
        };

        public static readonly GameObjectScript<string, Npc>[] Npcs = new GameObjectScript<string, Npc>[]
        {
            new NpcScript(),
        };

        public static readonly GameObjectScript<string, Player>[] Players = new GameObjectScript<string, Player>[]
        {
            new PlayerScript(),
        };
    }
}
#endif
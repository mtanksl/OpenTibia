#if AOT
namespace OpenTibia.Game.Scripts
{
    public static class _AotCompilation
    {
        public static readonly Script[] Scripts = new Script[]
        {
            new CreatureWalkScripts(),
            new GlobalScripts(),
            new ItemDestroyScript(),
            new PlayerDestroyScripts(),
            new PlayerLoginScripts(),
            new PlayerLogoutScripts(),
            new PlayerMoveCreatureScripts(),
            new PlayerMoveItemScripts(),
            new PlayerRotateItemScripts(),
            new PlayerSayScripts(),
            new PlayerTradeWithScripts(),
            new PlayerUseItemScripts(),
            new PlayerUseItemWithCreatureScripts(),
            new PlayerUseItemWithItemScripts(),
            new TileAddCreatureScripts(),
            new TileRemoveCreatureScripts(),
        };
    }
}
#endif
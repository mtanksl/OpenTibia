#if AOT
namespace OpenTibia.Game.Scripts
{
    public static class _AotCompilation
    {
        public static readonly Script[] Scripts = new Script[]
        {
            new CreatureDestroyScript(),
            new CreatureMoveScripts(),
            new GlobalScripts(),
            new ItemDestroyScript(),
            new ItemMoveScript(),
            new ItemTransformScripts(),
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
            new StackableItemUpdateCountScripts(),
            new TileAddCreatureScripts(),
            new TileRemoveCreatureScripts(),
        };
    }
}
#endif
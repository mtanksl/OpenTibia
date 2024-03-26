#if AOT
namespace OpenTibia.Game.Scripts
{
    public static class _AotCompilation
    {
        public static readonly Script[] Scripts = new Script[]
        {
            new ContainerAddItemScripts(),
            new ContainerRemoveItemScripts(),
            new CreatureDestroyScript(),
            new CreatureMoveScripts(),
            new FluidItemUpdateFluidTypeScripts(),
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
            new SplashItemUpdateFluidTypeScripts(),
            new StackableItemUpdateCountScripts(),
            new TileAddCreatureScripts(),
            new TileRemoveCreatureScripts(),
        };
    }
}
#endif
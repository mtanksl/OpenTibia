#if AOT
namespace OpenTibia.Game.Scripts
{
    public static class _AotCompilation
    {
        public static readonly Script[] Scripts = new Script[]
        {
            new ContainerAddItemScripts(),
            new ContainerRemoveItemScripts(),
            new CreatureDestroyScripts(),
            new CreatureMoveScripts(),
            new FluidItemUpdateFluidTypeScripts(),
            new GlobalScripts(),
            new ItemDestroyScripts(),
            new ItemMoveScripts(),
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
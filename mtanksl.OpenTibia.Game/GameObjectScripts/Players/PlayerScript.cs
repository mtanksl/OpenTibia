using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.GameObjectScripts
{
    public class PlayerScript : GameObjectScript<Player>
    {
        public override void Start(Player player)
        {
            Context.Server.GameObjectComponents.AddComponent(player, new PlayerThinkBehaviour(
                new IntervalAndChanceAttackStrategy(1000, 100, InventoryWeaponAttackStrategy.Instance), 
                FollowWalkStrategy.Instance) );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerMuteBehaviour() );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerRegenerationConditionBehaviour() );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerCooldownBehaviour() );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerEnvironmentLightBehaviour() );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerPingBehaviour() );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerIdleBehaviour() );
        }

        public override void Stop(Player player)
        {

        }
    }
}
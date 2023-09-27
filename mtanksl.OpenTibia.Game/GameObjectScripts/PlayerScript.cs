using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.GameObjectScripts
{
    public class PlayerScript : GameObjectScript<string, Player>
    {
        public override string Key
        {
            get
            {
                return "";
            }
        }

        public override void Start(Player player)
        {
            Context.Server.GameObjectComponents.AddComponent(player, new PlayerThinkBehaviour(new InventoryWeaponAttackStrategy(TimeSpan.FromSeconds(1) ), new FollowWalkStrategy() ) );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerCooldownBehaviour() );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerEnvironmentLightBehaviour() );

            Context.Server.GameObjectComponents.AddComponent(player, new PlayerPingBehaviour() );
        }

        public override void Stop(Player player)
        {

        }
    }
}
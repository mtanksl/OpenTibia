using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Commands
{
    public class RegenerationCondition : Condition
    {
        public RegenerationCondition(int regenerationTick) : base(ConditionSpecialCondition.Regeneration)
        {
            this.regenerationTick = regenerationTick;
        }

        private int regenerationTick;

        public int RegenerationTick
        {
            get
            {
                return regenerationTick;
            }
        }

        public bool AddRegenerationTick(int regenerationTick)
        {
            if (this.regenerationTick + regenerationTick > 20 * 60) 
            {
                return false;
            }

            this.regenerationTick += regenerationTick;

            return true;
        }

        private string key = Guid.NewGuid().ToString();

        public override async Promise AddCondition(Creature creature)
        {
            Player player = (Player)creature;
            
            var vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)player.Vocation);

            int health = vocationConfig.Health;
            int healthDelayInSeconds = vocationConfig.HealthDelayInSeconds;

            int mana = vocationConfig.Mana;
            int manaDelayInSeconds = vocationConfig.ManaDelayInSeconds;

            int healthTick = healthDelayInSeconds;

            int manaTick = manaDelayInSeconds;

            while (regenerationTick > 0)
            {
                await Promise.Delay(key, TimeSpan.FromSeconds(1) );

                healthTick--;

                if (healthTick == 0)
                {
                    healthTick = healthDelayInSeconds;

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + health) );
                }

                manaTick--;

                if (manaTick == 0)
                {
                    manaTick = manaDelayInSeconds;

                    await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana + mana) );
                }

                regenerationTick--;
            }
        }

        public override Promise RemoveCondition(Creature creature)
        {
            return Promise.Completed;
        }

        public override void Cancel()
        {
            Context.Current.Server.CancelQueueForExecution(key);
        }
    }
}
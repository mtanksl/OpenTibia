using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

            int health;
            int healthDelayInSeconds;

            int mana;
            int manaDelayInSeconds;

            switch (player.Vocation)
            {
                case Vocation.None:

                    health = 1;
                    healthDelayInSeconds = 12;

                    mana = 2;
                    manaDelayInSeconds = 6;

                    break;

                case Vocation.Knight:

                    health = 1;
                    healthDelayInSeconds = 6;

                    mana = 2;
                    manaDelayInSeconds = 6;

                    break;

                case Vocation.Paladin:

                    health = 1;
                    healthDelayInSeconds = 8;

                    mana = 2;
                    manaDelayInSeconds = 4;

                    break;

                case Vocation.Druid:

                    health = 1;
                    healthDelayInSeconds = 12;

                    mana = 2;
                    manaDelayInSeconds = 3;

                    break;

                case Vocation.Sorcerer:

                    health = 1;
                    healthDelayInSeconds = 12;

                    mana = 2;
                    manaDelayInSeconds = 3;

                    break;

                case Vocation.EliteKnight:

                    health = 1;
                    healthDelayInSeconds = 4;

                    mana = 2;
                    manaDelayInSeconds = 6;

                    break;

                case Vocation.RoyalPaladin:

                    health = 1;
                    healthDelayInSeconds = 6;

                    mana = 2;
                    manaDelayInSeconds = 3;

                    break;

                case Vocation.ElderDruid:

                    health = 1;
                    healthDelayInSeconds = 12;

                    mana = 2;
                    manaDelayInSeconds = 2;

                    break;

                case Vocation.MasterSorcerer:

                    health = 1;
                    healthDelayInSeconds = 12;

                    mana = 2;
                    manaDelayInSeconds = 2;

                    break;

                default:

                    throw new NotImplementedException();
            }

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
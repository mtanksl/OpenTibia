using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class RegenerationCondition : Condition
    {
        private DelayBehaviour delayBehaviour;

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

        public override async Promise Update(Creature target)
        {
            Player player = (Player)target;

            int health;
            int healthDelayInSeconds;

            int mana;
            int manaDelayInSeconds;

            switch (player.Vocation)
            {
                case Vocation.None:
                case Vocation.Gamemaster:

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
                delayBehaviour = Context.Current.Server.Components.AddComponent(player, new DelayBehaviour(TimeSpan.FromSeconds(1) ) );

                await delayBehaviour.Promise;

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

        public override void Stop(Server server)
        {
            if (delayBehaviour != null)
            {
                delayBehaviour.Stop(server);
            }
        }
    }
}
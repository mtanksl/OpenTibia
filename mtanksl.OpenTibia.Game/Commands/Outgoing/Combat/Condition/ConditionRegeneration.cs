using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class ConditionRegeneration : Condition
    {
        private DelayBehaviour delayBehaviour;

        public ConditionRegeneration(int regeneration) : base(ConditionSpecialCondition.Regeneration)
        {
            this.regeneration = regeneration;
        }

        private int regeneration;

        public int Regeneration
        {
            get
            {
                return regeneration;
            }
        }

        public bool AddRegeneration(int regeneration)
        {
            if (this.regeneration + regeneration > 1200) // 20 minutes
            {
                return false;
            }

            this.regeneration += regeneration;

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

            while (regeneration > 0)
            {
                delayBehaviour = Context.Current.Server.Components.AddComponent(player, new DelayBehaviour(1000) );

                await delayBehaviour.Promise;

                regeneration--;

                healthTick--;

                manaTick--;

                if (healthTick == 0)
                {
                    healthTick = healthDelayInSeconds;

                    await Context.Current.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + health) );
                }

                if (manaTick == 0)
                {
                    manaTick = manaDelayInSeconds;

                    await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana + mana) );
                }
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
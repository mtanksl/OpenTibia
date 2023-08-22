using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using System;

namespace OpenTibia.Game.Commands
{
    public class RegenerationCondition : CreatureConditionBehaviour
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

        public override async void Start()
        {
            base.Start();

            Player player = (Player)GameObject;

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

            try
            {
                while (regenerationTick > 0)
                {
                    await Promise.Delay(key, TimeSpan.FromSeconds(1) );

                    healthTick--;

                    if (healthTick == 0)
                    {
                        healthTick = healthDelayInSeconds;

                        await Context.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + health));
                    }

                    manaTick--;

                    if (manaTick == 0)
                    {
                        manaTick = manaDelayInSeconds;

                        await Context.AddCommand(new PlayerUpdateManaCommand(player, player.Mana + mana));
                    }

                    regenerationTick--;
                }

                Context.Server.GameObjectComponents.RemoveComponent(player, this);
            }
            catch (PromiseCanceledException) { }
        }

        public override void Stop()
        {
            base.Stop();

            Player player = (Player)GameObject;

            Context.Server.CancelQueueForExecution(key);
        }
    }
}
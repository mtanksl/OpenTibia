using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public class PlayerRegenerationConditionBehaviour : Behaviour
    {
        private int regeneration;

        public int Regeneration
        {
            get
            {
                return regeneration;
            }
        }

        public bool AddRegeneration(int regenerationInSeconds)
        {
            if (regeneration + regenerationInSeconds > 20 * 60)
            {
                return false;
            }

            regeneration += regenerationInSeconds;

            return true;
        }

        private Guid globalTick;

        public override void Start()
        {
            Player player = (Player)GameObject;

            VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)player.Vocation);

            int health = vocationConfig.RegenerateHealthInSeconds;

            int mana = vocationConfig.RegenerateManaInSeconds;

            int ticks = 1000;

            globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(player.Id), async (context, e) =>
            {
                ticks -= e.Ticks;

                while (ticks <= 0)
                {
                    ticks += 1000;

                    if (regeneration > 0)
                    {
                        regeneration--;

                        if (health > 0)
                        {
                            health--;
                        }
                        else
                        {
                            health = vocationConfig.RegenerateHealthInSeconds;

                            await Context.Current.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + vocationConfig.RegenerateHealth) );
                        }

                        if (mana > 0)
                        {
                            mana--;
                        }
                        else
                        {
                            mana = vocationConfig.RegenerateManaInSeconds;

                            await Context.Current.AddCommand(new PlayerUpdateManaCommand(player, player.Mana + vocationConfig.RegenerateMana) );
                        }
                    }
                }
            } );
        }

        public override void Stop()
        {
            Context.Server.EventHandlers.Unsubscribe(globalTick);
        }
    }
}
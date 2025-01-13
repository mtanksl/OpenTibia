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

        public bool AddRegeneration(int regenerationInSeconds)
        {
            if (regeneration + regenerationInSeconds > 20 * 60)
            {
                return false;
            }

            regeneration += regenerationInSeconds;

            return true;
        }

        private int soulRegeneration;

        public void AddSoulRegeneration()
        {
            soulRegeneration = 4 * 60;
        }

        private Guid globalTick;

        public override void Start()
        {
            Player player = (Player)GameObject;

            VocationConfig vocationConfig = Context.Server.Vocations.GetVocationById( (byte)player.Vocation);

            int health = vocationConfig.RegenerateHealthInSeconds;

            int mana = vocationConfig.RegenerateManaInSeconds;

            int soul = vocationConfig.RegenerateSoulInSeconds;

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

                            await Context.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + vocationConfig.RegenerateHealth) );
                        }

                        if (mana > 0)
                        {
                            mana--;
                        }
                        else
                        {
                            mana = vocationConfig.RegenerateManaInSeconds;

                            await Context.AddCommand(new PlayerUpdateManaCommand(player, player.Mana + vocationConfig.RegenerateMana) );
                        }
                    }

                    if (soulRegeneration > 0)
                    {
                        soulRegeneration--;

                        if (soul > 0)
                        {
                            soul--;
                        }
                        else
                        {
                            soul = vocationConfig.RegenerateSoulInSeconds;

                            if (player.Soul < vocationConfig.SoulMax)
                            {
                                await Context.AddCommand(new PlayerUpdateSoulCommand(player, player.Soul + vocationConfig.RegenerateSoul, vocationConfig.SoulMax) );
                            }
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
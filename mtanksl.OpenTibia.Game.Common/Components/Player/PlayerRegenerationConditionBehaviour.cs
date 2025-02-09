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

            if (regeneration == 0 && soulRegeneration == 0)
            {
                health = vocationConfig.RegenerateHealthInSeconds;

                mana = vocationConfig.RegenerateManaInSeconds;

                soul = vocationConfig.RegenerateSoulInSeconds;

                ticks = 1000;

                globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(GameObject.Id), OnThink);
            }

            regeneration += regenerationInSeconds;

            return true;
        }

        private int soulRegeneration;

        public void AddSoulRegeneration()
        {
            if (regeneration == 0 && soulRegeneration == 0)
            {
                health = vocationConfig.RegenerateHealthInSeconds;

                mana = vocationConfig.RegenerateManaInSeconds;

                soul = vocationConfig.RegenerateSoulInSeconds;

                ticks = 1000;

                globalTick = Context.Server.EventHandlers.Subscribe(GlobalTickEventArgs.Instance(GameObject.Id), OnThink);
            }

            soulRegeneration = 4 * 60;
        }

        private Player player;

        private VocationConfig vocationConfig;

        private int health;

        private int mana;

        private int soul;

        private int ticks;

        private Guid globalTick;

        public override void Start()
        {
            player = (Player)GameObject;

            vocationConfig = Context.Server.Vocations.GetVocationById( (byte)player.Vocation);
        }

        private async Promise OnThink(Context context, GlobalTickEventArgs e)
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

                        if ( !player.Tile.ProtectionZone)
                        {
                            await Context.AddCommand(new CreatureUpdateHealthCommand(player, player.Health + vocationConfig.RegenerateHealth) );
                        }
                    }

                    if (mana > 0)
                    {
                        mana--;
                    }
                    else
                    {
                        mana = vocationConfig.RegenerateManaInSeconds;

                        if ( !player.Tile.ProtectionZone)
                        {
                            await Context.AddCommand(new PlayerUpdateManaCommand(player, player.Mana + vocationConfig.RegenerateMana) );
                        }
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

                if (regeneration == 0 && soulRegeneration == 0)
                {
                    Context.Server.EventHandlers.Unsubscribe(globalTick);

                    break;
                }
            }
        }

        public override void Stop()
        {
            if (regeneration > 0 || soulRegeneration > 0)
            {
                regeneration = 0;

                soulRegeneration = 0;

                Context.Server.EventHandlers.Unsubscribe(globalTick);
            }
        }
    }
}
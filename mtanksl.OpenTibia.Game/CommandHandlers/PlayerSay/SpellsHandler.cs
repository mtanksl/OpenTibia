using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;

namespace OpenTibia.Game.CommandHandlers
{
    public class SpellsHandler : CommandHandler<PlayerSayCommand>
    {
        public override async Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            SpellPlugin plugin = Context.Server.Plugins.GetSpellPlugin(false, command.Message);

            if (plugin != null)
            {
                if (Context.Server.Config.LearnSpellFirst)
                {
                    if ( !command.Player.Client.Spells.HasSpell(plugin.Spell.Name) )
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouNeedToLearnThisSpellFirst) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }
                }

                if (command.Player.Level < plugin.Spell.Level)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughLevel) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                    await Promise.Break;
                }
                 
                if (command.Player.Mana < plugin.Spell.Mana)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMana) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (command.Player.Soul < plugin.Spell.Soul)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughSoul) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if (plugin.Spell.Vocations != null && !plugin.Spell.Vocations.Contains(command.Player.Vocation) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YourVocationCannotUseThisSpell) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                    await Promise.Break;
                }
                 
                if (plugin.Spell.Group == "Attack" && command.Player.Tile.ProtectionZone)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.ThisActionIsNotPermittedInAProtectionZone) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(command.Player);

                if (playerCooldownBehaviour.HasCooldown(plugin.Spell.Name) || playerCooldownBehaviour.HasCooldown(plugin.Spell.Group) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                if ( !await plugin.OnCasting(command.Player, null, command.Message) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );

                    await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                    await Promise.Break;
                }

                playerCooldownBehaviour.AddCooldown(plugin.Spell.Name, plugin.Spell.Cooldown);

                playerCooldownBehaviour.AddCooldown(plugin.Spell.Group, plugin.Spell.GroupCooldown);

                if (plugin.Spell.Mana > 0)
                {
                    await Context.AddCommand(new PlayerUpdateManaCommand(command.Player, command.Player.Mana - plugin.Spell.Mana) );
                }

                if (plugin.Spell.Soul > 0)
                {
                    await Context.AddCommand(new PlayerUpdateSoulCommand(command.Player, command.Player.Soul - plugin.Spell.Soul) );
                }

                await plugin.OnCast(command.Player, null, command.Message);
            }

            await next();
        }
    }
}
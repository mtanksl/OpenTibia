using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class Runes2Handler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        public override async Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            RunePlugin plugin = Context.Server.Plugins.GetRunePlugin(false, command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                if ( !(command.ToItem.Parent is Tile toTile) )
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                    await Promise.Break;
                }
                else
                {
                    if (command.Player.Level < plugin.Rune.Level)
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughLevel) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }

                    if (command.Player.Skills.MagicLevel < plugin.Rune.MagicLevel)
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouDoNotHaveEnoughMagicLevel) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }

                    if (command.Player.Tile.ProtectionZone)
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackAPersonWhileYouAreInAProtectionZone) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }

                    if (toTile.ProtectionZone)
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackAPersonInAProtectionZone) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }

                    PlayerCooldownBehaviour playerCooldownBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerCooldownBehaviour>(command.Player);

                    if (playerCooldownBehaviour.HasCooldown(plugin.Rune.Group) )
                    {                     
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouAreExhausted) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }

                    if ( !Context.Server.Pathfinding.CanThrow(command.Player.Tile.Position, toTile.Position) )
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }

                    if ( !await plugin.OnUsingRune(command.Player, null, toTile, command.Item) )
                    {
                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThere) );

                        await Context.AddCommand(new ShowMagicEffectCommand(command.Player, MagicEffectType.Puff) );

                        await Promise.Break;
                    }

                    playerCooldownBehaviour.AddCooldown(plugin.Rune.Group, plugin.Rune.GroupCooldown);

                    if ( !Context.Current.Server.Config.GameplayInfiniteRunes)
                    {
                        await Context.AddCommand(new ItemDecrementCommand(command.Item, 1) );
                    }
                
                    await plugin.OnUseRune(command.Player, null, toTile, command.Item);

                    return;               
                }
            }
            else
            {
                plugin = Context.Server.Plugins.GetRunePlugin(true, command.Item.Metadata.OpenTibiaId);

                if (plugin != null)
                {
                    Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouCanNotUseThisObject) );

                    await Promise.Break;
                }
            }

            await next();
        }
    }
}
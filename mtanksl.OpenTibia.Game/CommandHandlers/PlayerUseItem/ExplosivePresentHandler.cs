using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class ExplosivePresentHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> explosivePresent = new HashSet<ushort>() { 8110 };

        private string sound = "KABOOOOOOOOOOM!";

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (explosivePresent.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int count;

                command.Player.Client.Storages.TryGetValue(AchievementConstants.JokesOnYou, out count);

                command.Player.Client.Storages.SetValue(AchievementConstants.JokesOnYou, ++count);

                if (count >= 1)
                {
                    if ( !command.Player.Client.Achievements.HasAchievement("Jokes On You") )
                    {
                        command.Player.Client.Achievements.SetAchievement("Jokes On You");

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"Jokes On You\".") );
                    }
                }

                switch (command.Item.Root() )
                {
                    case Tile tile:

                        return Context.AddCommand(new ShowMagicEffectCommand(tile.Position, MagicEffectType.ExplosionDamage) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sound) );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(command.Item));
                        } );

                    case Inventory inventory:
                    case null:

                        return Context.AddCommand(new ShowMagicEffectCommand(command.Player.Tile.Position, MagicEffectType.ExplosionDamage) ).Then( () =>
                        {
                            return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sound) );

                        } ).Then( () =>
                        {
                            return Context.AddCommand(new ItemDestroyCommand(command.Item));
                        } );
                }
            }

            return next();
        }
    }
}
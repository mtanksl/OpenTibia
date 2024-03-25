using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class StuffedDragonHandler : CommandHandler<PlayerUseItemCommand>
    {
        private HashSet<ushort> stuffedDragons = new HashSet<ushort>() { 5791 };

        private List<string> sounds = new List<string>() { "Fchhhhhh!", "Zchhhhhh!", "Grooaaaaar*cough*", "Aaa... CHOO!", "You... will.... burn!!" };

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            if (stuffedDragons.Contains(command.Item.Metadata.OpenTibiaId) )
            {
                int count;

                command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugStuffedDragon, out count);

                command.Player.Client.Storages.SetValue(AchievementConstants.INeedAHugStuffedDragon, ++count);

                if (command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugPandaTeddy, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugStuffedDragon, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugBabySealDoll, out _) &&
                    command.Player.Client.Storages.TryGetValue(AchievementConstants.INeedAHugSantaDoll, out _) )
                {
                    if ( !command.Player.Client.Achievements.HasAchievement("I Need a Hug") )
                    {
                        command.Player.Client.Achievements.SetAchievement("I Need a Hug");

                        Context.AddPacket(command.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"I Need a Hug\".") );
                    }
                }

                int value = Context.Server.Randomization.Take(0, sounds.Count - 1);

                return Context.AddCommand(new ShowTextCommand(command.Player, TalkType.MonsterSay, sounds[value] ) ).Then( () =>
                {
                    if (value == sounds.Count - 1)
                    {
                        return Context.AddCommand(new CreatureAttackCreatureCommand(null, command.Player, 
                            
                            new SimpleAttack(null, MagicEffectType.ExplosionDamage, AnimatedTextColor.Orange, 1, 1) ) );
                    }

                    return Promise.Completed;
                } );
            }

            return next();
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;

namespace OpenTibia.Game.Commands
{
    public class PlayerAddSkillPointsCommand : Command
    {
        public PlayerAddSkillPointsCommand(Player player, Skill skill, ulong skillPoints)
        {
            Player = player;

            Skill = skill;

            SkillPoints = skillPoints;
        }

        public Player Player { get; set; }

        public Skill Skill { get; set; }

        public ulong SkillPoints { get; set; }

        public override async Promise Execute()
        {
            VocationConfig vocationConfig = Context.Current.Server.Vocations.GetVocationById( (byte)Player.Vocation);

            byte skillLevel = Player.Skills.GetSkillLevel(Skill);

            ulong skillPoints = Player.Skills.GetSkillPoints(Skill);

            double vocationConstant = vocationConfig.VocationConstants.GetValue(Skill);


            byte correctSkillLevel = skillLevel;

            byte correctSkillPercent = 0;

            ulong minSkillPoints = Formula.GetRequiredSkillPoints(Skill, correctSkillLevel, vocationConstant);

            while (true)
            {
                ulong maxSkillPoints = Formula.GetRequiredSkillPoints(Skill, (byte)(correctSkillLevel + 1), vocationConstant);

                if (skillPoints + SkillPoints < maxSkillPoints)
                {
                    correctSkillPercent = (byte)Math.Ceiling(100.0 * (skillPoints + SkillPoints - minSkillPoints) / (maxSkillPoints - minSkillPoints));

                    break;
                }
                else
                {
                    correctSkillLevel++;

                    minSkillPoints = maxSkillPoints;
                }
            }

            if (correctSkillLevel > skillLevel)
            {
                string name;

                switch (Skill)
                {
                    case Skill.MagicLevel:

                        name = "magic level";

                        break;

                    case Skill.Fist:

                        name = "fist fighting";

                        break;

                    case Skill.Club:

                        name = "club fighting";

                        break;

                    case Skill.Sword:

                        name = "sword fighting";

                        break;

                    case Skill.Axe:

                        name = "axe fighting";

                        break;

                    case Skill.Distance:

                        name = "distance fighting";

                        break;

                    case Skill.Shield:

                        name = "shielding";

                        break;

                    case Skill.Fish:

                        name = "fishing";

                        break;

                    default:

                        throw new NotImplementedException();
                }

                Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "You advanced to " + name + " level " + correctSkillLevel + ".") );

                Context.AddEvent(new PlayerAdvanceSkillEventArgs(Player, Skill, skillLevel, correctSkillLevel) );
            }

            switch (Skill)
            {
                case Skill.MagicLevel:

                    await Context.AddCommand(new PlayerUpdateMagicLevelCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                case Skill.Fist:
                                                            
                    await Context.AddCommand(new PlayerUpdateFistCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                case Skill.Club:

                    await Context.AddCommand(new PlayerUpdateClubCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                case Skill.Sword:

                    await Context.AddCommand(new PlayerUpdateSwordCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                case Skill.Axe:

                    await Context.AddCommand(new PlayerUpdateAxeCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                case Skill.Distance:

                    await Context.AddCommand(new PlayerUpdateDistanceCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                case Skill.Shield:

                    await Context.AddCommand(new PlayerUpdateShieldCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                case Skill.Fish:

                    await Context.AddCommand(new PlayerUpdateFishCommand(Player, skillPoints + SkillPoints, correctSkillLevel, correctSkillPercent) );

                    break;

                default:

                    throw new NotImplementedException();
            }
        }
    }
}
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Text;

namespace OpenTibia.Game.Commands
{
    public class PlayerLookCreatureCommand : Command
    {
        public PlayerLookCreatureCommand(Player player, Creature creature)
        {
            Player = player;

            Creature = creature;
        }

        public Player Player { get; set; }

        public Creature Creature { get; set; }

        public override Promise Execute()
        {
            StringBuilder builder = new StringBuilder();

            switch (Creature)
            {
                case Player player:

                    if (player == Player)
                    {
                        builder.Append("You see yourself.");

                        switch (player.Vocation)
                        {
                            case Vocation.None:

                                builder.Append(" You have no vocation.");

                                break;

                            case Vocation.Knight:

                                builder.Append(" You are a knight.");

                                break;

                            case Vocation.Paladin:

                                builder.Append(" You are a paladin.");

                                break;

                            case Vocation.Druid:

                                builder.Append(" You are a druid.");

                                break;

                            case Vocation.Sorcerer:

                                builder.Append(" You are a sorcerer.");

                                break;

                            case Vocation.EliteKnight:

                                builder.Append(" You are an elite knight.");

                                break;

                            case Vocation.RoyalPaladin:

                                builder.Append(" You are a royal paladin.");

                                break;

                            case Vocation.ElderDruid:

                                builder.Append(" You are an elder druid.");

                                break;

                            case Vocation.MasterSorcerer:

                                builder.Append(" You are a master sorcerer.");

                                break;

                            case Vocation.Gamemaster:

                                builder.Append(" You are a Gamemaster.");

                                break;

                            default:

                                throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        builder.Append("You see " + player.Name + " (Level: " + player.Level + ").");

                        switch (player.Gender)
                        {
                            case Gender.Male:

                                builder.Append(" He");

                                break;

                            case Gender.Female:

                                builder.Append(" She");

                                break;

                            default:

                                throw new NotImplementedException();
                        }

                        switch (player.Vocation)
                        {
                            case Vocation.None:

                                builder.Append(" has no vocation.");

                                break;

                            case Vocation.Knight:

                                builder.Append(" is a knight.");

                                break;

                            case Vocation.Paladin:

                                builder.Append(" is a paladin.");

                                break;

                            case Vocation.Druid:

                                builder.Append(" is a druid.");

                                break;

                            case Vocation.Sorcerer:

                                builder.Append(" is a sorcerer.");

                                break;

                            case Vocation.EliteKnight:

                                builder.Append(" is an elite knight.");

                                break;

                            case Vocation.RoyalPaladin:

                                builder.Append(" is a royal paladin.");

                                break;

                            case Vocation.ElderDruid:

                                builder.Append(" is an elder druid.");

                                break;

                            case Vocation.MasterSorcerer:

                                builder.Append(" is a master sorcerer.");

                                break;

                            case Vocation.Gamemaster:

                                builder.Append(" is a Gamemaster.");

                                break;

                            default:

                                throw new NotImplementedException();
                        }
                    }

                    break;

                case Monster monster:

                    builder.Append("You see " + monster.Name + ".");

                    break;

                case Npc npc:

                    builder.Append("You see " + npc.Name + ".");

                    break;
            }

            Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}
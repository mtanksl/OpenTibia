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
            return Promise.Run( (resolve, reject) =>
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("You see ");

                if (Player == Creature)
                {
                    builder.Append("yourself. ");

                    switch (Player.Vocation)
                    {
                        case Vocation.None:

                            builder.Append("You have no vocation.");

                            break;

                        case Vocation.Knight:

                            builder.Append("You are a knight.");

                            break;

                        case Vocation.Paladin:

                            builder.Append("You are a paladin.");

                            break;

                        case Vocation.Druid:

                            builder.Append("You are a druid.");

                            break;

                        case Vocation.Sorcerer:

                            builder.Append("You are a sorcerer.");

                            break;

                        case Vocation.EliteKnight:

                            builder.Append("You are an elite knight.");

                            break;

                        case Vocation.RoyalPaladin:

                            builder.Append("You are a royal paladin.");

                            break;

                        case Vocation.ElderDruid:

                            builder.Append("You are an elder druid.");

                            break;

                        case Vocation.MasterSorcerer:

                            builder.Append("You are a master sorcerer.");

                            break;

                        default:

                            throw new NotImplementedException();
                    }
                }
                else
                {
                    switch (Creature)
                    {
                        case Monster monster:

                            builder.Append(monster.Name + ".");

                            break;

                        case Npc npc:

                            builder.Append(npc.Name + ".");

                            break;

                        case Player player:

                            builder.Append(player.Name + " (Level " + player.Level + "). ");

                            switch (player.Gender)
                            {
                                case Gender.Male:

                                    builder.Append("He ");

                                    break;

                                case Gender.Female:

                                    builder.Append("She ");

                                    break;

                                default:

                                    throw new NotImplementedException();
                            }

                            switch (player.Vocation)
                            {
                                case Vocation.None:

                                    builder.Append("has no vocation.");

                                    break;

                                case Vocation.Knight:

                                    builder.Append("is a knight.");

                                    break;

                                case Vocation.Paladin:

                                    builder.Append("is a paladin.");

                                    break;

                                case Vocation.Druid:

                                    builder.Append("is a druid.");

                                    break;

                                case Vocation.Sorcerer:

                                    builder.Append("is a sorcerer.");

                                    break;

                                case Vocation.EliteKnight:

                                    builder.Append("is an elite knight.");

                                    break;

                                case Vocation.RoyalPaladin:

                                    builder.Append("is a royal paladin.");

                                    break;

                                case Vocation.ElderDruid:

                                    builder.Append("is an elder druid.");

                                    break;

                                case Vocation.MasterSorcerer:

                                    builder.Append("is a master sorcerer.");

                                    break;

                                default:

                                    throw new NotImplementedException();
                            }

                            break;
                    }
                }

                Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

                resolve();
            } );
        }
    }
}
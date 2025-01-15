using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
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

                        switch (player.Rank)
                        {
                            case Rank.Player:
                            case Rank.Tutor:

                                var vocationConfig = Context.Server.Vocations.GetVocationById( (byte)player.Vocation);

                                if (player.Vocation == Vocation.None)
                                {
                                    builder.Append(" You have " + vocationConfig.Description + ".");
                                }
                                else
                                {
                                    builder.Append(" You are " + vocationConfig.Description + ".");
                                }

                                break;

                            case Rank.Gamemaster:

                                builder.Append(" You are a Gamemaster.");

                                break;

                            case Rank.AccountManager:

                                builder.Append(" You are an Account Manager.");

                                break;

                            default:

                                throw new NotImplementedException();
                        }                        
                    }
                    else
                    {
                        List<string> attributes = new List<string>();

                        attributes.Add("Level: " + player.Level);

                        if (Player.Rank == Rank.Gamemaster)
                        {
                            attributes.Add("Account Status: " + (player.Premium ? "Premium" : "Free") );

                            attributes.Add("IP Address: " + Player.Client.Connection.IpAddress);

                            PlayerPingBehaviour playerPingBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerPingBehaviour>(Creature);

                            if (playerPingBehaviour != null)
                            {
                                attributes.Add("Latency: " + playerPingBehaviour.GetLatency() + " ms");
                            }
                        }

                        builder.Append("You see " + player.Name);

                        if (attributes.Count > 0)
                        {
                            builder.Append(" (" + string.Join(", ", attributes) + ")");
                        }

                        builder.Append(".");

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

                        switch (player.Rank)
                        {
                            case Rank.Player:
                            case Rank.Tutor:

                                var vocationConfig = Context.Server.Vocations.GetVocationById( (byte)player.Vocation);

                                if (player.Vocation == Vocation.None)
                                {
                                    builder.Append(" has " + vocationConfig.Description + ".");
                                }
                                else
                                {
                                    builder.Append(" is " + vocationConfig.Description + ".");
                                }

                                break;

                            case Rank.Gamemaster:

                                builder.Append(" is a Gamemaster.");

                                break;

                            case Rank.AccountManager:

                                builder.Append(" is an Account Manager.");

                                break;

                            default:

                                throw new NotImplementedException();
                        }
                    }

                    break;

                case Monster monster:

                    builder.Append("You see " + monster.Metadata.Description + ".");

                    break;

                case Npc npc:

                    builder.Append("You see " + npc.Metadata.Description + ".");

                    break;
            }

            Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, builder.ToString() ) );

            return Promise.Completed;
        }
    }
}
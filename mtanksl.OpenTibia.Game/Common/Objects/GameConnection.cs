using OpenTibia.Common.Events;
using OpenTibia.Common.Structures;
using OpenTibia.Game;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
using System;
using System.Net.Sockets;

namespace OpenTibia.Common.Objects
{
    public class GameConnection : Connection
    {
        private Server server;

        public GameConnection(Server server, int port, Socket socket) : base(port, socket)
        {
            this.server = server;
        }

        protected override void OnConnect()
        {
            Send( new Message() { new SendConnectionInfoOutgoingPacket() }.GetBytes(Keys) );

            base.OnConnect();
        }

        protected override void OnReceive(byte[] body)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                if (Adler32.Generate(body, 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        Rsa.DecryptAndReplace(body, 9);
                    }
                    else
                    {
                        Xtea.DecryptAndReplace(body, 4, 32, Keys);

                        stream.Seek(Origin.Current, 2);
                    }

                    Command command = null;

                    switch (reader.ReadByte() )
                    {
                        case 0x0A:
                            {
                                var packet = server.PacketsFactory.Create<SelectedCharacterIncomingPacket>(reader);

                                command = new LogInCommand(this, packet);
                            }
                            break;

                        case 0x14:

                            command = new LogOutCommand(Client.Player);

                            break;

                        case 0x1E:

                            command = new PongCommand(Client.Player);

                            break;

                        case 0x64:
                            {
                                var packet = server.PacketsFactory.Create<WalkToIncomingPacket>(reader);

                                command = new WalkToCommand(Client.Player, packet.MoveDirections);
                            }
                            break;

                        case 0x65:

                            command = new WalkCommand(Client.Player, MoveDirection.North);

                            break;

                        case 0x66:

                            command = new WalkCommand(Client.Player, MoveDirection.East);

                            break;

                        case 0x67:

                            command = new WalkCommand(Client.Player, MoveDirection.South);

                            break;

                        case 0x68:

                            command = new WalkCommand(Client.Player, MoveDirection.West);

                            break;

                        case 0x69:

                            command = new StopWalkCommand(Client.Player);

                            break;

                        case 0x6A:

                            command = new WalkCommand(Client.Player, MoveDirection.NorthEast);

                            break;

                        case 0x6B:

                            command = new WalkCommand(Client.Player, MoveDirection.SouthEast);

                            break;

                        case 0x6C:

                            command = new WalkCommand(Client.Player, MoveDirection.SouthWest);

                            break;

                        case 0x6D:

                            command = new WalkCommand(Client.Player, MoveDirection.NorthWest);

                            break;

                        case 0x6F:

                            command = new TurnCommand(Client.Player, Direction.North);

                            break;

                        case 0x70:

                            command = new TurnCommand(Client.Player, Direction.East);

                            break;

                        case 0x71:

                            command = new TurnCommand(Client.Player, Direction.South);

                            break;

                        case 0x72:

                            command = new TurnCommand(Client.Player, Direction.West);

                            break;

                        case 0x78: //TODO
                            {
                                var packet = server.PacketsFactory.Create<MoveItemIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

                                Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

                                if (fromPosition.IsContainer)
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new MoveItemFromContainerToContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new MoveItemFromContainerToInventoryCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, toPosition.InventoryIndex, packet.Count);
                                    }
                                    else
                                    {
                                        command = new MoveItemFromContainerToTileCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, toPosition, packet.Count);
                                    }
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new MoveItemFromInventoryToContainerCommand(Client.Player, fromPosition.InventoryIndex, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new MoveItemFromInventoryToInventoryCommand(Client.Player, fromPosition.InventoryIndex, toPosition.InventoryIndex, packet.Count);
                                    }
                                    else
                                    {
                                        command = new MoveItemFromInventoryToTileCommand(Client.Player, fromPosition.InventoryIndex, toPosition, packet.Count);
                                    }
                                }
                                else
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new MoveItemFromTileToContainerCommand(Client.Player, fromPosition, packet.FromIndex, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new MoveItemFromTileToInventoryCommand(Client.Player, fromPosition, packet.FromIndex, toPosition.InventoryIndex, packet.Count);
                                    }
                                    else
                                    {
                                        command = new MoveItemFromTileToTileCommand(Client.Player, fromPosition, packet.FromIndex, toPosition, packet.Count);
                                    }
                                }
                            }
                            break;

                        case 0x79:
                            {
                                var packet = server.PacketsFactory.Create<LookItemNpcTradeIncommingPacket>(reader);

                                command = new LookFromNpcTradeCommand(Client.Player, packet.ItemId, packet.Type);
                            }
                            break;

                        case 0x7A:
                            {
                                var packet = server.PacketsFactory.Create<BuyNpcTradeIncommingPacket>(reader);

                                command = new BuyNpcTradeCommand(Client.Player, packet);
                            }
                            break;

                        case 0x7B:
                            {
                                var packet = server.PacketsFactory.Create<SellNpcTradeIncommingPacket>(reader);

                                command = new SellNpcTradeCommand(Client.Player, packet);
                            }
                            break;

                        case 0x7C:
                            
                            command = new CloseNpcTradeCommand(Client.Player);
                            
                            break;

                        case 0x7D: //TODO
                            {
                                var packet = server.PacketsFactory.Create<TradeWithIncommingPacket>(reader);

                                Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new TradeWithFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.CreatureId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new TradeWithFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.CreatureId);
                                }
                                else
                                {
                                    command = new TradeWithFromTileCommand(Client.Player, fromPosition, packet.Index, packet.CreatureId);
                                }
                            }
                            break;

                        case 0x7E:
                            {
                                var packet = server.PacketsFactory.Create<LookItemTradeIncommingPacket>(reader);

                                command = new LookFromTradeCommand(Client.Player, packet.WindowId, packet.Index);
                            }
                            break;

                        case 0x7F:
                            
                            command = new AcceptTradeCommand(Client.Player);
                            
                            break;

                        case 0x80:
                            
                            command = new CancelTradeCommand(Client.Player);
                            
                            break;

                        case 0x82: //TODO
                            {
                                var packet = server.PacketsFactory.Create<UseItemIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new UseItemFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new UseItemFromInventoryCommand(Client.Player, fromPosition.InventoryIndex);
                                }
                                else
                                {
                                    command = new UseItemFromTileCommand(Client.Player, fromPosition, packet.Index);
                                }
                            }
                            break;

                        case 0x83: //TODO
                            {
                                var packet = server.PacketsFactory.Create<UseItemWithItemIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

                                Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

                                if (fromPosition.IsContainer)
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new UseItemWithItemFromContainerToContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, toPosition.ContainerId, toPosition.ContainerIndex);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new UseItemWithItemFromContainerToInventoryCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, toPosition.InventoryIndex);
                                    }
                                    else
                                    {
                                        command = new UseItemWithItemFromContainerToTileCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, toPosition);
                                    }
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new UseItemWithItemFromInventoryToContainerCommand(Client.Player, fromPosition.InventoryIndex, toPosition.ContainerId, toPosition.ContainerIndex);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new UseItemWithItemFromInventoryToInventoryCommand(Client.Player, fromPosition.InventoryIndex, toPosition.InventoryIndex);
                                    }
                                    else
                                    {
                                        command = new UseItemWithItemFromInventoryToTileCommand(Client.Player, fromPosition.InventoryIndex, toPosition);
                                    }
                                }
                                else
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new UseItemWithItemFromTileToContainerCommand(Client.Player, fromPosition, packet.FromIndex, toPosition.ContainerId, toPosition.ContainerIndex);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new UseItemWithItemFromTileToInventoryCommand(Client.Player, fromPosition, packet.FromIndex, toPosition.InventoryIndex);
                                    }
                                    else
                                    {
                                        command = new UseItemWithItemFromTileToTileCommand(Client.Player, fromPosition, packet.FromIndex, toPosition);
                                    }
                                }
                            }
                            break;

                        case 0x84:
                            {
                                var packet = server.PacketsFactory.Create<UseItemWithCreatureIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new UseItemWithCreatureFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId, packet.CreatureId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new UseItemWithCreatureFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId, packet.CreatureId);
                                }
                                else
                                {
                                    command = new UseItemWithCreatureFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId, packet.CreatureId);
                                }
                            }
                            break;

                        case 0x85:
                            {
                                var packet = server.PacketsFactory.Create<RotateItemIncomingPacket>(reader);

                                var fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new RotateItemFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new RotateItemFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId);
                                }
                                else
                                {
                                    command = new RotateItemFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId);
                                }
                            }
                            break;

                        case 0x87:
                            {
                                var packet = server.PacketsFactory.Create<CloseContainerIncommingPacket>(reader);

                                command = new CloseContainerCommand(Client.Player, packet.ContainerId);
                            }
                            break;

                        case 0x88:
                            {
                                var packet = server.PacketsFactory.Create<OpenParentIncommingPacket>(reader);

                                command = new OpenParentContainerCommand(Client.Player, packet.ContainerId);
                            }
                            break;

                        case 0x8C:
                            {
                                var packet = server.PacketsFactory.Create<LookIncomingPacket>(reader);

                                var fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new LookFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new LookFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId);
                                }
                                else
                                {
                                    command = new LookFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId);
                                }
                            }
                            break;

                        case 0x96:
                            {
                                var packet = server.PacketsFactory.Create<TalkIncommingPacket>(reader);

                                switch (packet.TalkType)
                                {
                                    case TalkType.Say:

                                        command = new SayCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Whisper:

                                        command = new WhisperCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Yell:

                                        command = new YellCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Private:

                                        command = new SendMessageToPlayerCommand(Client.Player, packet.Name, packet.Message);

                                        break;

                                    case TalkType.ChannelYellow:

                                        command = new SendMessageToChannel(Client.Player, packet.ChannelId, packet.Message);

                                        break;

                                    case TalkType.ReportRuleViolationOpen:

                                        command = new CreateReportRuleViolationCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.ReportRuleViolationAnswer:

                                        command = new AnswerInReportRuleViolationChannelCommand(Client.Player, packet.Name, packet.Message);

                                        break;

                                    case TalkType.ReportRuleViolationQuestion:

                                        command = new AskInReportRuleViolationChannelCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Broadcast:

                                        command = new BroadcastMessageCommand(Client.Player, packet.Message);

                                        break;

                                }
                            }
                            break;

                        case 0x97:
                            
                            command = new OpenNewChannelCommand(Client.Player);

                            break;

                        case 0x98:
                            {
                                var packet = server.PacketsFactory.Create<OpenedNewChannelIncomingPacket>(reader);

                                command = new OpenedNewChannelCommand(Client.Player, packet.ChannelId);
                            }
                            break;

                        case 0x99:
                            {
                                var packet = server.PacketsFactory.Create<CloseChannelIncommingPacket>(reader);

                                command = new CloseChannelCommand(Client.Player, packet.ChannelId);
                            }
                            break;

                        case 0x9A:
                            {
                                var packet = server.PacketsFactory.Create<OpenedPrivateChannelIncomingPacket>(reader);

                                command = new OpenedPrivateChannelCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0x9B:
                            {
                                var packet = server.PacketsFactory.Create<ProcessReportRuleViolationIncommingPacket>(reader);

                                command = new ProcessReportRuleViolationCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0x9C:
                            {
                                var packet = server.PacketsFactory.Create<CloseReportRuleViolationChannelAnswerIncommingPacket>(reader);

                                command = new CloseReportRuleViolationChannelAnswerCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0x9D:

                            command = new CloseReportRuleViolationChannelQuestionCommand(Client.Player);
                            
                            break;

                        case 0x9E:

                            command = new CloseNpcsChannelCommand(Client.Player);

                            break;

                        case 0xA0:
                            {
                                var packet = server.PacketsFactory.Create<CombatControlsIncommingPacket>(reader);

                                command = new CombatControlsCommand(Client.Player, packet.FightMode, packet.ChaseMode, packet.SafeMode);
                            }
                            break;

                        case 0xA1:
                            {
                                var packet = server.PacketsFactory.Create<AttackIncommingPacket>(reader);

                                command = new AttackCommand(Client.Player, packet.CreatureId, packet.Nonce);
                            }
                            break;

                        case 0xA2:
                            {
                                var packet = server.PacketsFactory.Create<FollowIncommingPacket>(reader);

                                command = new FollowCommand(Client.Player, packet.CreatureId, packet.Nonce);
                            }
                            break;

                        case 0xA3:
                            {
                                var packet = server.PacketsFactory.Create<InviteToPartyIncomingPacket>(reader);

                                command = new InviteToPartyCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA4:
                            {
                                var packet = server.PacketsFactory.Create<JoinPartyIncomingPacket>(reader);

                                command = new JoinPartyCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA5:
                            {
                                var packet = server.PacketsFactory.Create<RevokePartyIncomingPacket>(reader);

                                command = new RevokePartyCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA6:
                            {
                                var packet = server.PacketsFactory.Create<PassLeadershipToIncomingPacket>(reader);

                                command = new PassLeaderShipToCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA7:
                            
                            command = new LeavePartyCommand(Client.Player);
                            
                            break;

                        case 0xA8:
                            {
                                var packet = server.PacketsFactory.Create<SharedExperienceIncomingPacket>(reader);

                                command = new SharedExperienceCommand(Client.Player, packet.Enabled);
                            }
                            break;

                        case 0xAA:

                            command = new OpenedMyPrivateChannelCommand(Client.Player);

                            break;

                        case 0xAB:
                            {
                                var packet = server.PacketsFactory.Create<InvitePlayerIncommingPacket>(reader);

                                command = new InvitePlayerCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0xAC:
                            {
                                var packet = server.PacketsFactory.Create<ExcludePlayerIncommingPacket>(reader);

                                command = new ExcludePlayerCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0xBE:

                            command = new StopWalkCommand(Client.Player);

                            break;

                        case 0xD2:

                            command = new SelectOutfitCommand(Client.Player);

                            break;

                        case 0xD3:
                            {
                                var packet = server.PacketsFactory.Create<SelectedOutfitIncomingPacket>(reader);

                                command = new SelectedOutfitCommand(Client.Player, packet.Outfit);
                            }
                            break;

                        case 0xDC:
                            {
                                var packet = server.PacketsFactory.Create<AddVipIncommingPacket>(reader);

                                command = new AddVipCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0xDD:
                            {
                                var packet = server.PacketsFactory.Create<RemoveVipIncommingPacket>(reader);

                                command = new RemoveVipCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xE6:
                            {
                                var packet = server.PacketsFactory.Create<ReportBugIncomingPacket>(reader);

                                command = new ReportBugCommand(Client.Player, packet.Message);
                            }
                            break;

                        case 0xF0:
                            
                            command = new OpenQuestsCommand(Client.Player);
                            
                            break;

                        case 0xF1:
                            {
                                var packet = server.PacketsFactory.Create<OpenQuestIncomingPacket>(reader);

                                command = new OpenQuestCommand(Client.Player, packet.QuestId);
                            }
                            break;
                    }

                    if (command != null)
                    {
                        server.QueueForExecution(command);
                    }
                }
            }
            catch (Exception ex)
            {
                server.Logger.WriteLine(ex.ToString() );
            }

            base.OnReceive(body);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            if (e.Type != DisconnetionType.Requested)
            {
                if (Client != null && Client.Player != null)
                {
                    server.QueueForExecution(new LogOutCommand(Client.Player) );
                }
            }
            
            base.OnDisconnected(e);
        }
    }
}
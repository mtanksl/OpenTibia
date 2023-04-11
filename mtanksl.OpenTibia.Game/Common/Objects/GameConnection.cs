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

        public GameConnection(Server server, Socket socket) : base(socket)
        {
            this.server = server;
        }

        protected override void OnConnected()
        {
            server.Logger.WriteLine("Connected on game server", LogLevel.Debug);

            server.QueueForExecution( () =>
            {
                Context context = Context.Current;

                context.AddPacket(this, new SendConnectionInfoOutgoingPacket() );

                return Promise.Completed();
            } );

            base.OnConnected();
        }

        protected override void OnReceived(byte[] body)
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

                    byte identification = reader.ReadByte();

                    switch (identification)
                    {
                        case 0x0A:
                            {
                                var packet = server.PacketsFactory.Create<SelectedCharacterIncomingPacket>(reader);

                                command = new ParseSelectedCharacterCommand(this, packet);
                            }
                            break;

                        case 0x14:

                            command = new ParseLogOutCommand(Client.Player);

                            break;

                        case 0x1E:

                            command = new ParsePongCommand(Client.Player);

                            break;

                        case 0x64:
                            {
                                var packet = server.PacketsFactory.Create<WalkToIncomingPacket>(reader);

                                command = new ParseWalkToKnownPathCommand(Client.Player, packet.MoveDirections);
                            }
                            break;

                        case 0x65:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.North);

                            break;

                        case 0x66:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.East);

                            break;

                        case 0x67:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.South);

                            break;

                        case 0x68:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.West);

                            break;

                        case 0x69:

                            command = new ParseStopWalkCommand(Client.Player);

                            break;

                        case 0x6A:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.NorthEast);

                            break;

                        case 0x6B:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.SouthEast);

                            break;

                        case 0x6C:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.SouthWest);

                            break;

                        case 0x6D:

                            command = new ParseWalkCommand(Client.Player, MoveDirection.NorthWest);

                            break;

                        case 0x6F:

                            command = new ParseTurnCommand(Client.Player, Direction.North);

                            break;

                        case 0x70:

                            command = new ParseTurnCommand(Client.Player, Direction.East);

                            break;

                        case 0x71:

                            command = new ParseTurnCommand(Client.Player, Direction.South);

                            break;

                        case 0x72:

                            command = new ParseTurnCommand(Client.Player, Direction.West);

                            break;

                        case 0x78:
                            {
                                var packet = server.PacketsFactory.Create<MoveItemIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

                                Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

                                if (fromPosition.IsContainer)
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new ParseMoveItemFromContainerToContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromItemId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new ParseMoveItemFromContainerToInventoryCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromItemId, toPosition.InventoryIndex, packet.Count);
                                    }
                                    else
                                    {
                                        command = new ParseMoveItemFromContainerToTileCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromItemId, toPosition, packet.Count);
                                    }
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new ParseMoveItemFromInventoryToContainerCommand(Client.Player, fromPosition.InventoryIndex, packet.FromItemId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new ParseMoveItemFromInventoryToInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.FromItemId, toPosition.InventoryIndex, packet.Count);
                                    }
                                    else
                                    {
                                        command = new ParseMoveItemFromInventoryToTileCommand(Client.Player, fromPosition.InventoryIndex, packet.FromItemId, toPosition, packet.Count);
                                    }
                                }
                                else
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new ParseMoveItemFromTileToContainerCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromItemId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new ParseMoveItemFromTileToInventoryCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromItemId, toPosition.InventoryIndex, packet.Count);
                                    }
                                    else
                                    {
                                        command = new ParseMoveItemFromTileToTileCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromItemId, toPosition, packet.Count);
                                    }
                                }
                            }
                            break;

                        case 0x79:
                            {
                                var packet = server.PacketsFactory.Create<LookItemNpcTradeIncomingPacket>(reader);

                                command = new ParseLookItemNpcTradeCommand(Client.Player, packet.ItemId, packet.Type);
                            }
                            break;

                        case 0x7A:
                            {
                                var packet = server.PacketsFactory.Create<BuyNpcTradeIncomingPacket>(reader);

                                command = new ParseBuyNpcTradeCommand(Client.Player, packet);
                            }
                            break;

                        case 0x7B:
                            {
                                var packet = server.PacketsFactory.Create<SellNpcTradeIncomingPacket>(reader);

                                command = new ParseSellNpcTradeCommand(Client.Player, packet);
                            }
                            break;

                        case 0x7C:
                            
                            command = new ParseCloseNpcTradeCommand(Client.Player);
                            
                            break;

                        case 0x7D:
                            {
                                var packet = server.PacketsFactory.Create<TradeWithIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new ParseTradeWithFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId, packet.CreatureId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new ParseTradeWithFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId, packet.CreatureId);
                                }
                                else
                                {
                                    command = new ParseTradeWithFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId, packet.CreatureId);
                                }
                            }
                            break;

                        case 0x7E:
                            {
                                var packet = server.PacketsFactory.Create<LookItemTradeIncomingPacket>(reader);

                                command = new ParseLookItemTradeCommand(Client.Player, packet.WindowId, packet.Index);
                            }
                            break;

                        case 0x7F:
                            
                            command = new ParseAcceptTradeCommand(Client.Player);
                            
                            break;

                        case 0x80:
                            
                            command = new ParseCancelTradeCommand(Client.Player);
                            
                            break;

                        case 0x82:
                            {
                                var packet = server.PacketsFactory.Create<UseItemIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.X, packet.Y, packet.Z);
                                
                                if (fromPosition.IsContainer)
                                {
                                    command = new ParseUseItemFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId, packet.ContainerId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    if (fromPosition.IsHotkey)
                                    {
                                        command = new ParseUseItemFromHotkeyCommand(Client.Player, packet.ItemId);
                                    }
                                    else
                                    {
                                        command = new ParseUseItemFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId);
                                    }
                                }
                                else
                                {
                                    command = new ParseUseItemFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId);
                                }
                            }
                            break;

                        case 0x83:
                            {
                                var packet = server.PacketsFactory.Create<UseItemWithItemIncomingPacket>(reader);

                                Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

                                Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

                                if (fromPosition.IsContainer)
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new ParseUseItemWithItemFromContainerToContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromItemId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToItemId);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new ParseUseItemWithItemFromContainerToInventoryCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromItemId, toPosition.InventoryIndex, packet.ToItemId);
                                    }
                                    else
                                    {
                                        command = new ParseUseItemWithItemFromContainerToTileCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromItemId, toPosition, packet.ToIndex, packet.ToItemId);
                                    }
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    if (fromPosition.IsHotkey)
                                    {
                                        if (toPosition.IsContainer)
                                        {
                                            command = new ParseUseItemWithItemFromHotkeyToContainerCommand(Client.Player, packet.FromItemId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToItemId);
                                        }
                                        else if (toPosition.IsInventory)
                                        {
                                            command = new ParseUseItemWithItemFromHotkeyToInventoryCommand(Client.Player, packet.FromItemId, toPosition.InventoryIndex, packet.ToItemId);
                                        }
                                        else
                                        {
                                            command = new ParseUseItemWithItemFromHotkeyToTileCommand(Client.Player, packet.FromItemId, toPosition, packet.ToIndex, packet.ToItemId);
                                        }
                                    }
                                    else
                                    {
                                        if (toPosition.IsContainer)
                                        {
                                            command = new ParseUseItemWithItemFromInventoryToContainerCommand(Client.Player, fromPosition.InventoryIndex, packet.FromItemId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToItemId);
                                        }
                                        else if (toPosition.IsInventory)
                                        {
                                            command = new ParseUseItemWithItemFromInventoryToInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.FromItemId, toPosition.InventoryIndex, packet.ToItemId);
                                        }
                                        else
                                        {
                                            command = new ParseUseItemWithItemFromInventoryToTileCommand(Client.Player, fromPosition.InventoryIndex, packet.FromItemId, toPosition, packet.ToIndex, packet.ToItemId);
                                        }
                                    }
                                }
                                else
                                {
                                    if (toPosition.IsContainer)
                                    {
                                        command = new ParseUseItemWithItemFromTileToContainerCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromItemId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToItemId);
                                    }
                                    else if (toPosition.IsInventory)
                                    {
                                        command = new ParseUseItemWithItemFromTileToInventoryCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromItemId, toPosition.InventoryIndex, packet.ToItemId);
                                    }
                                    else
                                    {
                                        command = new ParseUseItemWithItemFromTileToTileCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromItemId, toPosition, packet.ToIndex, packet.ToItemId);
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
                                    command = new ParseUseItemWithCreatureFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId, packet.CreatureId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    if (fromPosition.IsHotkey)
                                    {
                                        command = new ParseUseItemWithCreatureFromHotkeyCommand(Client.Player, packet.ItemId, packet.CreatureId);
                                    }
                                    else
                                    {
                                        command = new ParseUseItemWithCreatureFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId, packet.CreatureId);
                                    }
                                }
                                else
                                {
                                    command = new ParseUseItemWithCreatureFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId, packet.CreatureId);
                                }
                            }
                            break;

                        case 0x85:
                            {
                                var packet = server.PacketsFactory.Create<RotateItemIncomingPacket>(reader);

                                var fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new ParseRotateItemFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new ParseRotateItemFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId);
                                }
                                else
                                {
                                    command = new ParseRotateItemFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId);
                                }
                            }
                            break;

                        case 0x87:
                            {
                                var packet = server.PacketsFactory.Create<CloseContainerIncomingPacket>(reader);

                                command = new ParseCloseContainerCommand(Client.Player, packet.ContainerId);
                            }
                            break;

                        case 0x88:
                            {
                                var packet = server.PacketsFactory.Create<OpenParentContainerIncomingPacket>(reader);

                                command = new ParseOpenParentContainerCommand(Client.Player, packet.ContainerId);
                            }
                            break;

                        case 0x8C:
                            {
                                var packet = server.PacketsFactory.Create<LookIncomingPacket>(reader);

                                var fromPosition = new Position(packet.X, packet.Y, packet.Z);

                                if (fromPosition.IsContainer)
                                {
                                    command = new ParseLookFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.ItemId);
                                }
                                else if (fromPosition.IsInventory)
                                {
                                    command = new ParseLookFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.ItemId);
                                }
                                else
                                {
                                    command = new ParseLookFromTileCommand(Client.Player, fromPosition, packet.Index, packet.ItemId);
                                }
                            }
                            break;

                        case 0x96:
                            {
                                var packet = server.PacketsFactory.Create<TalkIncomingPacket>(reader);

                                switch (packet.TalkType)
                                {
                                    case TalkType.Say:

                                        command = new ParseTalkSayCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Whisper:

                                        command = new ParseTalkWhisperCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Yell:

                                        command = new ParseTalkYellCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Private:

                                        command = new ParseTalkPrivateCommand(Client.Player, packet.Name, packet.Message);

                                        break;

                                    case TalkType.ChannelYellow:

                                        command = new ParseTalkChannelYellowCommand(Client.Player, packet.ChannelId, packet.Message);

                                        break;

                                    case TalkType.ReportRuleViolationOpen:

                                        command = new ParseOpenReportRuleViolationCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.ReportRuleViolationAnswer:

                                        command = new ParseAnswerReportRuleViolationCommand(Client.Player, packet.Name, packet.Message);

                                        break;

                                    case TalkType.ReportRuleViolationQuestion:

                                        command = new ParseQuestionReportRuleViolationCommand(Client.Player, packet.Message);

                                        break;

                                    case TalkType.Broadcast:

                                        command = new ParseTalkBroadcastCommand(Client.Player, packet.Message);

                                        break;
                                }
                            }
                            break;

                        case 0x97:
                            
                            command = new ParseOpenNewChannelCommand(Client.Player);

                            break;

                        case 0x98:
                            {
                                var packet = server.PacketsFactory.Create<OpenedNewChannelIncomingPacket>(reader);

                                command = new ParseOpenedNewChannelCommand(Client.Player, packet.ChannelId);
                            }
                            break;

                        case 0x99:
                            {
                                var packet = server.PacketsFactory.Create<CloseChannelIncomingPacket>(reader);

                                command = new ParseCloseChannelCommand(Client.Player, packet.ChannelId);
                            }
                            break;

                        case 0x9A:
                            {
                                var packet = server.PacketsFactory.Create<OpenedPrivateChannelIncomingPacket>(reader);

                                command = new ParseOpenedPrivateChannelCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0x9B:
                            {
                                var packet = server.PacketsFactory.Create<ProcessReportRuleViolationIncomingPacket>(reader);

                                command = new ParseProcessReportRuleViolationCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0x9C:
                            {
                                var packet = server.PacketsFactory.Create<CloseReportRuleViolationChannelAnswerIncomingPacket>(reader);

                                command = new ParseCloseReportRuleViolationChannelAnswerCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0x9D:

                            command = new ParseCloseReportRuleViolationChannelQuestionCommand(Client.Player);
                            
                            break;

                        case 0x9E:

                            command = new ParseCloseNpcsChannelCommand(Client.Player);

                            break;

                        case 0xA0:
                            {
                                var packet = server.PacketsFactory.Create<CombatControlsIncomingPacket>(reader);

                                command = new ParseCombatControlsCommand(Client.Player, packet.FightMode, packet.ChaseMode, packet.SafeMode);
                            }
                            break;

                        case 0xA1:
                            {
                                var packet = server.PacketsFactory.Create<AttackIncomingPacket>(reader);

                                if (packet.CreatureId == 0)
                                {
                                    command = new ParseStopAttackCommand(Client.Player);
                                }
                                else
                                {
                                    command = new ParseStartAttackCommand(Client.Player, packet.CreatureId, packet.Nonce);
                                }
                            }
                            break;

                        case 0xA2:
                            {
                                var packet = server.PacketsFactory.Create<FollowIncomingPacket>(reader);

                                if (packet.CreatureId == 0)
                                {
                                    command = new ParseStopFollowCommand(Client.Player);
                                }
                                else
                                {
                                    command = new ParseStartFollowCommand(Client.Player, packet.CreatureId, packet.Nonce);
                                }
                            }
                            break;

                        case 0xA3:
                            {
                                var packet = server.PacketsFactory.Create<InviteToPartyIncomingPacket>(reader);

                                command = new ParseInviteToPartyCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA4:
                            {
                                var packet = server.PacketsFactory.Create<JoinPartyIncomingPacket>(reader);

                                command = new ParseJoinPartyCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA5:
                            {
                                var packet = server.PacketsFactory.Create<RevokePartyIncomingPacket>(reader);

                                command = new ParseRevokePartyCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA6:
                            {
                                var packet = server.PacketsFactory.Create<PassLeadershipToIncomingPacket>(reader);

                                command = new ParsePassLeadershipToCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xA7:
                            
                            command = new ParseLeavePartyCommand(Client.Player);
                            
                            break;

                        case 0xA8:
                            {
                                var packet = server.PacketsFactory.Create<SharedExperienceIncomingPacket>(reader);

                                command = new ParseSharedExperienceCommand(Client.Player, packet.Enabled);
                            }
                            break;

                        case 0xAA:

                            command = new ParseOpenedMyPrivateChannelCommand(Client.Player);

                            break;

                        case 0xAB:
                            {
                                var packet = server.PacketsFactory.Create<InvitePlayerIncomingPacket>(reader);

                                command = new ParseInvitePlayerChannelCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0xAC:
                            {
                                var packet = server.PacketsFactory.Create<ExcludePlayerIncomingPacket>(reader);

                                command = new ParseExcludePlayerCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0xBE:

                            command = new ParseStopCommand(Client.Player);

                            break;

                        case 0xD2:

                            command = new ParseSetOutfitCommand(Client.Player);

                            break;

                        case 0xD3:
                            {
                                var packet = server.PacketsFactory.Create<SelectedOutfitIncomingPacket>(reader);

                                command = new ParseSelectedOutfitCommand(Client.Player, packet.Outfit);
                            }
                            break;

                        case 0xDC:
                            {
                                var packet = server.PacketsFactory.Create<AddVipIncomingPacket>(reader);

                                command = new ParseAddVipCommand(Client.Player, packet.Name);
                            }
                            break;

                        case 0xDD:
                            {
                                var packet = server.PacketsFactory.Create<RemoveVipIncomingPacket>(reader);

                                command = new ParseRemoveVipCommand(Client.Player, packet.CreatureId);
                            }
                            break;

                        case 0xE6:
                            {
                                var packet = server.PacketsFactory.Create<ReportBugIncomingPacket>(reader);

                                command = new ParseReportBugCommand(Client.Player, packet.Message);
                            }
                            break;

                        case 0xF0:
                            
                            command = new ParseQuestsCommand(Client.Player);
                            
                            break;

                        case 0xF1:
                            {
                                var packet = server.PacketsFactory.Create<OpenQuestIncomingPacket>(reader);

                                command = new ParseOpenQuestCommand(Client.Player, packet.QuestId);
                            }
                            break;

                        case 0xF2:
                            {
                                var packet = server.PacketsFactory.Create<ReportRuleViolationIncomingPacket>(reader);

                                command = new ParseReportRuleViolationCommand(Client.Player, packet);
                            }
                            break;
                    }

                    if (command != null)
                    {
                        server.Logger.WriteLine("Received on game server: 0x" + identification.ToString("X2"), LogLevel.Debug);

                        server.QueueForExecution( () =>
                        {
                            Context context = Context.Current;

                            return context.AddCommand(command);
                        } );
                    }
                    else
                    {
                        server.Logger.WriteLine("Unknown packet received on game server: 0x" + identification.ToString("X2"), LogLevel.Warning);

                        server.Logger.WriteLine(body.Print(), LogLevel.Warning);
                    }
                }
                else
                {
                    server.Logger.WriteLine("Invalid message received on game server.", LogLevel.Warning);

                    server.Logger.WriteLine(body.Print(), LogLevel.Warning);
                }
            }
            catch (Exception ex)
            {
                server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
            }

            base.OnReceived(body);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            server.Logger.WriteLine("Disconnected on game server", LogLevel.Debug);

            if (e.Type != DisconnectionType.Requested)
            {
                server.QueueForExecution( () =>
                {
                    Context context = Context.Current;

                    return context.AddCommand(new ParseLogOutCommand(Client.Player) );
                } );
            }
            
            base.OnDisconnected(e);
        }
    }
}
using OpenTibia.Common.Structures;
using OpenTibia.Game;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;
using OpenTibia.Network.Sockets;
using OpenTibia.Security;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace OpenTibia.Common.Objects
{
    public class GameConnection : RateLimitingConnection
    {
        private IServer server;

        public GameConnection(IServer server, Socket socket) : base(server, socket)
        {
            this.server = server;

            firstCommands.Add(0x0A, new PacketToCommand<SelectedCharacterIncomingPacket>(packet => new ParseSelectedCharacterCommand(this, packet) ) );

            commands.Add(0x14, new PacketToCommand<LogOutIncomingPacket>(packet => new ParseLogOutCommand(Client.Player) ) );

			commands.Add(0x1E, new PacketToCommand<PongIncomingPacket>(packet => new ParsePongCommand(Client.Player) ) );

			commands.Add(0x64, new PacketToCommand<WalkToIncomingPacket>(packet => new ParseWalkToKnownPathCommand(Client.Player, packet.MoveDirections) ) );

			commands.Add(0x65, new PacketToCommand<WalkNorthIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x66, new PacketToCommand<WalkEastIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x67, new PacketToCommand<WalkSouthIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x68, new PacketToCommand<WalkWestIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x69, new PacketToCommand<StopWalkIncomingPacket>(packet => new ParseStopWalkCommand(Client.Player) ) );

			commands.Add(0x6A, new PacketToCommand<WalkNorthEastIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x6B, new PacketToCommand<WalkSouthEastIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x6C, new PacketToCommand<WalkSouthWestIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x6D, new PacketToCommand<WalkNorthWestIncomingPacket>(packet => new ParseWalkCommand(Client.Player, packet.MoveDirection) ) );

			commands.Add(0x6F, new PacketToCommand<TurnNorthIncomingPacket>(packet => new ParseTurnCommand(Client.Player, packet.Direction) ) );

			commands.Add(0x70, new PacketToCommand<TurnEastIncomingPacket>(packet => new ParseTurnCommand(Client.Player, packet.Direction) ) );

			commands.Add(0x71, new PacketToCommand<TurnSoutIncomingPacketh>(packet => new ParseTurnCommand(Client.Player, packet.Direction) ) );

			commands.Add(0x72, new PacketToCommand<TurnWestIncomingPacket>(packet => new ParseTurnCommand(Client.Player, packet.Direction) ) );

			commands.Add(0x78, new PacketToCommand<MoveItemIncomingPacket>(packet => 
			{
				Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

				Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

				if (fromPosition.IsContainer)
				{
					if (toPosition.IsContainer)
					{
						return new ParseMoveItemFromContainerToContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseMoveItemFromContainerToInventoryCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.Count);
					}
					else
					{
						return new ParseMoveItemFromContainerToTileCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition, packet.Count);
					}
				}
				else if (fromPosition.IsInventory)
				{
					if (toPosition.IsContainer)
					{
						return new ParseMoveItemFromInventoryToContainerCommand(Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseMoveItemFromInventoryToInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.Count);
					}
					else
					{
						return new ParseMoveItemFromInventoryToTileCommand(Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition, packet.Count);
					}
				}
				else
				{
					if (toPosition.IsContainer)
					{
						return new ParseMoveItemFromTileToContainerCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseMoveItemFromTileToInventoryCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.Count);
					}
					else
					{
						return new ParseMoveItemFromTileToTileCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition, packet.Count);
					}
				}
			} ) );

			commands.Add(0x79, new PacketToCommand<LookItemNpcTradeIncomingPacket>(packet => new ParseLookItemNpcTradeCommand(Client.Player, packet.TibiaId, packet.Type) ) );

			commands.Add(0x7A, new PacketToCommand<BuyNpcTradeIncomingPacket>(packet => new ParseBuyNpcTradeCommand(Client.Player, packet) ) );

			commands.Add(0x7B, new PacketToCommand<SellNpcTradeIncomingPacket>(packet => new ParseSellNpcTradeCommand(Client.Player, packet) ) );

			commands.Add(0x7C, new PacketToCommand<CloseNpcTradeIncomingPacket>(packet => new ParseCloseNpcTradeCommand(Client.Player) ) );

			commands.Add(0x7D, new PacketToCommand<TradeWithIncomingPacket>(packet =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseTradeWithFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId, packet.CreatureId);
				}
				else if (fromPosition.IsInventory)
				{
					return new ParseTradeWithFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.TibiaId, packet.CreatureId);
				}
				else
				{
					return new ParseTradeWithFromTileCommand(Client.Player, fromPosition, packet.Index, packet.TibiaId, packet.CreatureId);
				}
			} ) );

			commands.Add(0x7E, new PacketToCommand<LookItemTradeIncomingPacket>(packet => new ParseLookItemTradeCommand(Client.Player, packet.WindowId, packet.Index) ) );

			commands.Add(0x7F, new PacketToCommand<AcceptTradeIncomingPacket>(packet => new ParseAcceptTradeCommand(Client.Player) ) );

			commands.Add(0x80, new PacketToCommand<CancelOrRejectTradeIncomingPacket>(packet => new ParseCancelOrRejectTradeCommand(Client.Player) ) );

			commands.Add(0x82, new PacketToCommand<UseItemIncomingPacket>(packet => 
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseUseItemFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId, packet.ContainerId);
				}
				else if (fromPosition.IsInventory)
				{
					if (fromPosition.IsHotkey)
					{
						return new ParseUseItemFromHotkeyCommand(Client.Player, packet.TibiaId);
					}
					else
					{
						return new ParseUseItemFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.TibiaId);
					}
				}
				else
				{
					return new ParseUseItemFromTileCommand(Client.Player, fromPosition, packet.Index, packet.TibiaId);
				}
			} ) );

			commands.Add(0x83, new PacketToCommand<UseItemWithItemIncomingPacket>(packet =>
			{
				Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

				Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

				if (fromPosition.IsContainer)
				{
					if (toPosition.IsContainer)
					{
						return new ParseUseItemWithItemFromContainerToContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseUseItemWithItemFromContainerToInventoryCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
					}
					else
					{
						return new ParseUseItemWithItemFromContainerToTileCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
					}
				}
				else if (fromPosition.IsInventory)
				{
					if (fromPosition.IsHotkey)
					{
						if (toPosition.IsContainer)
						{
							return new ParseUseItemWithItemFromHotkeyToContainerCommand(Client.Player, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
						}
						else if (toPosition.IsInventory)
						{
							return new ParseUseItemWithItemFromHotkeyToInventoryCommand(Client.Player, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
						}
						else
						{
							return new ParseUseItemWithItemFromHotkeyToTileCommand(Client.Player, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
						}
					}
					else
					{
						if (toPosition.IsContainer)
						{
							return new ParseUseItemWithItemFromInventoryToContainerCommand(Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
						}
						else if (toPosition.IsInventory)
						{
							return new ParseUseItemWithItemFromInventoryToInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
						}
						else
						{
							return new ParseUseItemWithItemFromInventoryToTileCommand(Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
						}
					}
				}
				else
				{
					if (toPosition.IsContainer)
					{
						return new ParseUseItemWithItemFromTileToContainerCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseUseItemWithItemFromTileToInventoryCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
					}
					else
					{
						return new ParseUseItemWithItemFromTileToTileCommand(Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
					}
				}
			} ) );

			commands.Add(0x84, new PacketToCommand<UseItemWithCreatureIncomingPacket>(packet =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseUseItemWithCreatureFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId, packet.CreatureId);
				}
				else if (fromPosition.IsInventory)
				{
					if (fromPosition.IsHotkey)
					{
						return new ParseUseItemWithCreatureFromHotkeyCommand(Client.Player, packet.TibiaId, packet.CreatureId);
					}
					else
					{
						return new ParseUseItemWithCreatureFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.TibiaId, packet.CreatureId);
					}
				}
				else
				{
					return new ParseUseItemWithCreatureFromTileCommand(Client.Player, fromPosition, packet.Index, packet.TibiaId, packet.CreatureId);
				}
			} ) );

			commands.Add(0x85, new PacketToCommand<RotateItemIncomingPacket>(packet =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseRotateItemFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId);
				}
				else if (fromPosition.IsInventory)
				{
					return new ParseRotateItemFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.TibiaId);
				}
				else
				{
					return new ParseRotateItemFromTileCommand(Client.Player, fromPosition, packet.Index, packet.TibiaId);
				}
			} ) );

			commands.Add(0x87, new PacketToCommand<CloseContainerIncomingPacket>(packet => new ParseCloseContainerCommand(Client.Player, packet.ContainerId) ) );

			commands.Add(0x88, new PacketToCommand<OpenParentContainerIncomingPacket>(packet => new ParseOpenParentContainerCommand(Client.Player, packet.ContainerId) ) );

			commands.Add(0x89, new PacketToCommand<EditTextDialogIncomingPacket>(packet => new ParseEditTextDialogCommand(Client.Player, packet.WindowId, packet.Text) ) );

			commands.Add(0x8A, new PacketToCommand<EditListDialogIncomingPacket>(packet => new ParseEditListDialogCommand(Client.Player, packet.DoorId, packet.WindowId, packet.Text) ) );

			commands.Add(0x8C, new PacketToCommand<LookIncomingPacket>(packet =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseLookFromContainerCommand(Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId);
				}
				else if (fromPosition.IsInventory)
				{
					return new ParseLookFromInventoryCommand(Client.Player, fromPosition.InventoryIndex, packet.TibiaId);
				}
				else
				{
					return new ParseLookFromTileCommand(Client.Player, fromPosition, packet.Index, packet.TibiaId);
				}
			} ) );

			commands.Add(0x96, new PacketToCommand<TalkIncomingPacket>(packet =>
			{
				switch (packet.TalkType)
				{
					case TalkType.Say:

						return new ParseTalkSayCommand(Client.Player, packet.Message);
			
					case TalkType.Whisper:

						return new ParseTalkWhisperCommand(Client.Player, packet.Message);

					case TalkType.Yell:

						return new ParseTalkYellCommand(Client.Player, packet.Message);

					case TalkType.PrivatePlayerToNpc:

						return new ParseTalkPrivatePlayerToNpcCommand(Client.Player, packet.Message);

					case TalkType.Private:

						return new ParseTalkPrivateCommand(Client.Player, packet.Name, packet.Message);

					case TalkType.ChannelYellow:

						return new ParseTalkChannelYellowCommand(Client.Player, packet.ChannelId, packet.Message);

					case TalkType.ReportRuleViolationOpen:

						return new ParseOpenReportRuleViolationCommand(Client.Player, packet.Message);

					case TalkType.ReportRuleViolationAnswer:

						return new ParseAnswerReportRuleViolationCommand(Client.Player, packet.Name, packet.Message);

					case TalkType.ReportRuleViolationQuestion:

						return new ParseQuestionReportRuleViolationCommand(Client.Player, packet.Message);

					case TalkType.Broadcast:

						return new ParseTalkBroadcastCommand(Client.Player, packet.Message);
			
					case TalkType.ChannelRedAnonymous:

						return new ParseTalkChannelRedAnonymousCommand(Client.Player, packet.ChannelId, packet.Message);
				}
	
				throw new NotImplementedException();
			} ) );

			commands.Add(0x97, new PacketToCommand<OpenNewChannelIncomingPacket>(packet => new ParseOpenNewChannelCommand(Client.Player) ) );

			commands.Add(0x98, new PacketToCommand<OpenedNewChannelIncomingPacket>(packet => new ParseOpenedNewChannelCommand(Client.Player, packet.ChannelId) ) );

			commands.Add(0x99, new PacketToCommand<CloseChannelIncomingPacket>(packet => new ParseCloseChannelCommand(Client.Player, packet.ChannelId) ) );

			commands.Add(0x9A, new PacketToCommand<OpenedPrivateChannelIncomingPacket>(packet => new ParseOpenedPrivateChannelCommand(Client.Player, packet.Name) ) );

			commands.Add(0x9B, new PacketToCommand<ProcessReportRuleViolationIncomingPacket>(packet => new ParseProcessReportRuleViolationCommand(Client.Player, packet.Name) ) );

			commands.Add(0x9C, new PacketToCommand<CloseReportRuleViolationChannelAnswerIncomingPacket>(packet => new ParseCloseReportRuleViolationChannelAnswerCommand(Client.Player, packet.Name) ) );

			commands.Add(0x9D, new PacketToCommand<CloseReportRuleViolationChannelQuestionIncomingPacket>(packet => new ParseCloseReportRuleViolationChannelQuestionCommand(Client.Player) ) );

			commands.Add(0x9E, new PacketToCommand<CloseNpcsChannelIncomingPacket>(packet => new ParseCloseNpcsChannelCommand(Client.Player) ) );

			commands.Add(0xA0, new PacketToCommand<CombatControlsIncomingPacket>(packet => new ParseCombatControlsCommand(Client.Player, packet.FightMode, packet.ChaseMode, packet.SafeMode) ) );

			commands.Add(0xA1, new PacketToCommand<AttackIncomingPacket>(packet =>
			{
				if (packet.CreatureId == 0)
				{
					return new ParseStopAttackCommand(Client.Player);
				}
				else
				{
					return new ParseStartAttackCommand(Client.Player, packet.CreatureId, packet.Nonce);
				}
			} ) );

			commands.Add(0xA2, new PacketToCommand<FollowIncomingPacket>(packet =>
			{
				if (packet.CreatureId == 0)
				{
					return new ParseStopFollowCommand(Client.Player);
				}
				else
				{
					return new ParseStartFollowCommand(Client.Player, packet.CreatureId, packet.Nonce);
				}
			} ) );

			commands.Add(0xA3, new PacketToCommand<InviteToPartyIncomingPacket>(packet => new ParseInviteToPartyCommand(Client.Player, packet.CreatureId) ) );

			commands.Add(0xA4, new PacketToCommand<JoinPartyIncomingPacket>(packet => new ParseJoinPartyCommand(Client.Player, packet.CreatureId) ) );

			commands.Add(0xA5, new PacketToCommand<RevokePartyIncomingPacket>(packet => new ParseRevokePartyCommand(Client.Player, packet.CreatureId) ) );

			commands.Add(0xA6, new PacketToCommand<PassLeadershipToIncomingPacket>(packet => new ParsePassLeadershipToCommand(Client.Player, packet.CreatureId) ) );

			commands.Add(0xA7, new PacketToCommand<LeavePartyIncomingPacket>(packet => new ParseLeavePartyCommand(Client.Player) ) );

			commands.Add(0xA8, new PacketToCommand<SharedExperienceIncomingPacket>(packet => new ParseSharedExperienceCommand(Client.Player, packet.Enabled) ) );

			commands.Add(0xAA, new PacketToCommand<OpenedMyPrivateChannelIncomingPacket>(packet => new ParseOpenedMyPrivateChannelCommand(Client.Player) ) );

			commands.Add(0xAB, new PacketToCommand<InvitePlayerIncomingPacket>(packet => new ParseInvitePlayerChannelCommand(Client.Player, packet.Name) ) );

			commands.Add(0xAC, new PacketToCommand<ExcludePlayerIncomingPacket>(packet => new ParseExcludePlayerCommand(Client.Player, packet.Name) ) );

			commands.Add(0xBE, new PacketToCommand<StopIncomingPacket>(packet => new ParseStopCommand(Client.Player) ) );

            // 0xC9 - Update Tile

            // 0xCA - Update Container

			commands.Add(0xD2, new PacketToCommand<SetOutfitIncomingPacket>(packet => new ParseSetOutfitCommand(Client.Player) ) );

			commands.Add(0xD3, new PacketToCommand<SelectedOutfitIncomingPacket>(packet => new ParseSelectedOutfitCommand(Client.Player, packet.Outfit) ) );

			commands.Add(0xDC, new PacketToCommand<AddVipIncomingPacket>(packet => new ParseAddVipCommand(Client.Player, packet.Name) ) );

			commands.Add(0xDD, new PacketToCommand<RemoveVipIncomingPacket>(packet => new ParseRemoveVipCommand(Client.Player, packet.CreatureId) ) );

			commands.Add(0xE6, new PacketToCommand<ReportBugIncomingPacket>(packet => new ParseReportBugCommand(Client.Player, packet.Message) ) );

			commands.Add(0xE8, new PacketToCommand<DebugAssertIncomingPacket>(packet => new ParseDebugAssertCommand(Client.Player, packet.AssertLine, packet.ReportDate, packet.Description, packet.Comment) ) );

			commands.Add(0xF0, new PacketToCommand<QuestsIncomingPacket>(packet => new ParseQuestsCommand(Client.Player) ) );

			commands.Add(0xF1, new PacketToCommand<OpenQuestIncomingPacket>(packet => new ParseOpenQuestCommand(Client.Player, packet.QuestId) ) );

			commands.Add(0xF2, new PacketToCommand<ReportRuleViolationIncomingPacket>(packet => new ParseReportRuleViolationCommand(Client.Player, packet.Type, packet.RuleViolation, packet.Name, packet.Comment, packet.Translation, packet.StatmentId) ) );
        }

        protected override void OnConnected()
        {
            server.Logger.WriteLine("Connected on game server", LogLevel.Debug);

            server.QueueForExecution( () =>
            {
                Context.Current.AddPacket(this, new SendConnectionInfoOutgoingPacket(0) );

                return Promise.Completed;
            } );

            base.OnConnected();
        }

        private bool first = true;

        private Dictionary<byte, IPacketToCommand> firstCommands = new Dictionary<byte, IPacketToCommand>();

        private Dictionary<byte, IPacketToCommand> commands = new Dictionary<byte, IPacketToCommand>();

        protected override void OnReceived(byte[] body, int length)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(body);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                if (Adler32.Generate(body, 4, length - 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        Rsa.DecryptAndReplace(body, 9, length - 9);
                    }
                    else
                    {
                        Xtea.DecryptAndReplace(body, 4, length - 4, 32, Keys);

                        stream.Seek(Origin.Current, 2);
                    }

                    byte identification = reader.ReadByte();

                    if (first)
                    {
                        first = false;

                        if (firstCommands.TryGetValue(identification, out var packetToCommand) )
                        {
                            Command command = packetToCommand.Convert(reader);

                            server.Logger.WriteLine("Received on game server: 0x" + identification.ToString("X2"), LogLevel.Debug);

                            server.QueueForExecution( () =>
                            {
                                return Context.Current.AddCommand(command);
                            } );
                        }
                        else
                        {                               
                            IncreaseUnknownPacket();

                            server.Logger.WriteLine("Unknown packet received on game server: 0x" + identification.ToString("X2"), LogLevel.Warning);

                            server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                        }
                    }
                    else
                    {
                        if (Client == null || Client.Player == null || Client.Player.Tile == null || Client.Player.IsDestroyed)
                        {
                            Disconnect();
                        }
                        else
                        {
                            if (commands.TryGetValue(identification, out var packetToCommand) )
                            {
                                Command command = packetToCommand.Convert(reader);

                                server.Logger.WriteLine("Received on game server: 0x" + identification.ToString("X2"), LogLevel.Debug);

                                server.QueueForExecution( () =>
                                {
                                    return Context.Current.AddCommand(command);
                                } );
                            }
                            else
                            {                         
                                IncreaseUnknownPacket();

                                server.Logger.WriteLine("Unknown packet received on game server: 0x" + identification.ToString("X2"), LogLevel.Warning);

                                server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                            }
                        }
                    }
                }
                else
                {
                    IncreaseInvalidMessage();

                    server.Logger.WriteLine("Invalid message received on game server", LogLevel.Warning);

                    server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
                }
            }
            catch
            {
                IncreaseInvalidMessage();

                server.Logger.WriteLine("Invalid message received on game server", LogLevel.Warning);

                server.Logger.WriteLine(body.Print(0, length), LogLevel.Warning);
            }

            base.OnReceived(body, length);
        }

        protected override void OnDisconnected(DisconnectedEventArgs e)
        {
            server.Logger.WriteLine("Disconnected on game server", LogLevel.Debug);

            if (Client == null || Client.Player == null || Client.Player.Tile == null || Client.Player.IsDestroyed)
            {
                    
            }
            else
            {
                server.QueueForExecution( () =>
                {
                    return Context.Current.AddCommand(new ParseLogOutCommand(Client.Player) );
                } );
            }
            
            base.OnDisconnected(e);
        }
    }
}
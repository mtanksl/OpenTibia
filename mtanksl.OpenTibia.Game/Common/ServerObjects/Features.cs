﻿using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Incoming;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Features : IFeatures
    {
        private Server server;

        public Features(Server server)
        {
            this.server = server;
        }

        public void Start()
        {
			if (server.Config.ClientVersion == new Version(7, 72) )
			{
                clientVersion = 772;

                tibiaDat = 1134385715;

                tibiaPic = 1146144984;

                tibiaSpr = 1134056126;
            }
            else if (server.Config.ClientVersion == new Version(8, 60) )
            {
                clientVersion = 860;

				tibiaDat = 1277983123;

				tibiaPic = 1256571859;

				tibiaSpr = 1277298068;				
            }
            else
            {
                throw new NotImplementedException();
            }

			if (clientVersion >= 770)
			{
				featureFlags.Add(FeatureFlag.LookTypeUInt16);
				featureFlags.Add(FeatureFlag.MessageStatement);
				//featureFlags.Add(FeatureFlag.LoginPacketEncryption);
			}

			if (clientVersion >= 780) 
			{
				featureFlags.Add(FeatureFlag.PlayerAddons);
				featureFlags.Add(FeatureFlag.PlayerStamina);
				//featureFlags.Add(FeatureFlag.NewFluids);
				featureFlags.Add(FeatureFlag.MessageLevel);
				featureFlags.Add(FeatureFlag.PlayerSpecialConditionUInt16);
				featureFlags.Add(FeatureFlag.NewOutfitProtocol);
			}

			if (clientVersion >= 790)
			{
				featureFlags.Add(FeatureFlag.ReadableItemDate);
			}

			if (clientVersion >= 840) 
			{
				featureFlags.Add(FeatureFlag.ProtocolChecksum);
				featureFlags.Add(FeatureFlag.AccountString);
				featureFlags.Add(FeatureFlag.PlayerCapacityUInt32);
			}

			if (clientVersion >= 841) 
			{
				featureFlags.Add(FeatureFlag.ChallengeOnLogin);
				//featureFlags.Add(FeatureFlag.MessageSizeCheck);
				featureFlags.Add(FeatureFlag.TileIndex);
			}

			if (clientVersion >= 854) 
			{
				featureFlags.Add(FeatureFlag.CreatureWarIcon);
				featureFlags.Add(FeatureFlag.CreatureBlock);
            }

			if (clientVersion >= 860) 
			{
				featureFlags.Add(FeatureFlag.AttackSequence);
			}

			loginFirstCommands.Add(0x01, new PacketToCommand<EnterGameIncomingPacket>("Enter Game",(connection, packet) => new ParseEnterGameCommand(connection, packet) ) );

            gameFirstCommands.Add(0x0A, new PacketToCommand<SelectedCharacterIncomingPacket>("Selected Character", (connection, packet) => new ParseSelectedCharacterCommand(connection, packet) ) );

			gameCommands.Add(0x14, new PacketToCommand<LogOutIncomingPacket>("Log Out", (connection, packet) => new ParseLogOutCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x1E, new PacketToCommand<PongIncomingPacket>("Pong", (connection, packet) => new ParsePongCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x64, new PacketToCommand<WalkToIncomingPacket>("Walk To", (connection, packet) => new ParseWalkToCommand(connection.Client.Player, packet.MoveDirections) ) );
			
			gameCommands.Add(0x65, new PacketToCommand<WalkNorthIncomingPacket>("Walk North", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x66, new PacketToCommand<WalkEastIncomingPacket>("Walk East", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x67, new PacketToCommand<WalkSouthIncomingPacket>("Walk South", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x68, new PacketToCommand<WalkWestIncomingPacket>("Walk West", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x69, new PacketToCommand<StopWalkIncomingPacket>("Stop Walk", (connection, packet) => new ParseStopWalkCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x6A, new PacketToCommand<WalkNorthEastIncomingPacket>("Walk North East", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x6B, new PacketToCommand<WalkSouthEastIncomingPacket>("Walk South East", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x6C, new PacketToCommand<WalkSouthWestIncomingPacket>("Walk South West", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x6D, new PacketToCommand<WalkNorthWestIncomingPacket>("Walk North West", (connection, packet) => new ParseWalkCommand(connection.Client.Player, packet.MoveDirection) ) );
			
			gameCommands.Add(0x6F, new PacketToCommand<TurnNorthIncomingPacket>("Turn North", (connection, packet) => new ParseTurnCommand(connection.Client.Player, packet.Direction) ) );
			
			gameCommands.Add(0x70, new PacketToCommand<TurnEastIncomingPacket>("Turn East", (connection, packet) => new ParseTurnCommand(connection.Client.Player, packet.Direction) ) );
			
			gameCommands.Add(0x71, new PacketToCommand<TurnSouthIncomingPacketh>("Turn South", (connection, packet) => new ParseTurnCommand(connection.Client.Player, packet.Direction) ) );
			
			gameCommands.Add(0x72, new PacketToCommand<TurnWestIncomingPacket>("Turn West", (connection, packet) => new ParseTurnCommand(connection.Client.Player, packet.Direction) ) );
			
			gameCommands.Add(0x78, new PacketToCommand<MoveItemIncomingPacket>("Move Item", (connection, packet) => 
			{
				Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

				Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

				if (fromPosition.IsContainer)
				{
					if (toPosition.IsContainer)
					{
						return new ParseMoveItemFromContainerToContainerCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseMoveItemFromContainerToInventoryCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.Count);
					}
					else
					{
						return new ParseMoveItemFromContainerToTileCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition, packet.Count);
					}
				}
				else if (fromPosition.IsInventory)
				{
					if (toPosition.IsContainer)
					{
						return new ParseMoveItemFromInventoryToContainerCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseMoveItemFromInventoryToInventoryCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.Count);
					}
					else
					{
						return new ParseMoveItemFromInventoryToTileCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition, packet.Count);
					}
				}
				else
				{
					if (toPosition.IsContainer)
					{
						return new ParseMoveItemFromTileToContainerCommand(connection.Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.Count);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseMoveItemFromTileToInventoryCommand(connection.Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.Count);
					}
					else
					{
						return new ParseMoveItemFromTileToTileCommand(connection.Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition, packet.Count);
					}
				}
			} ) );
			
			gameCommands.Add(0x79, new PacketToCommand<LookItemNpcTradeIncomingPacket>("Look Item Npc Trade", (connection, packet) => new ParseLookItemNpcTradeCommand(connection.Client.Player, packet.TibiaId, packet.Type) ) );
			
			gameCommands.Add(0x7A, new PacketToCommand<BuyNpcTradeIncomingPacket>("Buy Npc Trade", (connection, packet) => new ParseBuyNpcTradeCommand(connection.Client.Player, packet) ) );
			
			gameCommands.Add(0x7B, new PacketToCommand<SellNpcTradeIncomingPacket>("Sell Npc Trade", (connection, packet) => new ParseSellNpcTradeCommand(connection.Client.Player, packet) ) );
			
			gameCommands.Add(0x7C, new PacketToCommand<CloseNpcTradeIncomingPacket>("Close Npc Trade", (connection, packet) => new ParseCloseNpcTradeCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x7D, new PacketToCommand<TradeWithIncomingPacket>("Trade With", (connection, packet) =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseTradeWithFromContainerCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId, packet.CreatureId);
				}
				else if (fromPosition.IsInventory)
				{
					return new ParseTradeWithFromInventoryCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.TibiaId, packet.CreatureId);
				}
				else
				{
					return new ParseTradeWithFromTileCommand(connection.Client.Player, fromPosition, packet.Index, packet.TibiaId, packet.CreatureId);
				}
			} ) );
			
			gameCommands.Add(0x7E, new PacketToCommand<LookItemTradeIncomingPacket>("Look Item Trade", (connection, packet) => new ParseLookItemTradeCommand(connection.Client.Player, packet.WindowId, packet.Index) ) );
			
			gameCommands.Add(0x7F, new PacketToCommand<AcceptTradeIncomingPacket>("Accept Trade", (connection, packet) => new ParseAcceptTradeCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x80, new PacketToCommand<CancelOrRejectTradeIncomingPacket>("Cancel Or Reject Trade", (connection, packet) => new ParseCancelOrRejectTradeCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x82, new PacketToCommand<UseItemIncomingPacket>("Use Item", (connection, packet) => 
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseUseItemFromContainerCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId, packet.ContainerId);
				}
				else if (fromPosition.IsInventory)
				{
					if (fromPosition.IsHotkey)
					{
						return new ParseUseItemFromHotkeyCommand(connection.Client.Player, packet.TibiaId);
					}
					else
					{
						return new ParseUseItemFromInventoryCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.TibiaId);
					}
				}
				else
				{
					return new ParseUseItemFromTileCommand(connection.Client.Player, fromPosition, packet.Index, packet.TibiaId);
				}
			} ) );
			
			gameCommands.Add(0x83, new PacketToCommand<UseItemWithItemIncomingPacket>("Use Item With Item", (connection, packet) =>
			{
				Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

				Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

				if (fromPosition.IsContainer)
				{
					if (toPosition.IsContainer)
					{
						return new ParseUseItemWithItemFromContainerToContainerCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseUseItemWithItemFromContainerToInventoryCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
					}
					else
					{
						return new ParseUseItemWithItemFromContainerToTileCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
					}
				}
				else if (fromPosition.IsInventory)
				{
					if (fromPosition.IsHotkey)
					{
						if (toPosition.IsContainer)
						{
							return new ParseUseItemWithItemFromHotkeyToContainerCommand(connection.Client.Player, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
						}
						else if (toPosition.IsInventory)
						{
							return new ParseUseItemWithItemFromHotkeyToInventoryCommand(connection.Client.Player, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
						}
						else
						{
							return new ParseUseItemWithItemFromHotkeyToTileCommand(connection.Client.Player, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
						}
					}
					else
					{
						if (toPosition.IsContainer)
						{
							return new ParseUseItemWithItemFromInventoryToContainerCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
						}
						else if (toPosition.IsInventory)
						{
							return new ParseUseItemWithItemFromInventoryToInventoryCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
						}
						else
						{
							return new ParseUseItemWithItemFromInventoryToTileCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
						}
					}
				}
				else
				{
					if (toPosition.IsContainer)
					{
						return new ParseUseItemWithItemFromTileToContainerCommand(connection.Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.ContainerId, toPosition.ContainerIndex, packet.ToTibiaId);
					}
					else if (toPosition.IsInventory)
					{
						return new ParseUseItemWithItemFromTileToInventoryCommand(connection.Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition.InventoryIndex, packet.ToTibiaId);
					}
					else
					{
						return new ParseUseItemWithItemFromTileToTileCommand(connection.Client.Player, fromPosition, packet.FromIndex, packet.FromTibiaId, toPosition, packet.ToIndex, packet.ToTibiaId);
					}
				}
			} ) );
			
			gameCommands.Add(0x84, new PacketToCommand<UseItemWithCreatureIncomingPacket>("Use Item With Creature", (connection, packet) =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseUseItemWithCreatureFromContainerCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId, packet.CreatureId);
				}
				else if (fromPosition.IsInventory)
				{
					if (fromPosition.IsHotkey)
					{
						return new ParseUseItemWithCreatureFromHotkeyCommand(connection.Client.Player, packet.TibiaId, packet.CreatureId);
					}
					else
					{
						return new ParseUseItemWithCreatureFromInventoryCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.TibiaId, packet.CreatureId);
					}
				}
				else
				{
					return new ParseUseItemWithCreatureFromTileCommand(connection.Client.Player, fromPosition, packet.Index, packet.TibiaId, packet.CreatureId);
				}
			} ) );
			
			gameCommands.Add(0x85, new PacketToCommand<RotateItemIncomingPacket>("Rotate Item", (connection, packet) =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseRotateItemFromContainerCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId);
				}
				else if (fromPosition.IsInventory)
				{
					return new ParseRotateItemFromInventoryCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.TibiaId);
				}
				else
				{
					return new ParseRotateItemFromTileCommand(connection.Client.Player, fromPosition, packet.Index, packet.TibiaId);
				}
			} ) );
			
			gameCommands.Add(0x87, new PacketToCommand<CloseContainerIncomingPacket>("Close Container", (connection, packet) => new ParseCloseContainerCommand(connection.Client.Player, packet.ContainerId) ) );
			
			gameCommands.Add(0x88, new PacketToCommand<OpenParentContainerIncomingPacket>("Open Parent Container", (connection, packet) => new ParseOpenParentContainerCommand(connection.Client.Player, packet.ContainerId) ) );
			
			gameCommands.Add(0x89, new PacketToCommand<EditTextDialogIncomingPacket>("Edit Text Dialog", (connection, packet) => new ParseEditTextDialogCommand(connection.Client.Player, packet.WindowId, packet.Text) ) );
			
			gameCommands.Add(0x8A, new PacketToCommand<EditListDialogIncomingPacket>("Edit List Dialog", (connection, packet) => new ParseEditListDialogCommand(connection.Client.Player, packet.DoorId, packet.WindowId, packet.Text) ) );
			
			gameCommands.Add(0x8C, new PacketToCommand<LookIncomingPacket>("Look", (connection, packet) =>
			{
				Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

				if (fromPosition.IsContainer)
				{
					return new ParseLookFromContainerCommand(connection.Client.Player, fromPosition.ContainerId, fromPosition.ContainerIndex, packet.TibiaId);
				}
				else if (fromPosition.IsInventory)
				{
					return new ParseLookFromInventoryCommand(connection.Client.Player, fromPosition.InventoryIndex, packet.TibiaId);
				}
				else
				{
					return new ParseLookFromTileCommand(connection.Client.Player, fromPosition, packet.Index, packet.TibiaId);
				}
			} ) );
			
			gameCommands.Add(0x96, new PacketToCommand<TalkIncomingPacket>("Talk", (connection, packet) =>
			{
				switch (packet.TalkType)
				{
					case TalkType.Say:

						return new ParseTalkSayCommand(connection.Client.Player, packet.Message);
			
					case TalkType.Whisper:

						return new ParseTalkWhisperCommand(connection.Client.Player, packet.Message);

					case TalkType.Yell:

						return new ParseTalkYellCommand(connection.Client.Player, packet.Message);

					case TalkType.PrivatePlayerToNpc:

						return new ParseTalkPrivatePlayerToNpcCommand(connection.Client.Player, packet.Message);

					case TalkType.Private:

						return new ParseTalkPrivateCommand(connection.Client.Player, packet.Name, packet.Message);

					case TalkType.ChannelYellow:

						return new ParseTalkChannelYellowCommand(connection.Client.Player, packet.ChannelId, packet.Message);

					case TalkType.ReportRuleViolationOpen:

						return new ParseOpenReportRuleViolationCommand(connection.Client.Player, packet.Message);

					case TalkType.ReportRuleViolationAnswer:

						return new ParseAnswerReportRuleViolationCommand(connection.Client.Player, packet.Name, packet.Message);

					case TalkType.ReportRuleViolationQuestion:

						return new ParseQuestionReportRuleViolationCommand(connection.Client.Player, packet.Message);

					case TalkType.Broadcast:

						return new ParseTalkBroadcastCommand(connection.Client.Player, packet.Message);

					case TalkType.ChannelRed:

						return new ParseTalkChannelRedCommand(connection.Client.Player, packet.ChannelId, packet.Message);

					case TalkType.PrivateRed:

						return new ParseTalkPrivateRedCommand(connection.Client.Player, packet.Name, packet.Message);

					case TalkType.Unknown:

						return new ParseTalkUnknownCommand(connection.Client.Player, packet.Message);

					case TalkType.ChannelRedAnonymous:

						return new ParseTalkChannelRedAnonymousCommand(connection.Client.Player, packet.ChannelId, packet.Message);
				}
	
				throw new NotImplementedException();
			} ) );
			
			gameCommands.Add(0x97, new PacketToCommand<OpenNewChannelIncomingPacket>("Open New Channel", (connection, packet) => new ParseOpenNewChannelCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x98, new PacketToCommand<OpenedNewChannelIncomingPacket>("Opened New Channel", (connection, packet) => new ParseOpenedNewChannelCommand(connection.Client.Player, packet.ChannelId) ) );
			
			gameCommands.Add(0x99, new PacketToCommand<CloseChannelIncomingPacket>("Close Channel", (connection, packet) => new ParseCloseChannelCommand(connection.Client.Player, packet.ChannelId) ) );
			
			gameCommands.Add(0x9A, new PacketToCommand<OpenedPrivateChannelIncomingPacket>("Opened Private Channel", (connection, packet) => new ParseOpenedPrivateChannelCommand(connection.Client.Player, packet.Name) ) );
			
			gameCommands.Add(0x9B, new PacketToCommand<ProcessReportRuleViolationIncomingPacket>("Process Report Rule Violation", (connection, packet) => new ParseProcessReportRuleViolationCommand(connection.Client.Player, packet.Name) ) );
			
			gameCommands.Add(0x9C, new PacketToCommand<CloseReportRuleViolationChannelAnswerIncomingPacket>("Close Report Rule Violation Channel Answer", (connection, packet) => new ParseCloseReportRuleViolationChannelAnswerCommand(connection.Client.Player, packet.Name) ) );
			
			gameCommands.Add(0x9D, new PacketToCommand<CloseReportRuleViolationChannelQuestionIncomingPacket>("Close Report Rule Violation Channel Question", (connection, packet) => new ParseCloseReportRuleViolationChannelQuestionCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x9E, new PacketToCommand<CloseNpcsChannelIncomingPacket>("Close Npcs Channel", (connection, packet) => new ParseCloseNpcsChannelCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0xA0, new PacketToCommand<CombatControlsIncomingPacket>("Combat Controls", (connection, packet) => new ParseCombatControlsCommand(connection.Client.Player, packet.FightMode, packet.ChaseMode, packet.SafeMode) ) );
			
			gameCommands.Add(0xA1, new PacketToCommand<AttackIncomingPacket>("Attack", (connection, packet) =>
			{
				if (packet.CreatureId == 0)
				{
					return new ParseStopAttackCommand(connection.Client.Player);
				}
				else
				{
					return new ParseStartAttackCommand(connection.Client.Player, packet.CreatureId, packet.Nonce);
				}
			} ) );
			
			gameCommands.Add(0xA2, new PacketToCommand<FollowIncomingPacket>("Follow", (connection, packet) =>
			{
				if (packet.CreatureId == 0)
				{
					return new ParseStopFollowCommand(connection.Client.Player);
				}
				else
				{
					return new ParseStartFollowCommand(connection.Client.Player, packet.CreatureId, packet.Nonce);
				}
			} ) );
			
			gameCommands.Add(0xA3, new PacketToCommand<InviteToPartyIncomingPacket>("Invite To Party", (connection, packet) => new ParseInviteToPartyCommand(connection.Client.Player, packet.CreatureId) ) );
			
			gameCommands.Add(0xA4, new PacketToCommand<JoinPartyIncomingPacket>("Join Party", (connection, packet) => new ParseJoinPartyCommand(connection.Client.Player, packet.CreatureId) ) );
			
			gameCommands.Add(0xA5, new PacketToCommand<RevokePartyIncomingPacket>("Revoke Party", (connection, packet) => new ParseRevokePartyCommand(connection.Client.Player, packet.CreatureId) ) );
			
			gameCommands.Add(0xA6, new PacketToCommand<PassLeadershipToIncomingPacket>("Pass Leadership To", (connection, packet) => new ParsePassLeadershipToCommand(connection.Client.Player, packet.CreatureId) ) );
			
			gameCommands.Add(0xA7, new PacketToCommand<LeavePartyIncomingPacket>("Leave Party", (connection, packet) => new ParseLeavePartyCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0xA8, new PacketToCommand<SharedExperienceIncomingPacket>("Shared Experience", (connection, packet) => new ParseSharedExperienceCommand(connection.Client.Player, packet.Enabled) ) );
			
			gameCommands.Add(0xAA, new PacketToCommand<OpenedMyPrivateChannelIncomingPacket>("Opened My Private Channel", (connection, packet) => new ParseOpenedMyPrivateChannelCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0xAB, new PacketToCommand<InvitePlayerIncomingPacket>("Invite Player", (connection, packet) => new ParseInvitePlayerChannelCommand(connection.Client.Player, packet.Name) ) );
			
			gameCommands.Add(0xAC, new PacketToCommand<ExcludePlayerIncomingPacket>("Exclude Player", (connection, packet) => new ParseExcludePlayerCommand(connection.Client.Player, packet.Name) ) );
			
			gameCommands.Add(0xBE, new PacketToCommand<StopIncomingPacket>("Stop", (connection, packet) => new ParseStopCommand(connection.Client.Player) ) );
            
			// 0xC9 - Update Tile
            
			// 0xCA - Update Container
			
			gameCommands.Add(0xD2, new PacketToCommand<SetOutfitIncomingPacket>("Set Outfit", (connection, packet) => new ParseSetOutfitCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0xD3, new PacketToCommand<SelectedOutfitIncomingPacket>("Selected Outfit", (connection, packet) => new ParseSelectedOutfitCommand(connection.Client.Player, packet.Outfit) ) );
			
			gameCommands.Add(0xDC, new PacketToCommand<AddVipIncomingPacket>("Add Vip", (connection, packet) => new ParseAddVipCommand(connection.Client.Player, packet.Name) ) );
			
			gameCommands.Add(0xDD, new PacketToCommand<RemoveVipIncomingPacket>("Remove Vip", (connection, packet) => new ParseRemoveVipCommand(connection.Client.Player, packet.CreatureId) ) );
			
			gameCommands.Add(0xE6, new PacketToCommand<ReportBugIncomingPacket>("Report Bug", (connection, packet) => new ParseReportBugCommand(connection.Client.Player, packet.Message) ) );
			
			gameCommands.Add(0xE8, new PacketToCommand<DebugAssertIncomingPacket>("Debug Assert", (connection, packet) => new ParseDebugAssertCommand(connection.Client.Player, packet.AssertLine, packet.ReportDate, packet.Description, packet.Comment) ) );
			
			gameCommands.Add(0xF0, new PacketToCommand<QuestsIncomingPacket>("Quests", (connection, packet) => new ParseQuestsCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0xF1, new PacketToCommand<OpenQuestIncomingPacket>("Open Quest", (connection, packet) => new ParseOpenQuestCommand(connection.Client.Player, packet.QuestId) ) );
			
			gameCommands.Add(0xF2, new PacketToCommand<ReportRuleViolationIncomingPacket>("Report Rule Violation", (connection, packet) => new ParseReportRuleViolationCommand(connection.Client.Player, packet.Type, packet.RuleViolation, packet.Name, packet.Comment, packet.Translation, packet.StatmentId) ) );
								
			gameAccountManagerCommands.Add(0x14, new PacketToCommand<LogOutIncomingPacket>("Log Out", (connection, packet) => new ParseLogOutCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x1E, new PacketToCommand<PongIncomingPacket>("Pong", (connection, packet) => new ParsePongCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x64, new PacketToCommand<WalkToIncomingPacket>("Walk To", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x65, new PacketToCommand<WalkNorthIncomingPacket>("Walk North", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x66, new PacketToCommand<WalkEastIncomingPacket>("Walk East", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x67, new PacketToCommand<WalkSouthIncomingPacket>("Walk South", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x68, new PacketToCommand<WalkWestIncomingPacket>("Walk West", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x69, new PacketToCommand<StopWalkIncomingPacket>("Stop Walk", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x6A, new PacketToCommand<WalkNorthEastIncomingPacket>("Walk North East", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x6B, new PacketToCommand<WalkSouthEastIncomingPacket>("Walk South East", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x6C, new PacketToCommand<WalkSouthWestIncomingPacket>("Walk South West", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x6D, new PacketToCommand<WalkNorthWestIncomingPacket>("Walk North West", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x6F, new PacketToCommand<TurnNorthIncomingPacket>("Turn North", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x70, new PacketToCommand<TurnEastIncomingPacket>("Turn East", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x71, new PacketToCommand<TurnSouthIncomingPacketh>("Turn South", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x72, new PacketToCommand<TurnWestIncomingPacket>("Turn West", (connection, packet) => new DisallowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x78, new PacketToCommand<MoveItemIncomingPacket>("Move Item", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x79, new PacketToCommand<LookItemNpcTradeIncomingPacket>("Look Item Npc Trade", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x7A, new PacketToCommand<BuyNpcTradeIncomingPacket>("Buy Npc Trade", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x7B, new PacketToCommand<SellNpcTradeIncomingPacket>("Sell Npc Trade", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x7C, new PacketToCommand<CloseNpcTradeIncomingPacket>("Close Npc Trade", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x7D, new PacketToCommand<TradeWithIncomingPacket>("Trade With", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x7E, new PacketToCommand<LookItemTradeIncomingPacket>("Look Item Trade", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x7F, new PacketToCommand<AcceptTradeIncomingPacket>("Accept Trade", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x80, new PacketToCommand<CancelOrRejectTradeIncomingPacket>("Cancel Or Reject Trade", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x82, new PacketToCommand<UseItemIncomingPacket>("Use Item", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x83, new PacketToCommand<UseItemWithItemIncomingPacket>("Use Item With Item", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x84, new PacketToCommand<UseItemWithCreatureIncomingPacket>("Use Item With Creature", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x85, new PacketToCommand<RotateItemIncomingPacket>("Rotate Item", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x87, new PacketToCommand<CloseContainerIncomingPacket>("Close Container", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x88, new PacketToCommand<OpenParentContainerIncomingPacket>("Open Parent Container", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x89, new PacketToCommand<EditTextDialogIncomingPacket>("Edit Text Dialog", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x8A, new PacketToCommand<EditListDialogIncomingPacket>("Edit List Dialog", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x8C, new PacketToCommand<LookIncomingPacket>("Look", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x96, new PacketToCommand<TalkIncomingPacket>("Talk", (connection, packet) => new ParseTalkSayCommand(connection.Client.Player, packet.Message) ) );
            
			gameAccountManagerCommands.Add(0x97, new PacketToCommand<OpenNewChannelIncomingPacket>("Open New Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x98, new PacketToCommand<OpenedNewChannelIncomingPacket>("Opened New Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x99, new PacketToCommand<CloseChannelIncomingPacket>("Close Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x9A, new PacketToCommand<OpenedPrivateChannelIncomingPacket>("Opened Private Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x9B, new PacketToCommand<ProcessReportRuleViolationIncomingPacket>("Process Report Rule Violation", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x9C, new PacketToCommand<CloseReportRuleViolationChannelAnswerIncomingPacket>("Close Report Rule Violation Channel Answer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x9D, new PacketToCommand<CloseReportRuleViolationChannelQuestionIncomingPacket>("Close Report Rule Violation Channel Question", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x9E, new PacketToCommand<CloseNpcsChannelIncomingPacket>("Close Npcs Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA0, new PacketToCommand<CombatControlsIncomingPacket>("Combat Controls", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA1, new PacketToCommand<AttackIncomingPacket>("Attack", (connection, packet) => new ParseStopAttackCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA2, new PacketToCommand<FollowIncomingPacket>("Follow", (connection, packet) => new ParseStopFollowCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA3, new PacketToCommand<InviteToPartyIncomingPacket>("Invite To Party", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA4, new PacketToCommand<JoinPartyIncomingPacket>("Join Party", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA5, new PacketToCommand<RevokePartyIncomingPacket>("Revoke Party", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA6, new PacketToCommand<PassLeadershipToIncomingPacket>("Pass Leadership To", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA7, new PacketToCommand<LeavePartyIncomingPacket>("Leave Party", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xA8, new PacketToCommand<SharedExperienceIncomingPacket>("Shared Experience", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xAA, new PacketToCommand<OpenedMyPrivateChannelIncomingPacket>("Opened My Private Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xAB, new PacketToCommand<InvitePlayerIncomingPacket>("Invite Player", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xAC, new PacketToCommand<ExcludePlayerIncomingPacket>("Exclude Player", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xBE, new PacketToCommand<StopIncomingPacket>("Stop", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			// 0xC9 - Update Tile
            
			// 0xCA - Update Container
            
			gameAccountManagerCommands.Add(0xD2, new PacketToCommand<SetOutfitIncomingPacket>("Set Outfit", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xD3, new PacketToCommand<SelectedOutfitIncomingPacket>("Selected Outfit", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xDC, new PacketToCommand<AddVipIncomingPacket>("Add Vip", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xDD, new PacketToCommand<RemoveVipIncomingPacket>("Remove Vip", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xE6, new PacketToCommand<ReportBugIncomingPacket>("Report Bug", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			
			gameAccountManagerCommands.Add(0xE8, new PacketToCommand<DebugAssertIncomingPacket>("Debug Assert", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			
			gameAccountManagerCommands.Add(0xF0, new PacketToCommand<QuestsIncomingPacket>("Quests", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xF1, new PacketToCommand<OpenQuestIncomingPacket>("Open Quest", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			
			gameAccountManagerCommands.Add(0xF2, new PacketToCommand<ReportRuleViolationIncomingPacket>("Report Rule Violation", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
         
			infoFirstCommands.Add(0xFF, new PacketToCommand<InfoIncomingPacket>("Info", (connection, packet) => new ParseInfoProtocolCommand(connection, packet) ) );
        }

        public int clientVersion;

		public int ClientVersion
        {
            get
            {
                return clientVersion;
            }
        }

        public int tibiaDat;

        public int TibiaDat
        {
            get
            {
                return tibiaDat;
            }
        }

        public int tibiaPic;

        public int TibiaPic
        {
            get
            {
                return tibiaPic;				
			}
        }

        public int tibiaSpr;

		public int TibiaSpr
		{
			get
			{
				return tibiaSpr;
            }
		}

        private HashSet<FeatureFlag> featureFlags = new HashSet<FeatureFlag>();

        public bool HasFeatureFlag(FeatureFlag featureFlag)
        {
            return featureFlags.Contains(featureFlag);
        }

        private Dictionary<byte, IPacketToCommand> loginFirstCommands = new Dictionary<byte, IPacketToCommand>();

        public Dictionary<byte, IPacketToCommand> LoginFirstCommands
        {
            get
            {
                return loginFirstCommands;
            }
        }

        private Dictionary<byte, IPacketToCommand> gameFirstCommands = new Dictionary<byte, IPacketToCommand>();

        public Dictionary<byte, IPacketToCommand> GameFirstCommands
        {
            get
            {
                return gameFirstCommands;
            }
        }

        private Dictionary<byte, IPacketToCommand> gameCommands = new Dictionary<byte, IPacketToCommand>();

        public Dictionary<byte, IPacketToCommand> GameCommands
        {
            get
            {
                return gameCommands;
            }
        }

        private Dictionary<byte, IPacketToCommand> gameAccountManagerCommands = new Dictionary<byte, IPacketToCommand>();

        public Dictionary<byte, IPacketToCommand> GameAccountManagerCommands
        {
            get
            {
                return gameAccountManagerCommands;
            }
        }

        private Dictionary<byte, IPacketToCommand> infoFirstCommands = new Dictionary<byte, IPacketToCommand>();

        public Dictionary<byte, IPacketToCommand> InfoFirstCommands
        {
            get
            {
                return infoFirstCommands;
            }
        }
    }
}
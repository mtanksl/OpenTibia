using OpenTibia.Common.Structures;
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
			#region Clients

			if (server.Config.ClientVersion == new Version(7, 72) )
			{
                clientVersion = 772;
                tibiaDat = 1134385715;
                tibiaPic = 1146144984;
                tibiaSpr = 1134056126;

				Outfit.Swimming = Outfit.Invisible;
            }
            else if (server.Config.ClientVersion == new Version(8, 60) )
            {
                clientVersion = 860;
				tibiaDat = 1277983123;
				tibiaPic = 1256571859;
				tibiaSpr = 1277298068;				
            }
			else if (server.Config.ClientVersion == new Version(8, 70) )
            {
                clientVersion = 870;
				tibiaDat = 1291723461;
				tibiaPic = 1291111508;
				tibiaSpr = 1291650954;				
            }
            else if (server.Config.ClientVersion == new Version(9, 86) )
            {
                clientVersion = 986;
                tibiaDat = 1366354180;
                tibiaPic = 1360233828;
                tibiaSpr = 1366354287;
			}
			else if (server.Config.ClientVersion == new Version(10, 98) )
            {
                clientVersion = 1098;
                tibiaDat = 0;
                tibiaPic = 1467784158;
                tibiaSpr = 1471927811;
			}
			else
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Feature Flags

            featureFlags.Add(FeatureFlag.RuleViolationChannel);

            if (clientVersion >= 770)
			{
				featureFlags.Add(FeatureFlag.LookTypeUInt16);
				featureFlags.Add(FeatureFlag.MessageStatement);
				featureFlags.Add(FeatureFlag.LoginPacketEncryption);
			}

			if (clientVersion >= 780) 
			{
				featureFlags.Add(FeatureFlag.PlayerAddons);
				featureFlags.Add(FeatureFlag.PlayerStamina);
				featureFlags.Add(FeatureFlag.MessageLevel);
				featureFlags.Add(FeatureFlag.PlayerSpecialConditionUInt16);
				featureFlags.Add(FeatureFlag.NewOutfitProtocol);
			}

			if (clientVersion >= 790)
			{
				featureFlags.Add(FeatureFlag.ReadableItemDate);
				featureFlags.Add(FeatureFlag.QuestLog);
            }

            if (clientVersion >= 810) 
			{
				featureFlags.Add(FeatureFlag.SpeedFactor);
            }

			if (clientVersion >= 820)
			{
                featureFlags.Add(FeatureFlag.NpcsChannel);
			}

            if (clientVersion >= 840) 
			{
				featureFlags.Add(FeatureFlag.ProtocolChecksum);
				featureFlags.Add(FeatureFlag.AccountString);
				featureFlags.Add(FeatureFlag.PlayerCapacityUInt32);
				featureFlags.Add(FeatureFlag.PartyChannel);
            }

			if (clientVersion >= 841) 
			{
				featureFlags.Add(FeatureFlag.ChallengeOnLogin);
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

			if (clientVersion >= 861)
			{
                featureFlags.Remove(FeatureFlag.RuleViolationChannel);
            }

			if (clientVersion >= 862)
			{
                featureFlags.Add(FeatureFlag.PenalityOnDeath);				
            }

            if (clientVersion >= 870)
			{
                featureFlags.Add(FeatureFlag.PlayerExperienceUInt64);
                featureFlags.Add(FeatureFlag.PlayerMounts);
                featureFlags.Add(FeatureFlag.CooldownBar);
            }

			if (clientVersion >= 873)
			{
                featureFlags.Add(FeatureFlag.ConsoleMessageOtherCreatures);
            }

            if (clientVersion >= 900) 
			{
				featureFlags.Add(FeatureFlag.ServerBeat);
				featureFlags.Add(FeatureFlag.InviteNpcTradeU16);
            }

            if (clientVersion >= 910) 
			{
				featureFlags.Add(FeatureFlag.NameOnNpcTrade);
				featureFlags.Add(FeatureFlag.PlayerTotalCapacity);
				featureFlags.Add(FeatureFlag.PlayerSkillsBase);
				featureFlags.Add(FeatureFlag.PlayerRegenerationTime);
				featureFlags.Add(FeatureFlag.ChannelPlayerList);
				featureFlags.Add(FeatureFlag.EnvironmentEffect);
                featureFlags.Add(FeatureFlag.ItemAnimationPhase);
				featureFlags.Add(FeatureFlag.CreatureType);
            }

            if (clientVersion >= 940) 
			{
				featureFlags.Add(FeatureFlag.PlayerMarket);
			}

			if (clientVersion >= 950)
			{
                featureFlags.Add(FeatureFlag.PlayerBasicData);
            }

            if (clientVersion >= 953)
            {
				featureFlags.Add(FeatureFlag.PurseSlot);
                featureFlags.Add(FeatureFlag.ClientPing);
				featureFlags.Add(FeatureFlag.CreatureUnpass);
            }

			if (clientVersion >= 960) 
			{
                featureFlags.Add(FeatureFlag.SpritesUInt32);
                featureFlags.Add(FeatureFlag.OfflineTrainingTime);
            }

			if (clientVersion >= 963) 
			{
				featureFlags.Add(FeatureFlag.AdditionalVipInfo);
            }

			if (clientVersion >= 973) 
			{
				featureFlags.Add(FeatureFlag.JoinNpcTradeU64);
            }

            if (clientVersion >= 980) 
			{
				featureFlags.Add(FeatureFlag.PreviewState);
				featureFlags.Add(FeatureFlag.ClientVersion);
			}

			if (clientVersion >= 981) 
			{
				featureFlags.Add(FeatureFlag.LoginPending);
				featureFlags.Add(FeatureFlag.NewSpeedLaw);
            }

			if (clientVersion >= 984) 
			{
				featureFlags.Add(FeatureFlag.ContainerPagination);
                featureFlags.Add(FeatureFlag.BrowseField);
            }

			if (clientVersion >= 1000) 
			{
				featureFlags.Add(FeatureFlag.ThingMarks);
				featureFlags.Add(FeatureFlag.PVPMode);
			}

			if (clientVersion >= 1010)
            {
                featureFlags.Add(FeatureFlag.NoMovementAnimation);
                featureFlags.Add(FeatureFlag.GroupWorlds);
            }

			if (clientVersion >= 1035) 
			{
				featureFlags.Add(FeatureFlag.SkillLevelU16);
			}

			if (clientVersion >= 1036) 
			{
				featureFlags.Add(FeatureFlag.CreatureIcons);
			}

			if (clientVersion >= 1038) 
			{
				featureFlags.Add(FeatureFlag.PremiumExpiration);
			}

			if (clientVersion >= 1050) 
			{
				featureFlags.Add(FeatureFlag.EnhancedAnimations);
			}

			if (clientVersion >= 1053) 
			{
				featureFlags.Add(FeatureFlag.UnjustifiedPoints);
			}

			if (clientVersion >= 1054)
			{
				featureFlags.Add(FeatureFlag.ExperienceBonus);
				featureFlags.Add(FeatureFlag.PVPFrame);
            }

			if (clientVersion >= 1055) 
			{
				featureFlags.Add(FeatureFlag.DeathType);
			}

			if (clientVersion >= 1057) 
			{
				featureFlags.Add(FeatureFlag.IdleAnimations);
			}

			if (clientVersion >= 1058)
			{
				featureFlags.Add(FeatureFlag.ExpertMode);
			}

			if (clientVersion >= 1061) 
			{
				featureFlags.Add(FeatureFlag.OGLInformation);
			}

			if (clientVersion >= 1071) 
			{
				featureFlags.Add(FeatureFlag.ContentRevision);
			}

			if (clientVersion >= 1072) 
			{
				featureFlags.Add(FeatureFlag.Authenticator);
			}

			if (clientVersion >= 1074) 
			{
				featureFlags.Add(FeatureFlag.SessionKey);
			}

			if (clientVersion >= 1076)
			{
				featureFlags.Add(FeatureFlag.LoginServerErrorNew);
			}

			if (clientVersion >= 1077)
			{
				featureFlags.Add(FeatureFlag.AccountStatus);                
            }

			if (clientVersion >= 1080) 
			{
				featureFlags.Add(FeatureFlag.IngameStore);
			}

			if (clientVersion >= 1092) 
			{
				featureFlags.Add(FeatureFlag.IngameStoreServiceType);
			}

			if (clientVersion >= 1093) 
			{
				featureFlags.Add(FeatureFlag.IngameStoreHighlights);
			}

			if (clientVersion >= 1094) 
			{
				featureFlags.Add(FeatureFlag.AdditionalSkills);
			}

			if (clientVersion >= 1097) 
			{
				featureFlags.Add(FeatureFlag.MultipleExperienceBonus);
			}

            #endregion

            #region Fluid Type

            byte empty = 0;
			byte blue = 0;
			byte red = 0;
			byte brown = 0;
			byte green = 0;
			byte yellow = 0;
			byte white = 0;
			byte pink = 0;

            if (clientVersion >= 780) 
			{
                empty = 0;
                blue = 1;
                red = 5;
                brown = 7;
                green = 6;
                yellow = 8;
                white = 9;
                pink = 2;
			} 
			else
			{
                empty = 0;
                blue = 1;
                red = 2;
                brown = 3;
                green = 4;
                yellow = 5;
                white = 6;
                pink = 7;
            }

            MapFluidType(FluidType.Empty, empty);
            MapFluidType(FluidType.Water, blue);
            MapFluidType(FluidType.Blood, red);
            MapFluidType(FluidType.Beer, brown);
            MapFluidType(FluidType.Slime, green);
            MapFluidType(FluidType.Lemonade, yellow);
            MapFluidType(FluidType.Milk, white);
            MapFluidType(FluidType.Manafluid, pink);
            MapFluidType(FluidType.Lifefluid, red);
            MapFluidType(FluidType.Oil, brown);
            MapFluidType(FluidType.Urine, yellow);
            MapFluidType(FluidType.CoconutMilk, white);
            MapFluidType(FluidType.Wine, pink);
            MapFluidType(FluidType.Mud, brown);
            MapFluidType(FluidType.FruitJuice, yellow);
            MapFluidType(FluidType.Lava, red);
            MapFluidType(FluidType.Rum, brown);
            MapFluidType(FluidType.Swamp, green);

            #endregion

            #region Text Color

			if (clientVersion >= 1055) 
			{
				MapMessageMode(0, MessageMode.None);
				MapMessageMode(1, MessageMode.Say);
                MapMessageMode(2, MessageMode.Whisper);
                MapMessageMode(3, MessageMode.Yell);
                MapMessageMode(4, MessageMode.PrivateFrom);
                MapMessageMode(5, MessageMode.PrivateTo);
                MapMessageMode(6, MessageMode.ChannelManagement);
                MapMessageMode(7, MessageMode.Channel);
                MapMessageMode(8, MessageMode.ChannelHighlight);
                MapMessageMode(9, MessageMode.Spell);
                MapMessageMode(10, MessageMode.NpcFromStartBlock);
                MapMessageMode(11, MessageMode.NpcFrom);
                MapMessageMode(12, MessageMode.NpcTo);
                MapMessageMode(13, MessageMode.GamemasterBroadcast);
                MapMessageMode(14, MessageMode.GamemasterChannel);
                MapMessageMode(15, MessageMode.GamemasterPrivateFrom);
                MapMessageMode(16, MessageMode.GamemasterPrivateTo);
                MapMessageMode(17, MessageMode.Login);
                MapMessageMode(18, MessageMode.Warning);
                MapMessageMode(19, MessageMode.Game);
                MapMessageMode(20, MessageMode.GameHighlight);
                MapMessageMode(21, MessageMode.Failure);
                MapMessageMode(22, MessageMode.Look);
                MapMessageMode(23, MessageMode.DamageDealed);
                MapMessageMode(24, MessageMode.DamageReceived);
                MapMessageMode(25, MessageMode.Heal);
                MapMessageMode(26, MessageMode.Exp);
                MapMessageMode(27, MessageMode.DamageOthers);
                MapMessageMode(28, MessageMode.HealOthers);
                MapMessageMode(29, MessageMode.ExpOthers);
                MapMessageMode(30, MessageMode.Status);
                MapMessageMode(31, MessageMode.Loot);
                MapMessageMode(32, MessageMode.TradeNpc);
                MapMessageMode(33, MessageMode.Guild);
                MapMessageMode(34, MessageMode.PartyManagement);
                MapMessageMode(35, MessageMode.Party);
                MapMessageMode(36, MessageMode.BarkLow);
                MapMessageMode(37, MessageMode.BarkLoud);
                MapMessageMode(38, MessageMode.Report);
                MapMessageMode(39, MessageMode.HotkeyUse);
                MapMessageMode(40, MessageMode.TutorialHint);
                MapMessageMode(41, MessageMode.Thankyou);
                MapMessageMode(42, MessageMode.Market);
                MapMessageMode(43, MessageMode.Mana);
            }
			else if (clientVersion >= 900)
			{
				for (int i = 0; i < 42; i++)
				{
					MapMessageMode( (byte)i, (MessageMode)i);
				}
			}
			else if (clientVersion >= 861)
			{
				MapMessageMode(0, MessageMode.None);
				MapMessageMode(1, MessageMode.Say);
				MapMessageMode(2, MessageMode.Whisper);
				MapMessageMode(3, MessageMode.Yell);
				MapMessageMode(4, MessageMode.NpcTo);
				MapMessageMode(5, MessageMode.NpcFrom);
				MapMessageMode(6, MessageMode.PrivateTo);
				MapMessageMode(6, MessageMode.PrivateFrom);
				MapMessageMode(7, MessageMode.Channel);
				MapMessageMode(8, MessageMode.ChannelManagement);
				MapMessageMode(9, MessageMode.GamemasterBroadcast);
				MapMessageMode(10, MessageMode.GamemasterChannel);
				MapMessageMode(11, MessageMode.GamemasterPrivateTo);
				MapMessageMode(11, MessageMode.GamemasterPrivateFrom);
				MapMessageMode(12, MessageMode.ChannelHighlight);
				MapMessageMode(13, MessageMode.MonsterSay);
				MapMessageMode(14, MessageMode.MonsterYell);
				MapMessageMode(15, MessageMode.Warning);
				MapMessageMode(16, MessageMode.Game);
				MapMessageMode(17, MessageMode.Login);
				MapMessageMode(18, MessageMode.Status);
				MapMessageMode(19, MessageMode.Look);
				MapMessageMode(20, MessageMode.Failure);
				MapMessageMode(21, MessageMode.Blue);
				MapMessageMode(22, MessageMode.Red);
			}
			else if (clientVersion >= 840)
			{
				MapMessageMode(0, MessageMode.None);
				MapMessageMode(1, MessageMode.Say);
				MapMessageMode(2, MessageMode.Whisper);
				MapMessageMode(3, MessageMode.Yell);
				MapMessageMode(4, MessageMode.NpcTo);
				MapMessageMode(5, MessageMode.NpcFrom);
				MapMessageMode(6, MessageMode.PrivateTo);
				MapMessageMode(6, MessageMode.PrivateFrom);
				MapMessageMode(7, MessageMode.Channel);
				MapMessageMode(8, MessageMode.ChannelManagement);
				MapMessageMode(9, MessageMode.RVRChannel);
				MapMessageMode(10, MessageMode.RVRAnswer);
				MapMessageMode(11, MessageMode.RVRContinue);
				MapMessageMode(12, MessageMode.GamemasterBroadcast);
				MapMessageMode(13, MessageMode.GamemasterChannel);
				MapMessageMode(14, MessageMode.GamemasterPrivateTo);
				MapMessageMode(14, MessageMode.GamemasterPrivateFrom);
				MapMessageMode(15, MessageMode.ChannelHighlight);
				MapMessageMode(16, MessageMode.Unknown);
				MapMessageMode(17, MessageMode.GamemasterChannelAnonymous);
				MapMessageMode(18, MessageMode.Red);
				MapMessageMode(19, MessageMode.MonsterSay);
				MapMessageMode(20, MessageMode.MonsterYell);
				MapMessageMode(21, MessageMode.Warning);
				MapMessageMode(22, MessageMode.Game);
				MapMessageMode(23, MessageMode.Login);
				MapMessageMode(24, MessageMode.Status);
				MapMessageMode(25, MessageMode.Look);
				MapMessageMode(26, MessageMode.Failure);
				MapMessageMode(27, MessageMode.Blue);
			}
			else if (clientVersion >= 760)
			{
				MapMessageMode(0, MessageMode.None);
				MapMessageMode(1, MessageMode.Say);
				MapMessageMode(2, MessageMode.Whisper);
				MapMessageMode(3, MessageMode.Yell);
				MapMessageMode(4, MessageMode.PrivateTo);
				MapMessageMode(4, MessageMode.PrivateFrom);
				MapMessageMode(5, MessageMode.Channel);
				MapMessageMode(6, MessageMode.RVRChannel);
				MapMessageMode(7, MessageMode.RVRAnswer);
				MapMessageMode(8, MessageMode.RVRContinue);
				MapMessageMode(9, MessageMode.GamemasterBroadcast);
				MapMessageMode(10, MessageMode.GamemasterChannel);
				MapMessageMode(11, MessageMode.GamemasterPrivateTo);
				MapMessageMode(11, MessageMode.GamemasterPrivateFrom);
				MapMessageMode(12, MessageMode.ChannelHighlight);
				MapMessageMode(13, MessageMode.Unknown);
				MapMessageMode(14, MessageMode.GamemasterChannelAnonymous);
				// 15
				MapMessageMode(16, MessageMode.MonsterSay);
				MapMessageMode(17, MessageMode.MonsterYell);
				MapMessageMode(18, MessageMode.Warning);
				MapMessageMode(19, MessageMode.Game);
				MapMessageMode(20, MessageMode.Login);
				MapMessageMode(21, MessageMode.Status);
				MapMessageMode(22, MessageMode.Look);
				MapMessageMode(23, MessageMode.Failure);
				MapMessageMode(24, MessageMode.Blue);
				MapMessageMode(25, MessageMode.Red);
			}
			else
			{
				throw new NotImplementedException();
			}

            #endregion

            #region Packets

            loginFirstCommands.Add(0x01, new PacketToCommand<EnterGameIncomingPacket>("Enter Game",(connection, packet) => new ParseEnterGameCommand(connection, packet) ) );

            gameFirstCommands.Add(0x0A, new PacketToCommand<SelectedCharacterIncomingPacket>("Selected Character", (connection, packet) => new ParseSelectedCharacterCommand(connection, packet) ) );

			if (HasFeatureFlag(FeatureFlag.LoginPending) )
			{
                gameCommands.Add(0x0F, new PacketToCommand<EnterGame2IncomingPacket>("Enter Game", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}

			gameCommands.Add(0x14, new PacketToCommand<LogOutIncomingPacket>("Log Out", (connection, packet) => new ParseLogOutCommand(connection.Client.Player) ) );
			
			if (HasFeatureFlag(FeatureFlag.ClientPing) )
			{
				gameCommands.Add(0x1D, new PacketToCommand<PingIncomingPacket>("Client ping request", (connection, packet) => new ParsePingCommand(connection.Client.Player) ) );
            }
				
			gameCommands.Add(0x1E, new PacketToCommand<PongIncomingPacket>("Server ping response", (connection, packet) => new ParsePongCommand(connection.Client.Player) ) );

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

			gameCommands.Add(0x8D, new PacketToCommand<LookInBattleListIncomingPacket>("Look Creature Battle List", (connection, packet) => new ParseLookInBattleListCommand(connection.Client.Player, packet.CreatureId) ) );

            // 0x8E - JoinAggresionIncomingPacket

            gameCommands.Add(0x96, new PacketToCommand<TalkIncomingPacket>("Talk", (connection, packet) =>
			{
                switch (packet.MessageMode)
				{
					case MessageMode.Say:

						return new ParseTalkSayCommand(connection.Client.Player, packet.Message);
			
					case MessageMode.Whisper:

						return new ParseTalkWhisperCommand(connection.Client.Player, packet.Message);

					case MessageMode.Yell:

						return new ParseTalkYellCommand(connection.Client.Player, packet.Message);

					case MessageMode.NpcTo:

						return new ParseTalkPrivatePlayerToNpcCommand(connection.Client.Player, packet.Message);

					case MessageMode.PrivateTo:

                        return new ParseTalkPrivateCommand(connection.Client.Player, packet.Name, packet.Message);

					case MessageMode.Channel:

						return new ParseTalkChannelYellowCommand(connection.Client.Player, packet.ChannelId, packet.Message);

					case MessageMode.RVRChannel:

						return new ParseOpenReportRuleViolationCommand(connection.Client.Player, packet.Message);

					case MessageMode.RVRAnswer:

						return new ParseAnswerReportRuleViolationCommand(connection.Client.Player, packet.Name, packet.Message);

					case MessageMode.RVRContinue:

						return new ParseQuestionReportRuleViolationCommand(connection.Client.Player, packet.Message);

					case MessageMode.GamemasterBroadcast:

						return new ParseTalkBroadcastCommand(connection.Client.Player, packet.Message);

					case MessageMode.GamemasterChannel:

						return new ParseTalkChannelRedCommand(connection.Client.Player, packet.ChannelId, packet.Message);

					case MessageMode.GamemasterPrivateTo:

                        return new ParseTalkPrivateRedCommand(connection.Client.Player, packet.Name, packet.Message);

                    case MessageMode.Unknown:
                    
                    	return new ParseTalkUnknownCommand(connection.Client.Player, packet.Message);
                    
                    case MessageMode.GamemasterChannelAnonymous:
                    
                    	return new ParseTalkChannelRedAnonymousCommand(connection.Client.Player, packet.ChannelId, packet.Message);
                }

                throw new NotImplementedException();
			} ) );

			gameCommands.Add(0x97, new PacketToCommand<OpenNewChannelIncomingPacket>("Open New Channel", (connection, packet) => new ParseOpenNewChannelCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0x98, new PacketToCommand<OpenedNewChannelIncomingPacket>("Opened New Channel", (connection, packet) => new ParseOpenedNewChannelCommand(connection.Client.Player, packet.ChannelId) ) );
			
			gameCommands.Add(0x99, new PacketToCommand<CloseChannelIncomingPacket>("Close Channel", (connection, packet) => new ParseCloseChannelCommand(connection.Client.Player, packet.ChannelId) ) );
			
			gameCommands.Add(0x9A, new PacketToCommand<OpenedPrivateChannelIncomingPacket>("Opened Private Channel", (connection, packet) => new ParseOpenedPrivateChannelCommand(connection.Client.Player, packet.Name) ) );

            if (HasFeatureFlag(FeatureFlag.RuleViolationChannel) )
            {
				gameCommands.Add(0x9B, new PacketToCommand<ProcessReportRuleViolationIncomingPacket>("Process Report Rule Violation", (connection, packet) => new ParseProcessReportRuleViolationCommand(connection.Client.Player, packet.Name) ) );

				gameCommands.Add(0x9C, new PacketToCommand<CloseReportRuleViolationChannelAnswerIncomingPacket>("Close Report Rule Violation Channel Answer", (connection, packet) => new ParseCloseReportRuleViolationChannelAnswerCommand(connection.Client.Player, packet.Name) ) );
				
				gameCommands.Add(0x9D, new PacketToCommand<CloseReportRuleViolationChannelQuestionIncomingPacket>("Close Report Rule Violation Channel Question", (connection, packet) => new ParseCloseReportRuleViolationChannelQuestionCommand(connection.Client.Player) ) );
            }

            if (HasFeatureFlag(FeatureFlag.NpcsChannel) )
            {
				gameCommands.Add(0x9E, new PacketToCommand<CloseNpcsChannelIncomingPacket>("Close Npcs Channel", (connection, packet) => new ParseCloseNpcsChannelCommand(connection.Client.Player) ) );
			}
			
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

			if (HasFeatureFlag(FeatureFlag.BrowseField) )
			{
                gameCommands.Add(0xCB, new PacketToCommand<BrowseFieldIncomingPacket>("Browse Field", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            }

            if (HasFeatureFlag(FeatureFlag.ContainerPagination) )
			{
                gameCommands.Add(0xCC, new PacketToCommand<SeekInContainerIncomingPacket>("Seek In Container", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            }

            gameCommands.Add(0xD2, new PacketToCommand<SetOutfitIncomingPacket>("Set Outfit", (connection, packet) => new ParseSetOutfitCommand(connection.Client.Player) ) );
			
			gameCommands.Add(0xD3, new PacketToCommand<SelectedOutfitIncomingPacket>("Selected Outfit", (connection, packet) => new ParseSelectedOutfitCommand(connection.Client.Player, packet.Outfit) ) );

            if (HasFeatureFlag(FeatureFlag.PlayerMounts) )
            {
				gameCommands.Add(0xD4, new PacketToCommand<MountIncomingPacket>("Mount", (connection, packet) => new ParseMountCommand(connection.Client.Player, packet.IsMounted) ) );
			}

			gameCommands.Add(0xDC, new PacketToCommand<AddVipIncomingPacket>("Add Vip", (connection, packet) => new ParseAddVipCommand(connection.Client.Player, packet) ) );
			
			gameCommands.Add(0xDD, new PacketToCommand<RemoveVipIncomingPacket>("Remove Vip", (connection, packet) => new ParseRemoveVipCommand(connection.Client.Player, packet) ) );
			
			if (HasFeatureFlag(FeatureFlag.AdditionalVipInfo) )
			{
                gameCommands.Add(0xDE, new PacketToCommand<UpdateVipIncomingPacket>("Update Vip", (connection, packet) => new ParseUpdateVipCommand(connection.Client.Player, packet) ) );
            }

			gameCommands.Add(0xE6, new PacketToCommand<ReportBugIncomingPacket>("Report Bug", (connection, packet) => new ParseReportBugCommand(connection.Client.Player, packet.Message) ) );
			
			gameCommands.Add(0xE8, new PacketToCommand<DebugAssertIncomingPacket>("Debug Assert", (connection, packet) => new ParseDebugAssertCommand(connection.Client.Player, packet.AssertLine, packet.ReportDate, packet.Description, packet.Comment) ) );

            if (HasFeatureFlag(FeatureFlag.QuestLog) )
            {
				gameCommands.Add(0xF0, new PacketToCommand<QuestsIncomingPacket>("Quests", (connection, packet) => new ParseQuestsCommand(connection.Client.Player) ) );

				gameCommands.Add(0xF1, new PacketToCommand<OpenQuestIncomingPacket>("Open Quest", (connection, packet) => new ParseOpenQuestCommand(connection.Client.Player, packet.QuestId) ) );
            }
			
			gameCommands.Add(0xF2, new PacketToCommand<ReportRuleViolationIncomingPacket>("Report Rule Violation", (connection, packet) => new ParseReportRuleViolationCommand(connection.Client.Player, packet.Type, packet.RuleViolation, packet.Name, packet.Comment, packet.Translation, packet.StatmentId) ) );

            if (HasFeatureFlag(FeatureFlag.PlayerMarket) )
			{
                gameCommands.Add(0xF3, new PacketToCommand<GetObjectInfoIncomingPacket>("Get Object Info", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

				gameCommands.Add(0xF4, new PacketToCommand<MarketLeaveIncomingPacket>("Market Leave", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameCommands.Add(0xF5, new PacketToCommand<MarketBrowseIncomingPacket>("Market Browse", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameCommands.Add(0xF6, new PacketToCommand<MarketCreateOfferIncomingPacket>("Market Create Offer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameCommands.Add(0xF7, new PacketToCommand<MarketCancelOfferIncomingPacket>("Market Cancel Offer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameCommands.Add(0xF8, new PacketToCommand<MarketAcceptOfferIncomingPacket>("Market Accept Offer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}

			if (HasFeatureFlag(FeatureFlag.OfflineTrainingTime) )
			{
				gameCommands.Add(0xF9, new PacketToCommand<ModalWindowAnswerIncomingPacket>("Modal Window Answer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            }

            if (HasFeatureFlag(FeatureFlag.LoginPending) )
			{
                gameAccountManagerCommands.Add(0x0F, new PacketToCommand<EnterGame2IncomingPacket>("Enter Game", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}

			gameAccountManagerCommands.Add(0x14, new PacketToCommand<LogOutIncomingPacket>("Log Out", (connection, packet) => new ParseLogOutCommand(connection.Client.Player) ) );
            
			if (HasFeatureFlag(FeatureFlag.ClientPing) )
			{
                gameAccountManagerCommands.Add(0x1D, new PacketToCommand<PingIncomingPacket>("Client ping request", (connection, packet) => new ParsePingCommand(connection.Client.Player) ) );
            }

			gameAccountManagerCommands.Add(0x1E, new PacketToCommand<PongIncomingPacket>("Server ping response", (connection, packet) => new ParsePongCommand(connection.Client.Player) ) );

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

            gameAccountManagerCommands.Add(0x8D, new PacketToCommand<LookInBattleListIncomingPacket>("Look Creature Battle List", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

			gameAccountManagerCommands.Add(0x96, new PacketToCommand<TalkIncomingPacket>("Talk", (connection, packet) => new ParseTalkSayCommand(connection.Client.Player, packet.Message) ) );
            
			gameAccountManagerCommands.Add(0x97, new PacketToCommand<OpenNewChannelIncomingPacket>("Open New Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x98, new PacketToCommand<OpenedNewChannelIncomingPacket>("Opened New Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x99, new PacketToCommand<CloseChannelIncomingPacket>("Close Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0x9A, new PacketToCommand<OpenedPrivateChannelIncomingPacket>("Opened Private Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

            if (HasFeatureFlag(FeatureFlag.RuleViolationChannel) )
            {
				gameAccountManagerCommands.Add(0x9B, new PacketToCommand<ProcessReportRuleViolationIncomingPacket>("Process Report Rule Violation", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

				gameAccountManagerCommands.Add(0x9C, new PacketToCommand<CloseReportRuleViolationChannelAnswerIncomingPacket>("Close Report Rule Violation Channel Answer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
				
				gameAccountManagerCommands.Add(0x9D, new PacketToCommand<CloseReportRuleViolationChannelQuestionIncomingPacket>("Close Report Rule Violation Channel Question", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}

            if (HasFeatureFlag(FeatureFlag.NpcsChannel) )
            {
                gameAccountManagerCommands.Add(0x9E, new PacketToCommand<CloseNpcsChannelIncomingPacket>("Close Npcs Channel", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}
            
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
            
			if (HasFeatureFlag(FeatureFlag.BrowseField) )
			{
                gameAccountManagerCommands.Add(0xCB, new PacketToCommand<BrowseFieldIncomingPacket>("Browse Field", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            }

            if (HasFeatureFlag(FeatureFlag.ContainerPagination) )
			{
                gameAccountManagerCommands.Add(0xCC, new PacketToCommand<SeekInContainerIncomingPacket>("Seek In Container", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            }

			gameAccountManagerCommands.Add(0xD2, new PacketToCommand<SetOutfitIncomingPacket>("Set Outfit", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xD3, new PacketToCommand<SelectedOutfitIncomingPacket>("Selected Outfit", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

            if (HasFeatureFlag(FeatureFlag.PlayerMounts) )
            {
                gameAccountManagerCommands.Add(0xD4, new PacketToCommand<MountIncomingPacket>("Mount", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}

			gameAccountManagerCommands.Add(0xDC, new PacketToCommand<AddVipIncomingPacket>("Add Vip", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			gameAccountManagerCommands.Add(0xDD, new PacketToCommand<RemoveVipIncomingPacket>("Remove Vip", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            
			if (HasFeatureFlag(FeatureFlag.AdditionalVipInfo) )
			{
                gameAccountManagerCommands.Add(0xDE, new PacketToCommand<UpdateVipIncomingPacket>("Update Vip", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            }

			gameAccountManagerCommands.Add(0xE6, new PacketToCommand<ReportBugIncomingPacket>("Report Bug", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			
			gameAccountManagerCommands.Add(0xE8, new PacketToCommand<DebugAssertIncomingPacket>("Debug Assert", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

            if (HasFeatureFlag(FeatureFlag.QuestLog) )
            {
                gameAccountManagerCommands.Add(0xF0, new PacketToCommand<QuestsIncomingPacket>("Quests", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
	
				gameAccountManagerCommands.Add(0xF1, new PacketToCommand<OpenQuestIncomingPacket>("Open Quest", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}
			
			gameAccountManagerCommands.Add(0xF2, new PacketToCommand<ReportRuleViolationIncomingPacket>("Report Rule Violation", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
         
			if (HasFeatureFlag(FeatureFlag.PlayerMarket) )
			{
                gameAccountManagerCommands.Add(0xF3, new PacketToCommand<GetObjectInfoIncomingPacket>("Get Object Info", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameAccountManagerCommands.Add(0xF4, new PacketToCommand<MarketLeaveIncomingPacket>("Market Leave", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameAccountManagerCommands.Add(0xF5, new PacketToCommand<MarketBrowseIncomingPacket>("Market Browse", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameAccountManagerCommands.Add(0xF6, new PacketToCommand<MarketCreateOfferIncomingPacket>("Market Create Offer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameAccountManagerCommands.Add(0xF7, new PacketToCommand<MarketCancelOfferIncomingPacket>("Market Cancel Offer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );

                gameAccountManagerCommands.Add(0xF8, new PacketToCommand<MarketAcceptOfferIncomingPacket>("Market Accept Offer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
			}

			if (HasFeatureFlag(FeatureFlag.OfflineTrainingTime) )
			{
                gameAccountManagerCommands.Add(0xF9, new PacketToCommand<ModalWindowAnswerIncomingPacket>("Modal Window Answer", (connection, packet) => new IgnoreCommand(connection.Client.Player) ) );
            }

			infoFirstCommands.Add(0xFF, new PacketToCommand<InfoIncomingPacket>("Info", (connection, packet) => new ParseInfoProtocolCommand(connection, packet) ) );
         
			#endregion
        }

        public int clientVersion;

		public int ClientVersion
        {
            get
            {
                return clientVersion;
            }
        }

        public uint tibiaDat;

        public uint TibiaDat
        {
            get
            {
                return tibiaDat;
            }
        }

        public uint tibiaPic;

        public uint TibiaPic
        {
            get
            {
                return tibiaPic;				
			}
        }

        public uint tibiaSpr;

		public uint TibiaSpr
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

		public byte GetByteForMagicEffectType(MagicEffectType magicEffectType)
        {
            if (clientVersion < 780)
            {
                if (magicEffectType >= MagicEffectType.Bubbles)
                {
                    magicEffectType =  MagicEffectType.Puff;
                }
            }
			else if (clientVersion < 986)
			{
				if (magicEffectType >= MagicEffectType.OrcChaman)
                {
                    magicEffectType =  MagicEffectType.Puff;
                }
			}
			else if (clientVersion < 1098)
			{
				if (magicEffectType >= MagicEffectType.RedSmoke)
                {
                    magicEffectType =  MagicEffectType.Puff;
                }
			}

            //TODO: Support other versions

            return (byte)magicEffectType;
        }

		public byte GetByteForProjectileType(ProjectileType projectileType)
        {
            if (clientVersion < 780)
            {
                if (projectileType >= ProjectileType.ViperStar)
                {
                    projectileType = ProjectileType.Spear;
                }
            }
			else if (clientVersion < 986)
            {
                if (projectileType >= ProjectileType.Cake)
                {
                    projectileType = ProjectileType.Spear;
                }
            }
			else if (clientVersion < 1098)
            {
                if (projectileType >= ProjectileType.GloothSpear)
                {
                    projectileType = ProjectileType.Spear;
                }
            }

            //TODO: Support other versions

            return (byte)projectileType;
        }

		private Dictionary<FluidType, byte> fluidTypeToByte = new Dictionary<FluidType, byte>();

		private void MapFluidType(FluidType fluidType, byte value)
        {
            fluidTypeToByte.Add(fluidType, value);
        }

        public byte GetByteForFluidType(FluidType fluidType)
        {
            byte value;

            if ( !fluidTypeToByte.TryGetValue(fluidType, out value) )
			{
				value = 0;
            }

			return value;
        }

		private Dictionary<byte, List<MessageMode>> byteToMessageModes = new Dictionary<byte, List<MessageMode>>();

        private Dictionary<MessageMode, byte> messageModeToBytes = new Dictionary<MessageMode, byte>();

        private void MapMessageMode(byte value, MessageMode messageMode)
        {
			List<MessageMode> byteToMessageMode;

            if ( !byteToMessageModes.TryGetValue(value, out byteToMessageMode) )
			{
				byteToMessageMode = new List<MessageMode>();

				byteToMessageModes.Add(value, byteToMessageMode);
            }

			byteToMessageMode.Add(messageMode);

            messageModeToBytes.Add(messageMode, value);
        }

        public MessageMode GetMessageModeForByte(byte value)
		{
			List<MessageMode> byteToMessageMode;

            if ( !byteToMessageModes.TryGetValue(value, out byteToMessageMode) )
			{
				return MessageMode.Say;
            }

			return byteToMessageMode[0];
		}

        public byte GetByteForMessageMode(MessageMode messageMode)
		{
			byte value;

            if ( !messageModeToBytes.TryGetValue(messageMode, out value) )
			{
				messageModeToBytes.TryGetValue(MessageMode.Say, out value);
			}

			return value;
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
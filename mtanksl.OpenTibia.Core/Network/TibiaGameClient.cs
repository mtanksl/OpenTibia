using OpenTibia.IO;
using OpenTibia.Security;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace OpenTibia
{
    public partial class TibiaGameClient : TibiaClient
    {
        public TibiaGameClient(Socket socket) : base(socket)
        {
            PingRequest = DateTime.Now;
        }

        public Player Player { get; set; }

        public FightMode FightMode { get; set; }

        public ChaseMode ChaseMode { get; set; }

        public SafeMode SafeMode { get; set; }

        public SchedulerEvent WalkSchedulerEvent { get; set; }

        public DateTime PingRequest { get; set; }

        public DateTime PingResponse { get; set; }

        private HashSet<uint> creatureIds = new HashSet<uint>();

        public bool IsKnownCreature(uint creatureId, out uint removeId)
        {
            if ( creatureIds.Add(creatureId) )
            {
                if (creatureIds.Count > 250)
                {
                    removeId = creatureIds.Where(id =>
                    {
                        if (id != Player.Id)
                        {
                            Creature creature = Game.Current.Map.GetCreature(id);

                            if (creature == null || !Player.Tile.Position.CanSee(creature.Tile.Position) )
                            {
                                return true;
                            }
                        }

                        return false;

                    } ) .FirstOrDefault();

                    if (removeId == 0)
                    {
                        removeId = creatureIds.Where(id =>
                        {
                            if (id != Player.Id)
                            {
                                return true;
                            }

                            return false;

                        } ).First();
                    }

                    creatureIds.Remove(removeId);

                    return false;
                }

                removeId = 0;

                return false;
            }

            removeId = 0;

            return true;
        }

        protected override void OnConnect()
        {
            ExecuteInContext( () => 
            {
                Response.Write(new ConnectingOutgoingPacket() );
            } );
        }
                
        protected override void OnReceive(byte[] bytes, bool first)
        {
            ByteArrayArrayStream stream = new ByteArrayArrayStream(bytes);

            ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

            try
            {
                if (Adler32.Generate(bytes, 4) == reader.ReadUInt() )
                {
                    if (Keys == null)
                    {
                        Rsa.DecryptAndReplace(bytes, 9);
                    }
                    else
                    {
                        Xtea.DecryptAndReplace(bytes, 4, 32, Keys);

                        stream.Seek(Origin.Current, 2);
                    }

                    byte identifier = reader.ReadByte();

                        Game.Current.Log.WriteLine("Received 0x{0:X2}", identifier);

                    if (first)
                    {
                        switch (identifier)
                        {
                            case 0x0A:

                                ExecuteInContext<LoginIncomingPacket>(reader, packet =>
                                {
                                    Login(packet);
                                } );

                                break;
                        }
                    }
                    else
                    {
                        switch (identifier)
                        {
                            case 0x14:

                                ExecuteInContext( () => 
                                {
                                    OnDisconnect(new DisconnectEventArgs(DisconnetionType.Logout) );
                                } );

                                break;

                            case 0x1E:

                                ExecuteInContext( () => 
                                {
                                    Ping();
                                } );

                                break;

                            case 0x64:

                                ExecuteInContext<WalkToIncomingPacket>(reader, packet =>
                                {
                                    WalkTo(packet);
                                } );

                                break;
                        
                            case 0x65:

                                ExecuteInContext( () =>
                                {
                                    Walk(MoveDirection.North);
                                } );

                                break;

                            case 0x66:

                                ExecuteInContext( () => 
                                {
                                    Walk(MoveDirection.East);
                                } );

                                break;

                            case 0x67:

                                ExecuteInContext( () => 
                                {
                                    Walk(MoveDirection.South);
                                } );

                                break;

                            case 0x68:

                                ExecuteInContext( () =>
                                {
                                    Walk(MoveDirection.West);
                                } );

                                break;

                            case 0x69:

                                ExecuteInContext( () => 
                                {
                                    StopWalk();
                                } );

                                break;

                            case 0x6A:

                                ExecuteInContext( () => 
                                {
                                    Walk(MoveDirection.NorthEast);
                                } );

                                break;

                            case 0x6B:

                                ExecuteInContext( () => 
                                {
                                    Walk(MoveDirection.SouthEast);
                                } );

                                break;

                            case 0x6C:

                                ExecuteInContext( () => 
                                {
                                    Walk(MoveDirection.SouthWest);
                                } );

                                break;

                            case 0x6D:

                                ExecuteInContext( () => 
                                {
                                    Walk(MoveDirection.NorthWest);
                                } );

                                break;

                            case 0x6F:

                                ExecuteInContext( () => 
                                {
                                    Turn(Direction.North);
                                } );

                                break;

                            case 0x70:

                                ExecuteInContext( () => 
                                {
                                    Turn(Direction.East);
                                } );

                                break;

                            case 0x71:

                                ExecuteInContext( () => 
                                {
                                    Turn(Direction.South);
                                } );

                                break;

                            case 0x72:

                                ExecuteInContext( () => 
                                {
                                    Turn(Direction.West);
                                } );

                                break;

                            case 0x78:

                                ExecuteInContext<MoveItemIncomingPacket>(reader, packet => 
                                {
                                    MoveItem(packet);
                                } );

                                break;

                            case 0x79:

                                ExecuteInContext<LookItemNpcTradeIncomingPacket>(reader, packet =>
                                {
                                    LookItemNpcTrade(packet);
                                } );

                                break;

                            case 0x7A:

                                ExecuteInContext<BuyNpcTradeIncomingPacket>(reader, packet => 
                                {
                                    BuyNpcTrade(packet);
                                } );

                                break;

                            case 0x7B:

                                ExecuteInContext<SellNpcTradeIncomingPacket>(reader, packet => 
                                {
                                    SellNpcTrade(packet);
                                } );

                                break;

                            case 0x7C:

                                ExecuteInContext( () => 
                                {
                                    CloseNpcTrade();
                                } );

                                break;

                            case 0x7D:

                                ExecuteInContext<TradeWithIncomingPacket>(reader, packet => 
                                {
                                    TradeWith(packet);
                                } );

                                break;

                            case 0x7E:

                                ExecuteInContext<LookItemTradeIncomingPacket>(reader, packet => 
                                {
                                    LookItemTrade(packet);
                                } );

                                break;

                            case 0x7F:

                                ExecuteInContext( () => 
                                {
                                    AcceptTrade();
                                } );

                                break;

                            case 0x80:

                                ExecuteInContext( () => 
                                {
                                    CancelTrade();
                                } );

                                break;

                            case 0x82:

                                ExecuteInContext<UseItemIncomingPacket>(reader, packet =>
                                {
                                    UseItem(packet);
                                } );

                                break;

                            case 0x83:

                                ExecuteInContext<UseItemWithItemIncomingPacket>(reader, packet => 
                                {
                                    UseItemWithItem(packet);
                                } );

                                break;

                            case 0x84:

                                ExecuteInContext<UseItemWithCreatureIncomingPacket>(reader, packet => 
                                {
                                    UseItemWithCreature(packet);
                                } );

                                break;

                            case 0x85:

                                ExecuteInContext<RotateItemIncomingPacket>(reader, packet => 
                                {
                                    RotateItem(packet);
                                } );

                                break;

                            case 0x87:

                                ExecuteInContext<CloseContainerIncomingPacket>(reader, packet => 
                                {
                                    CloseContainer(packet);
                                } );

                                break;

                            case 0x88:

                                ExecuteInContext<OpenParentIncomingPacket>(reader, packet => 
                                {
                                    OpenParent(packet);
                                } );

                                break;

                            case 0x8C:

                                ExecuteInContext<LookIncomingPacket>(reader, packet => 
                                {
                                    Look(packet);
                                } );

                                break;

                            case 0x96:

                                ExecuteInContext<TalkIncomingPacket>(reader, packet => 
                                {
                                    Talk(packet);
                                } );

                                break;

                            case 0x97:

                                ExecuteInContext( () => 
                                {
                                    Channels();
                                } );

                                break;

                            case 0x98:

                                ExecuteInContext<OpenChannelIncomingPacket>(reader, packet => 
                                {
                                    OpenChannel(packet);
                                } );

                                break;

                            case 0x99:

                                ExecuteInContext<CloseChannelIncomingPacket>(reader, packet => 
                                {
                                    CloseChannel(packet);
                                } );

                                break;

                            case 0x9A:

                                ExecuteInContext<OpenPrivateChannelIncomingPacket>(reader, packet => 
                                {
                                    OpenPrivateChannel(packet);
                                } );

                                break;

                            case 0x9B:

                                ExecuteInContext<OpenReportRuleViolationChannelIncomingPacket>(reader, packet => 
                                {
                                    OpenReportRuleViolationChannel(packet);
                                } );

                                break;

                            case 0x9C:

                                ExecuteInContext<CloseReportRuleViolationChannelAnswerIncomingPacket>(reader, packet =>
                                {
                                    CloseReportRuleViolationAnswer(packet);
                                } );

                                break;
                        
                            case 0x9D:

                                ExecuteInContext( () => 
                                {
                                    CloseReportRuleViolationQuestion();
                                } );

                                break;

                            case 0x9E:

                                ExecuteInContext( () => 
                                {
                                    CloseNpcsChannel();
                                } );

                                break;

                            case 0xA0:

                                ExecuteInContext<CombatControlsIncomingPacket>(reader, packet => 
                                {
                                    CombatControls(packet);
                                } );

                                break;

                            case 0xA1:

                                ExecuteInContext<AttackIncomingPacket>(reader, packet => 
                                {
                                    Attack(packet);
                                } );

                                break;

                            case 0xA2:

                                ExecuteInContext<FollowIncomingPacket>(reader, packet => 
                                {
                                    Follow(packet);
                                } );

                                break;

                            case 0xA3:

                                ExecuteInContext<InviteToPartyIncomingPacket>(reader, packet => 
                                {
                                    InviteToParty(packet);
                                } );

                                break;

                            case 0xA4:

                                ExecuteInContext<JoinPartyIncomingPacket>(reader, packet => 
                                {
                                    JoinParty(packet);
                                } );

                                break;

                            case 0xA5:

                                ExecuteInContext<RevokePartyIncomingPacket>(reader, packet => 
                                {
                                    RevokeParty(packet);
                                } );

                                break;

                            case 0xA6:

                                ExecuteInContext<PassLeadershipToIncomingPacket>(reader, packet => 
                                {
                                    PassLeadershipTo(packet);
                                } );

                                break;

                            case 0xA7:

                                ExecuteInContext( () => 
                                {
                                    LeaveParty();
                                } );

                                break;

                            case 0xA8:

                                ExecuteInContext<SharedExperienceIncomingPacket>(reader, packet =>
                                {
                                    SharedExperience(packet);
                                } );

                                break;

                            case 0xAA:

                                ExecuteInContext( () => 
                                {
                                    OpenMyPrivateChannel();
                                } );

                                break;

                            case 0xAB:

                                ExecuteInContext<InvitePlayerIncomingPacket>(reader, packet => 
                                {
                                    InvitePlayer(packet);
                                } );
                        
                                break;

                            case 0xAC:

                                ExecuteInContext<ExcludePlayerIncomingPacket>(reader, packet => 
                                {
                                    ExcludePlayer(packet);
                                } );

                                break;

                            case 0xBE:

                                ExecuteInContext( () => 
                                {
                                    Stop();
                                } );

                                break;

                            case 0xD2:

                                ExecuteInContext( () => 
                                {
                                    SelectOutfit();
                                } );

                                break;

                            case 0xD3:

                                ExecuteInContext<ChangeOutfitIncomingPacket>(reader, packet => 
                                {
                                    ChangeOutfit(packet);
                                } );

                                break;

                            case 0xDC:

                                ExecuteInContext<AddVipIncomingPacket>(reader, packet => 
                                {
                                    AddVip(packet);
                                } );

                                break;

                            case 0xDD:

                                ExecuteInContext<RemoveVipIncomingPacket>(reader, packet => 
                                {
                                    RemoveVip(packet);
                                } );

                                break;

                            case 0xE6:

                                ExecuteInContext<ReportBugIncomingPacket>(reader, packet => 
                                {
                                    ReportBug(packet);
                                } );

                                break;

                            case 0xF0:

                                ExecuteInContext( () => 
                                {
                                    Quests();
                                } );

                                break;

                            case 0xF1:

                                ExecuteInContext<QuestIncomingPacket>(reader, packet => 
                                {
                                    Quest(packet);
                                } );

                                break;
                        }
                    }                    
                }
            }
            catch (Exception e)
            {
                Game.Current.Log.WriteLine("Exception {0}", e.Message);
            }
        }

        protected override void OnDisconnect(DisconnectEventArgs e)
        {
            ExecuteInContext( () => 
            {
                if (Player != null)
                {
                    if (WalkSchedulerEvent != null)
                    {
                        WalkSchedulerEvent.Cancel();
                    }
                    
                    List<Chat> chatsToRemove = new List<Chat>();
                    
                    foreach (var chat in Game.Current.Chats.GetChats() )
                    {
                        PrivateChat privateChat = chat as PrivateChat;

                        if (privateChat != null)
                        {
                            if (privateChat.Owner == Player)
                            {
                                foreach (var observer in privateChat.GetPlayers() )
                                {
                                    if (observer != Player)
                                    {
                                        observer.Client.Response.Write(new CloseChannelOutgoingPacket(privateChat.Id) );
                                    }
                                }

                                chatsToRemove.Add(privateChat);
                            }
                            else
                            {
                                if ( privateChat.ContainsInvitation(Player) )
                                {
                                    privateChat.RemoveInvitation(Player);
                                }
                                else if ( privateChat.ContainsPlayer(Player) )
                                {
                                    privateChat.RemovePlayer(Player);
                                }
                            }                            
                        }
                        else
                        {
                            if ( chat.ContainsPlayer(Player) )
                            {
                                chat.RemovePlayer(Player);
                            }
                        }
                    }
                    
                    foreach (var chatToRemove in chatsToRemove)
                    {
                        Game.Current.Chats.RemoveChat(chatToRemove);
                    }
                    
                    List<RuleViolation> ruleViolationsToRemove = new List<RuleViolation>();
                    
                    foreach (var ruleViolation in Game.Current.RuleViolations.GetRuleViolations() )
                    {
                        if (ruleViolation.Reporter == Player)
                        {
                            if (ruleViolation.Assignee == null)
                            {
                                foreach (var observer in Game.Current.Chats.GetChat(3).GetPlayers() )
                                {
                                    observer.Client.Response.Write(new RemoveReportOutgoingPacket(ruleViolation.Reporter.Name) );
                                }

                                ruleViolationsToRemove.Add(ruleViolation);
                            }
                            else
                            {
                                ruleViolation.Assignee.Client.Response.Write(new CancelReportOutgoingPacket(ruleViolation.Reporter.Name) );

                                ruleViolationsToRemove.Add(ruleViolation);
                            }
                        }
                        else if (ruleViolation.Assignee == Player)
                        {
                            ruleViolation.Reporter.Client.Response.Write(new CloseReportOutgoingPacket() );

                            ruleViolationsToRemove.Add(ruleViolation);
                        }
                    }

                    foreach (var ruleViolationToRemove in ruleViolationsToRemove)
                    {
                        Game.Current.RuleViolations.RemoveRuleViolation(ruleViolationToRemove);
                    }
                    
                    Position position = Player.Tile.Position; byte index = (byte)Player.Tile.RemoveContent(Player);

                    foreach (var observer in Game.Current.Map.GetPlayers() )
                    {
                        if (observer != Player)
                        {
                            if (observer.Tile.Position.CanSee(position) )
                            {
                                observer.Client.Response.Write(new ThingRemoveOutgoingPacket(position, index) )
                            
                                                        .Write(new MagicEffectOutgoingPacket(position, MagicEffectType.Puff) );
                            }
                        }
                    }
                    
                    Game.Current.Map.RemoveCreature(Player);
                }
            } );

            base.OnDisconnect(e);
        }
    }
}
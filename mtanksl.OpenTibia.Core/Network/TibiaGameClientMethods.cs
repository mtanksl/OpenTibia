using OpenTibia.Common.Structures;
using OpenTibia.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia
{
    public partial class TibiaGameClient 
    {
        private void Login(LoginIncomingPacket packet)
        {
            Keys = packet.Keys;

            if (packet.Version != 860)
            {
                Response.Write( new SorryOutgoingPacket(false, "Only protocol 8.6 allowed.") );

                return;
            }

            var account = new PlayerRepository().GetPlayer(packet.Account, packet.Password, packet.Character);

            if (account == null)
            {
                Response.Write( new SorryOutgoingPacket(false, "Account name or password is not correct.") );

                return;
            }

            Player = Game.Current.Map.AddCreature(new Player()
            {
                Client = this,

                Name = account.Name
            } );

            Position position = new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ); byte index = (byte)Game.Current.Map.GetTile(position).AddContent(Player);

            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                if (observer == Player)
                {
                    observer.Client.Response.Write(new InfoOutgoingPacket(Player.Id, Player.CanReportBugs) )

                                            .Write(new SpecialConditionOutgoingPacket(SpecialCondition.None) )
                    
                                            .Write(new StatusOutgoingPacket(Player.Health, Player.MaxHealth, Player.Capacity, Player.Experience, Player.Level, Player.LevelPercent, Player.Mana, Player.MaxMana, Player.Skills.GetValue(Skill.MagicLevel), Player.Skills.GetPercent(Skill.MagicLevel), Player.Soul, Player.Stamina) )
                    
                                            .Write(new SkillsOutgoingPacket(Player.Skills.GetValue(Skill.Fist), Player.Skills.GetPercent(Skill.Fist), Player.Skills.GetValue(Skill.Club), Player.Skills.GetPercent(Skill.Club), Player.Skills.GetValue(Skill.Sword), Player.Skills.GetPercent(Skill.Sword), Player.Skills.GetValue(Skill.Axe), Player.Skills.GetPercent(Skill.Axe), Player.Skills.GetValue(Skill.Distance), Player.Skills.GetPercent(Skill.Distance), Player.Skills.GetValue(Skill.Shield), Player.Skills.GetPercent(Skill.Shield), Player.Skills.GetValue(Skill.Fish), Player.Skills.GetPercent(Skill.Fish) ) )

                                            .Write(new EnvironmentLightOutgoingPacket(new Light(Game.Current.Level, 215) ) )

                                            .Write(new TilesOutgoingPacket(Player.Client, position) )

                                            .Write(new MagicEffectOutgoingPacket(position, MagicEffectType.Teleport) );
                }
                else if (observer.Tile.Position.CanSee(position) )
                {
                    uint removeId;

                    if (observer.Client.IsKnownCreature(Player.Id, out removeId) )
                    {
                        observer.Client.Response.Write(new ThingAddOutgoingPacket(position, index, Player) )
                        
                                                .Write(new MagicEffectOutgoingPacket(position, MagicEffectType.Teleport) );
                    }
                    else
                    {
                        observer.Client.Response.Write(new ThingAddOutgoingPacket(position, index, removeId, Player) )

                                                .Write(new MagicEffectOutgoingPacket(position, MagicEffectType.Teleport) );
                    }
                }
            }
        }
        
        private void Ping()
        {
            PingResponse = DateTime.Now;

            Game.Current.Log.WriteLine("Ping from {0} in {1} ms", Player.Name, (int)PingResponse.Subtract(PingRequest).TotalMilliseconds);
        }
        
        private void WalkTo(WalkToIncomingPacket packet)
        {
            if (WalkSchedulerEvent != null)
            {
                if ( WalkSchedulerEvent.Cancel() )
                {
                    Response.Write(new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            Action<int> callback = null;

            callback = (index) =>
            {
                if (index != packet.MoveDirections.Length)
                {
                    WalkSchedulerEvent = Game.Current.Scheduler.QueueForExecution(Player.WalkDelay, () =>
                    {
                        OnMove(Player, packet.MoveDirections[index] );

                        callback(index + 1);
                    } );
                }
            };

            callback(0);
        }

        private void Walk(MoveDirection moveDirection)
        {
            if (WalkSchedulerEvent != null)
            {
                if ( WalkSchedulerEvent.Cancel() )
                {
                    Response.Write(new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            if ( Player.CanMove(moveDirection) )
            {
                WalkSchedulerEvent = Game.Current.Scheduler.QueueForExecution(Player.WalkDelay, () =>
                {
                    OnMove(Player, moveDirection);
                } );
            }
            else
            {
                Player.Client.Response.Write(new StopWalkOutgoingPacket(Player.Direction))

                                      .Write(new TextOutgoingPacket(TextColor.WhiteBottomGameWindow, "Sorry, not possible."));
            }
        }
        
        private void OnMove(Player player, MoveDirection moveDirection)
        {
            Tile fromTile2 = player.Tile;

            Position fromPosition = player.Tile.Position; 
            
            Position toPosition = fromPosition.Offset(moveDirection);

            Tile toTile = Game.Current.Map.GetTile(toPosition);

            if (toTile == null)
            {
                Tile toTileDown = Game.Current.Map.GetTile(toPosition.Offset(0, 0, 1) );

                if (toTileDown != null && toTileDown.Walkable && toTileDown.HasHeight)
                {
                    toTile = toTileDown;
                }
                else
                {
                    Player.Client.Response.Write(new StopWalkOutgoingPacket(Player.Direction) )
                        
                                          .Write(new TextOutgoingPacket(TextColor.WhiteBottomGameWindow, "Sorry, not possible.") );
                    return;
                }
            }
            else if (!toTile.Walkable || toTile.GetCreatures().Any() )
            {
                Tile fromTile = Game.Current.Map.GetTile(fromPosition);

                Tile fromTileUp = Game.Current.Map.GetTile(fromPosition.Offset(0, 0, -1) );

                Tile toTileUp = Game.Current.Map.GetTile(toPosition.Offset(0, 0, -1) );

                if (fromTile.HasHeight && fromTileUp == null && toTileUp != null && toTileUp.Walkable)
                {
                    toTile = toTileUp;
                }
                else
                {
                    Player.Client.Response.Write(new StopWalkOutgoingPacket(Player.Direction) )
                        
                                          .Write(new TextOutgoingPacket(TextColor.WhiteBottomGameWindow, "Sorry, not possible.") );
                    return;
                }
            }

            if (toTile.FloorChange == FloorChange.None)
            {

            }
            else if (toTile.FloorChange == FloorChange.Down)
            {
                Position toPositionDown = toTile.Position.Offset(0, 0, 1); Tile toTileDown = Game.Current.Map.GetTile(toPositionDown);

                if (toTileDown.FloorChange.Any(FloorChange.East) )
                {
                    toPositionDown = toPositionDown.Offset(-1, 0, 0);
                }

                if (toTileDown.FloorChange.Any(FloorChange.North) )
                {
                    toPositionDown = toPositionDown.Offset(0, 1, 0);
                }

                if (toTileDown.FloorChange.Any(FloorChange.West) )
                {
                    toPositionDown = toPositionDown.Offset(1, 0, 0);
                }

                if (toTileDown.FloorChange.Any(FloorChange.South) )
                {
                    toPositionDown = toPositionDown.Offset(0, -1, 0);
                }

                toTile = Game.Current.Map.GetTile(toPositionDown);
            }
            else
            {
                Position toPositionUp = toTile.Position.Offset(0, 0, -1);
                
                if (toTile.FloorChange.Any(FloorChange.East) )
                {
                    toPositionUp = toPositionUp.Offset(1, 0, 0);
                }

                if (toTile.FloorChange.Any(FloorChange.North) )
                {
                    toPositionUp = toPositionUp.Offset(0, -1, 0);
                }

                if (toTile.FloorChange.Any(FloorChange.West) )
                {
                    toPositionUp = toPositionUp.Offset(-1, 0, 0);
                }

                if (toTile.FloorChange.Any(FloorChange.South) )
                {
                    toPositionUp = toPositionUp.Offset(0, 1, 0);
                }

                toTile = Game.Current.Map.GetTile(toPositionUp);
            }

            toPosition = toTile.Position;
            
            byte fromIndex = (byte)player.Tile.RemoveContent(player);
               
            byte toIndex = (byte)Game.Current.Map.GetTile(toPosition).AddContent(player);

            player.DiagonalDelay = fromPosition.ToMoveDirection(toPosition).IsDiagonal() ? 2 : 1;

            player.Direction = fromPosition.ToDirection(toPosition);
            
            Delta delta = fromPosition.ToDelta(toPosition);

            if (delta.ModuleX > 1 || delta.ModuleY > 1 || delta.ModuleZ > 1)
            {
                player.Client.Response.Write(new TilesOutgoingPacket(player.Client, toPosition) );
            }
            else
            {
                if (fromPosition.CanSee(toPosition) )
                {
                    player.Client.Response.Write(new WalkOutgoingPacket(fromPosition, fromIndex, toPosition));
                }
                else
                {
                    player.Client.Response.Write(new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
                }

                if (delta.Z == -1)
                {
                    player.Client.Response.Write(new WalkUpOutgoingPacket(player.Client, fromPosition) );
                }
                else if (delta.Z == 1)
                {
                    player.Client.Response.Write(new WalkDownOutgoingPacket(player.Client, fromPosition) );
                }

                if (delta.Y == -1)
                {
                    player.Client.Response.Write(new WalkNorthOutgoingPacket(player.Client, fromPosition.Offset(0, 0, delta.Z) ) );
                }
                else if (delta.Y == 1)
                {
                    player.Client.Response.Write(new WalkSouthOutgoingPacket(player.Client, fromPosition.Offset(0, 0, delta.Z) ) );
                }

                if (delta.X == -1)
                {
                    player.Client.Response.Write(new WalkWestOutgoingPacket(player.Client, fromPosition.Offset(0, delta.Y, delta.Z) ) );
                }
                else if (delta.X == 1)
                {
                    player.Client.Response.Write(new WalkEastOutgoingPacket(player.Client, fromPosition.Offset(0, delta.Y, delta.Z) ) );
                }
            }

            foreach (var observer in Game.Current.Map.GetPlayers() )
            {
                if (observer != player)
                {
                    if (observer.Tile.Position.CanSee(fromPosition) && observer.Tile.Position.CanSee(toPosition) )
                    {
                        observer.Client.Response.Write(new WalkOutgoingPacket(fromPosition, fromIndex, toPosition) );
                    }
                    else if (observer.Tile.Position.CanSee(fromPosition) )
                    {
                        observer.Client.Response.Write(new ThingRemoveOutgoingPacket(fromPosition, fromIndex) );
                    }
                    else if (observer.Tile.Position.CanSee(toPosition) )
                    {
                        uint removeId;

                        if (observer.Client.IsKnownCreature(player.Id, out removeId) )
                        {
                            observer.Client.Response.Write(new ThingAddOutgoingPacket(toPosition, toIndex, player) );
                        }
                        else
                        {
                            observer.Client.Response.Write(new ThingAddOutgoingPacket(toPosition, toIndex, removeId, player) );
                        }
                    }
                }
            }

            Game.Current.EventBus.Publish( new CreatureMoveEvent(player, fromTile2, fromIndex, toTile, toIndex) );
        }
        
        private void StopWalk()
        {
            if (WalkSchedulerEvent != null)
            {
                if ( WalkSchedulerEvent.Cancel() )
                {
                    Response.Write(new StopWalkOutgoingPacket(Player.Direction) );
                }
            }
        }
        
        private void Turn(Direction direction)
        {
            if (WalkSchedulerEvent != null)
            {
                if ( WalkSchedulerEvent.Cancel() )
                {
                    Response.Write(new StopWalkOutgoingPacket(Player.Direction) );
                }
            }

            Player.Turn(direction);
        }

        private void MoveItem(MoveItemIncomingPacket packet)
        {
            //TODO
        }
        
        private void LookItemNpcTrade(LookItemNpcTradeIncomingPacket packet)
        {
            //TODO
        }
        
        private void BuyNpcTrade(BuyNpcTradeIncomingPacket packet)
        {
            //TODO
        }
        
        private void SellNpcTrade(SellNpcTradeIncomingPacket packet)
        {
            //TODO
        }
        
        private void CloseNpcTrade()
        {
            //TODO
        }
        
        private void TradeWith(TradeWithIncomingPacket packet)
        {
            //TODO
        }
        
        private void LookItemTrade(LookItemTradeIncomingPacket packet)
        {
            //TODO
        }
        
        private void AcceptTrade()
        {
            //TODO
        }
        
        private void CancelTrade()
        {
            //TODO
        }
                
        private void UseItem(UseItemIncomingPacket packet)
        {
            //TODO
        }
        
        private void UseItemWithItem(UseItemWithItemIncomingPacket packet)
        {
            //TODO
        }
        
        private void UseItemWithCreature(UseItemWithCreatureIncomingPacket packet)
        {
            //TODO
        }
        
        private void RotateItem(RotateItemIncomingPacket packet)
        {
            //TODO
        }
        
        private void CloseContainer(CloseContainerIncomingPacket packet)
        {
            //TODO
        }
        
        private void OpenParent(OpenParentIncomingPacket packet)
        {
            //TODO
        }
        
        private void Look(LookIncomingPacket packet)
        {
            //TODO
        }
        
        private void Talk(TalkIncomingPacket packet)
        {
            switch (packet.TalkType)
            {
                case TalkType.Say:
                {
                    foreach (var observer in Game.Current.Map.GetPlayers() )
                    {
                        if (observer == Player || observer.Tile.Position.CanHearSay(Player.Tile.Position))
                        {
                            ushort level = Player.Level;

                            observer.Client.Response.Write( new TalkOutgoingPacket(0, Player.Name, level, TalkType.Say, Player.Tile.Position, packet.Message) );
                        }
                    }
                    
                    break;
                }

                case TalkType.Whisper:
                {
                    foreach (var observer in Game.Current.Map.GetPlayers())
                    {
                        if (observer == Player || observer.Tile.Position.CanHearWhisper(Player.Tile.Position) )
                        {
                            observer.Client.Response.Write(new TalkOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, packet.Message) );
                        }
                        else if (observer.Tile.Position.CanHearSay(Player.Tile.Position) )
                        {
                            observer.Client.Response.Write(new TalkOutgoingPacket(0, Player.Name, Player.Level, TalkType.Whisper, Player.Tile.Position, "pspsps") );
                        }
                    }

                    break;
                }

                case TalkType.Yell:
                {
                    foreach (var observer in Game.Current.Map.GetPlayers() )
                    {
                        if (observer == Player || observer.Tile.Position.CanHearYell(Player.Tile.Position) )
                        {
                            observer.Client.Response.Write(new TalkOutgoingPacket(0, Player.Name, Player.Level, TalkType.Yell, Player.Tile.Position, packet.Message.ToUpper() ) );
                        }
                    }

                    break;
                }

                case TalkType.Private:

                case TalkType.PrivateRed:
                {
                    Player observer = Game.Current.Map.GetPlayer(packet.Name);

                    if (observer != null)
                    {
                        if (observer != Player)
                        {
                            observer.Client.Response.Write(new TalkOutgoingPacket(0, Player.Name, Player.Level, TalkType.Private, packet.Message) );
                        }
                    }

                    break;
                }

                case TalkType.ChannelYellow:

                case TalkType.ChannelWhite:

                case TalkType.ChannelRed:

                case TalkType.ChannelOrange:

                case TalkType.ChannelRedAnonymous:
                {
                    Chat chat = Game.Current.Chats.GetChat(packet.ChannelId);

                    if (chat != null)
                    {
                        PrivateChat privateChat = chat as PrivateChat;

                        if (privateChat != null)
                        {
                            if (privateChat.ContainsPlayer(Player) )
                            {
                                foreach (var observer in chat.GetPlayers() )
                                {
                                    observer.Client.Response.Write(new TalkOutgoingPacket(0, Player.Name, Player.Level, TalkType.ChannelYellow, chat.Id, packet.Message) );
                                }
                            }
                        }
                    }

                     break;
                }
                    
                case TalkType.ReportRuleViolationOpen:
                {
                    RuleViolation ruleViolation = Game.Current.RuleViolations.GetRuleViolation(Player);

                    if (ruleViolation == null)
                    {
                        ruleViolation = Game.Current.RuleViolations.AddRuleViolation(new RuleViolation()
                        {
                            Reporter = Player,

                            Message = packet.Message
                        } );

                        foreach (var observer in Game.Current.Chats.GetChat(3).GetPlayers() )
                        {
                            observer.Client.Response.Write(new TalkOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                        }
                    }

                    break;
                }

                case TalkType.ReportRuleViolationAnswer:
                {
                    Player observer = Game.Current.Map.GetPlayer(packet.Name);

                    if (observer != null)
                    {
                        RuleViolation ruleViolation = Game.Current.RuleViolations.GetRuleViolation(observer);

                        if (ruleViolation != null)
                        {
                            if (ruleViolation.Assignee != null)
                            {
                                if (ruleViolation.Assignee == Player)
                                {
                                    ruleViolation.Reporter.Client.Response.Write(new TalkOutgoingPacket(0, ruleViolation.Assignee.Name, ruleViolation.Assignee.Level, TalkType.ReportRuleViolationAnswer, packet.Message) );
                                }
                            }
                        }
                    }
                    
                    break;
                }

                case TalkType.ReportRuleViolationQuestion:
                {
                    RuleViolation ruleViolation = Game.Current.RuleViolations.GetRuleViolation(Player);

                    if (ruleViolation != null)
                    {
                        if (ruleViolation.Assignee != null)
                        {
                            ruleViolation.Assignee.Client.Response.Write(new TalkOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationQuestion, packet.Message) );
                        }
                    }

                    break;
                }
            }
        }
        
        private void Channels()
        {
            List<Channel> channels = new List<Channel>()
            {
                new Channel(0, "Guild"),

                new Channel(1, "Party"),

                new Channel(2, "Tutor"),

                new Channel(3, "Rule Violations"),

                new Channel(4, "Gamemaster"),

                new Channel(5, "Game chat"),

                new Channel(6, "Trade"),

                new Channel(7, "Trade-Rookgaard"),

                new Channel(8, "Real Life Chat"),

                new Channel(9, "Help"),

                new Channel(65535, "Private Chat Channel")
            };
                
            foreach (var privateChat in Game.Current.Chats.GetPrivateChats() )
            {
                if (privateChat.ContainsInvitation(Player) || privateChat.ContainsPlayer(Player) )
                {
                    channels.Add(new Channel(privateChat.Id, privateChat.Name) );
                }
            }

            Response.Write(new NewChannelOutgoingPacket(channels) );
        }
        
        private void OpenChannel(OpenChannelIncomingPacket packet)
        {
            Chat chat = Game.Current.Chats.GetChat(packet.ChannelId);

            if (chat != null)
            {
                PrivateChat privateChat = chat as PrivateChat;

                if (privateChat != null)
                {
                    if ( privateChat.ContainsInvitation(Player) )
                    {
                        privateChat.RemoveInvitation(Player);

                        privateChat.AddPlayer(Player);
                    }

                    if ( !privateChat.ContainsPlayer(Player) )
                    {
                        return;
                    }
                }
                else
                {
                    if ( !chat.ContainsPlayer(Player) )
                    {
                        chat.AddPlayer(Player);
                    }
                }

                if (chat.Id == 3)
                {
                    Response.Write(new OpenRuleViolationsChannelOutgoingPacket(chat.Id) );

                    foreach (var ruleViolation in Game.Current.RuleViolations.GetRuleViolations() )
                    {
                        if (ruleViolation.Assignee == null)
                        {
                            Response.Write(new TalkOutgoingPacket(0, ruleViolation.Reporter.Name, ruleViolation.Reporter.Level, TalkType.ReportRuleViolationOpen, ruleViolation.Time, ruleViolation.Message) );
                        }
                    }
                }
                else
                {
                    Response.Write(new OpenChannelOutgoingPacket(chat.Id, chat.Name) );
                }
            }
        }
        
        private void CloseChannel(CloseChannelIncomingPacket packet)
        {
            Chat chat = Game.Current.Chats.GetChat(packet.ChannelId);

            if (chat != null)
            {
                PrivateChat privateChat = chat as PrivateChat;

                if (privateChat != null)
                {
                    if ( privateChat.ContainsInvitation(Player) )
                    {

                    }
                    else if ( privateChat.ContainsPlayer(Player) )
                    {
                        chat.RemovePlayer(Player);
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
        }
        
        private void OpenPrivateChannel(OpenPrivateChannelIncomingPacket packet)
        {
            Player observer = Game.Current.Map.GetPlayer(packet.Name);

            if (observer != null)
            {
                if (observer != Player)
                {
                    Response.Write(new OpenPrivateChannelOutgoingPacket(observer.Name) );
                }
            }
        }
        
        private void OpenReportRuleViolationChannel(OpenReportRuleViolationChannelIncomingPacket packet)
        {
            Player observer = Game.Current.Map.GetPlayer(packet.Name);

            if (observer != null)
            {
                RuleViolation ruleViolation = Game.Current.RuleViolations.GetRuleViolation(observer);

                if (ruleViolation != null)
                {
                    if (ruleViolation.Assignee == null)
                    {
                        ruleViolation.Assignee = Player;

                        foreach (var observer2 in Game.Current.Chats.GetChat(3).GetPlayers() )
                        {
                            observer2.Client.Response.Write(new RemoveReportOutgoingPacket(ruleViolation.Reporter.Name) );
                        }
                    }
                }
            }
        }
        
        private void CloseReportRuleViolationAnswer(CloseReportRuleViolationChannelAnswerIncomingPacket packet)
        {
            Player observer = Game.Current.Map.GetPlayer(packet.Name);

            if (observer != null)
            {
                RuleViolation ruleViolation = Game.Current.RuleViolations.GetRuleViolation(observer);

                if (ruleViolation != null)
                {
                    if (ruleViolation.Assignee == null)
	                {
                        foreach (var observer2 in Game.Current.Chats.GetChat(3).GetPlayers() )
                        {
                            observer2.Client.Response.Write(new RemoveReportOutgoingPacket(ruleViolation.Reporter.Name) );
                        }

                        ruleViolation.Reporter.Client.Response.Write(new CloseReportOutgoingPacket() );

                        Game.Current.RuleViolations.RemoveRuleViolation(ruleViolation);
	                }
                    else
	                {
                        if (ruleViolation.Assignee == Player)
	                    {
                            ruleViolation.Reporter.Client.Response.Write(new CloseReportOutgoingPacket() ); 

                            Game.Current.RuleViolations.RemoveRuleViolation(ruleViolation);
	                    }
	                }
                }
            }   
        }
        
        private void CloseReportRuleViolationQuestion()
        {
            RuleViolation ruleViolation = Game.Current.RuleViolations.GetRuleViolation(Player);

            if (ruleViolation != null)
            {
                if (ruleViolation.Assignee == null)
                {
                    foreach (var observer in Game.Current.Chats.GetChat(3).GetPlayers() )
                    {
                        observer.Client.Response.Write(new RemoveReportOutgoingPacket(ruleViolation.Reporter.Name) );
                    }

                    Game.Current.RuleViolations.RemoveRuleViolation(ruleViolation);
                }
                else
                {
                    ruleViolation.Assignee.Client.Response.Write(new CancelReportOutgoingPacket(ruleViolation.Reporter.Name) );

                    Game.Current.RuleViolations.RemoveRuleViolation(ruleViolation);
                }
            }
        }
        
        private void CloseNpcsChannel()
        {
            //TODO
        }
        
        private void CombatControls(CombatControlsIncomingPacket packet)
        {
            FightMode = packet.FightMode;

            ChaseMode = packet.ChaseMode;

            SafeMode = packet.SafeMode;

            //TODO
        }
        
        private void Attack(AttackIncomingPacket packet)
        {
            //TODO
        }
        
        private void Follow(FollowIncomingPacket packet)
        {
            //TODO
        }
        
        private void InviteToParty(InviteToPartyIncomingPacket packet)
        {
            //TODO
        }
        
        private void JoinParty(JoinPartyIncomingPacket packet)
        {
            //TODO
        }
        
        private void RevokeParty(RevokePartyIncomingPacket packet)
        {
            //TODO
        }
        
        private void PassLeadershipTo(PassLeadershipToIncomingPacket packet)
        {
            //TODO
        }
        
        private void LeaveParty()
        {
            //TODO
        }
        
        private void SharedExperience(SharedExperienceIncomingPacket packet)
        {
            //TODO
        }
        
        private void OpenMyPrivateChannel()
        {
            PrivateChat privateChat = Game.Current.Chats.GetPrivateChat(Player);

            if (privateChat == null)
            {
                privateChat = Game.Current.Chats.AddChat(new PrivateChat()
                {
                    Owner = Player, 
                
                    Name = Player.Name
                } );

                privateChat.AddPlayer(Player);
            }

            Response.Write(new OpenMyPrivateChannelOutgoingPacket(privateChat.Id, privateChat.Name) );
        }
        
        private void InvitePlayer(InvitePlayerIncomingPacket packet)
        {
            Player observer = Game.Current.Map.GetPlayer(packet.Name);

            if (observer != null)
            {
                if (observer != Player)
                {
                    PrivateChat privateChat = Game.Current.Chats.GetPrivateChat(Player);

                    if (privateChat != null)
	                {
                        if ( !privateChat.ContainsInvitation(Player) )
                        {
                            if ( !privateChat.ContainsPlayer(Player) )
                            {
                                privateChat.AddInvitation(observer);
		    
                                Response.Write(new TextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );
            
                                observer.Client.Response.Write(new TextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " invites you to his private chat channel.") );
                            }
                        }
	                }
                }
            }
        }
        
        private void ExcludePlayer(ExcludePlayerIncomingPacket packet)
        {
            Player observer = Game.Current.Map.GetPlayer(packet.Name);

            if (observer != null)
            {
                if (observer != Player)
                {
                    PrivateChat privateChat = Game.Current.Chats.GetPrivateChat(Player);

                    if (privateChat != null)
                    {
                        if ( privateChat.ContainsInvitation(Player) )
                        {
                            privateChat.RemoveInvitation(observer);

                            Response.Write(new TextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );
                        }
                        else if ( privateChat.ContainsPlayer(Player) )
                        {
                            privateChat.RemovePlayer(Player);

                            Response.Write(new TextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );

                            observer.Client.Response.Write(new CloseChannelOutgoingPacket(privateChat.Id) );
                        }
                    }
                }
            }
        }
        
        private void Stop()
        {
            if (WalkSchedulerEvent != null)
            {
                if ( WalkSchedulerEvent.Cancel() )
                {
                    Response.Write(new StopWalkOutgoingPacket(Player.Direction) );
                }
            }
        }
        
        private void SelectOutfit()
        {
            List<SelectOutfit> outfits = new List<SelectOutfit>()
            {
                new SelectOutfit(128, "Citizen", Addons.None),

                new SelectOutfit(129, "Hunter", Addons.None),

                new SelectOutfit(130, "Mage", Addons.None),

                new SelectOutfit(131, "Knight", Addons.None)
            };

            Response.Write(new SelectOutfitOutgoingPacket(Player.Outfit, outfits) );
        }
        
        private void ChangeOutfit(ChangeOutfitIncomingPacket packet)
        {
            Player.ChangeOutfit(packet.Outfit);
        }
        
        private void AddVip(AddVipIncomingPacket packet)
        {
            //TODO
        }
        
        private void RemoveVip(RemoveVipIncomingPacket packet)
        {
            //TODO
        }
        
        private void ReportBug(ReportBugIncomingPacket packet)
        {
            //TODO
        }
        
        private void Quests()
        {
            //TODO

            List<Quest> quests = new List<Quest>();

            quests.Add(new Quest(1, "Quest 1", false) );

            quests.Add(new Quest(2, "Quest 2", true) );
            
            Response.Write(new QuestLogOutgoingPacket(quests) );
        }
        
        private void Quest(QuestIncomingPacket packet)
        {
            //TODO

            List<Mission> missions = new List<Mission>();

            missions.Add(new Mission("Mission 1", "Description 1") );

            missions.Add(new Mission("Mission 2", "Description 2") );

            Response.Write(new QuestLineOutgoingPacket(packet.QuestId, missions) );
        }
    }
}
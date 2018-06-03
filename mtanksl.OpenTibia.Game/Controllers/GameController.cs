using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Mvc;
using OpenTibia.Network.Packets.Incoming;

namespace OpenTibia.Game.Controllers
{
    [Port(7172)]
    public class GameController : Controller
    {
        private Server server;

        public GameController(Server server, IConnection connection) : base(connection)
        {
            this.server = server;
        }

        public IActionResult Command(Command command)
        {
            return new CommandResult(server, Context, command);
        }

        public IActionResult DelayedCommand(string key, int delay, Command command)
        {
            return new DelayedCommandResult(server, key, delay, Context, command);
        }

        private static int Sequence = 1;

        [Packet(0x0A)]
        public IActionResult SelectedCharacter(SelectedCharacter packet)
        {
            Context.Request.Connection.Keys = packet.Keys;

            if (packet.Version != 860)
            {
                Context.Response.Write(Context.Request.Connection, new Network.Packets.Outgoing.OpenSorryDialog(true, Constants.OnlyProtocol86Allowed) );

                Context.Response.Flush();

                return null;
            }

            var account = new Data.PlayerRepository().GetPlayer(packet.Account, packet.Password, packet.Character);

            if (account == null)
            {
                Context.Response.Write(Context.Request.Connection, new Network.Packets.Outgoing.OpenSorryDialog(true, Constants.AccountNameOrPasswordIsNotCorrect) );

                Context.Response.Flush();

                return null;
            }

            Context.Request.Connection.Client = new Client(server) { Player = new Player() { Name = account.Name + " " + Sequence++ } };

            return Command(new LogInCommand(server) { Player = Context.Request.Connection.Client.Player, Position = new Position(account.CoordinateX, account.CoordinateY, account.CoordinateZ) } );
        }

        [Packet(0x14)]
        public IActionResult Logout()
        {
            Context.Request.Connection.Disconnect();

            return Command(new LogOutCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0x1E)]
        public IActionResult Ping()
        {
            return Command(new PongCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0x64)]
        public IActionResult WalkTo(WalkTo packet)
        {
            return Command(new WalkToCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirections = packet.MoveDirections } );
        }

        [Packet(0x65)]
        public IActionResult WalkNorth()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.North } );
        }

        [Packet(0x66)]
        public IActionResult WalkEast()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.East } );
        }

        [Packet(0x67)]
        public IActionResult WalkSouth()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.South } );
        }

        [Packet(0x68)]
        public IActionResult WalkWest()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.West } );
        }

        [Packet(0x69)]
        public IActionResult StopWalk()
        {
            return Command(new StopWalkCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0x6A)]
        public IActionResult WalkNorthEast()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.NorthEast } );
        }

        [Packet(0x6B)]
        public IActionResult WalkSouthEast()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.SouthEast } );
        }

        [Packet(0x6C)]
        public IActionResult WalkSouthWest()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.SouthWest } );
        }

        [Packet(0x6D)]
        public IActionResult WalkNorthWest()
        {
            return Command(new WalkCommand(server) { Player = Context.Request.Connection.Client.Player, MoveDirection = MoveDirection.NorthWest } );
        }

        [Packet(0x6F)]
        public IActionResult TurnNorth()
        {
            return Command(new TurnCommand(server) { Player = Context.Request.Connection.Client.Player, Direction = Direction.North } );
        }

        [Packet(0x70)]
        public IActionResult TurnEast()
        {
            return Command(new TurnCommand(server) { Player = Context.Request.Connection.Client.Player, Direction = Direction.East } );
        }

        [Packet(0x71)]
        public IActionResult TurnSouth()
        {
            return Command(new TurnCommand(server) { Player = Context.Request.Connection.Client.Player, Direction = Direction.South } );
        }

        [Packet(0x72)]
        public IActionResult TurnWest()
        {
            return Command(new TurnCommand(server) { Player = Context.Request.Connection.Client.Player, Direction = Direction.West } );
        }

        [Packet(0x78)]
        public IActionResult MoveItem(MoveItem packet)
        {
            Position fromPosition = new Position(packet.FromX, packet.FromY, packet.FromZ);

            Position toPosition = new Position(packet.ToX, packet.ToY, packet.ToZ);

            if (fromPosition.IsContainer)
            {
                if (toPosition.IsContainer)
                {
                    //TODO
                }
                else if (toPosition.IsInventory)
                {
                    //TODO
                }
                else
                {
                    //TODO
                }
            }
            else if (fromPosition.IsInventory)
            {
                if (toPosition.IsContainer)
                {
                    //TODO
                }
                else if (toPosition.IsInventory)
                {
                    return Command(new MoveItemFromInventoryToInventoryCommand(server) { Player = Context.Request.Connection.Client.Player, FromSlot = fromPosition.InventoryIndex, ToSlot = toPosition.InventoryIndex } );
                }
                else
                {
                    return Command(new MoveItemFromInventoryToTileCommand(server) { Player = Context.Request.Connection.Client.Player, FromSlot = fromPosition.InventoryIndex, ToPosition = toPosition } );
                }
            }
            else
            {
                if (toPosition.IsContainer)
                {
                    //TODO
                }
                else if (toPosition.IsInventory)
                {
                    return Command(new MoveItemFromTileToInventoryCommand(server) { Player = Context.Request.Connection.Client.Player, FromPosition = fromPosition, FromIndex = packet.FromIndex, ToSlot = toPosition.InventoryIndex } );
                }
                else
                {
                    return Command(new MoveItemFromTileToTileCommand(server) { Player = Context.Request.Connection.Client.Player, FromPosition = fromPosition, FromIndex = packet.FromIndex, ToPosition = toPosition } );
                }
            }
            
            return null;
        }

        [Packet(0x79)]
        public IActionResult LookItemNpcTrade(LookItemNpcTrade packet)
        {
           //TODO

            return null;
        }

        [Packet(0x7A)]
        public IActionResult BuyNpcTrade(BuyNpcTrade packet)
        {
            //TODO

            return null;
        }

        [Packet(0x7B)]
        public IActionResult SellNpcTrade(SellNpcTrade packet)
        {
            //TODO

            return null;
        }

        [Packet(0x7C)]
        public IActionResult CloseNpcTrade()
        {
            //TODO

            return null;
        }

        [Packet(0x7D)]
        public IActionResult TradeWith(TradeWith packet)
        {
            //TODO

            return null;
        }

        [Packet(0x7E)]
        public IActionResult LookItemTrade(LookItemTrade packet)
        {
            //TODO

            return null;
        }

        [Packet(0x7F)]
        public IActionResult AcceptTrade(AcceptTrade packet)
        {
            //TODO

            return null;
        }

        [Packet(0x80)]
        public IActionResult CancelTrade(CancelTrade packet)
        {
            //TODO

            return null;
        }

        [Packet(0x82)]
        public IActionResult UseItem(UseItem packet)
        {
            Position fromPosition = new Position(packet.X, packet.Y, packet.Z);

            if (fromPosition.IsContainer)
            {
                //TODO
            }
            else if (fromPosition.IsInventory)
            {
                //TODO
            }
            else
            {
                return Command(new UseItemFromTileCommand(server) { Player = Context.Request.Connection.Client.Player, FromPosition = fromPosition, FromIndex = packet.Index } );
            }

            return null;
        }

        [Packet(0x83)]
        public IActionResult UseItemWithItem(UseItemWithItem packet)
        {
            //TODO

            return null;
        }
        
        [Packet(0x84)]
        public IActionResult UseItemWithCreature(UseItemWithCreature packet)
        {
            //TODO

            return null;
        }

        [Packet(0x85)]
        public IActionResult RotateItem(RotateItem packet)
        {
            //TODO

            return null;
        }

        [Packet(0x87)]
        public IActionResult CloseContainer(CloseContainer packet)
        {
            //TODO

            return null;
        }

        [Packet(0x88)]
        public IActionResult OpenParent(OpenParent packet)
        {
            //TODO

            return null;
        }

        [Packet(0x8C)]
        public IActionResult Look(Look packet)
        {
            //TODO

            return null;
        }

        [Packet(0x96)]
        public IActionResult Talk(Talk packet)
        {
            switch (packet.TalkType)
            {
                case TalkType.Say:

                    return Command(new SayCommand(server) { Player = Context.Request.Connection.Client.Player, Message = packet.Message } );

                case TalkType.Whisper:

                    return Command(new WhisperCommand(server) { Player = Context.Request.Connection.Client.Player, Message = packet.Message } );
                    
                case TalkType.Yell:

                    return Command(new YellCommand(server) { Player = Context.Request.Connection.Client.Player, Message = packet.Message } );
 
                case TalkType.Private:

                    return Command(new SendMessageToPlayerCommand(server) { Player = Context.Request.Connection.Client.Player, Name = packet.Name, Message = packet.Message } );

                case TalkType.ChannelYellow:
                          
                    return Command(new SendMessageToChannel(server) { Player = Context.Request.Connection.Client.Player, ChannelId = packet.ChannelId, Message = packet.Message } );
                                        
                case TalkType.ReportRuleViolationOpen:

                    return Command(new CreateReportRuleViolationCommand(server) { Player = Context.Request.Connection.Client.Player, Message = packet.Message } );

                case TalkType.ReportRuleViolationAnswer:

                    return Command(new AnswerInReportRuleViolationChannelCommand(server) { Player = Context.Request.Connection.Client.Player, Name = packet.Name, Message = packet.Message } );

                case TalkType.ReportRuleViolationQuestion:

                    return Command(new AskInReportRuleViolationChannelCommand(server) { Player = Context.Request.Connection.Client.Player, Message = packet.Message } );

                case TalkType.Broadcast:

                    return Command(new BroadcastMessageCommand(server) { Player = Context.Request.Connection.Client.Player, Message = packet.Message } );
            }

            return null;
        }

        [Packet(0x97)]
        public IActionResult OpenNewChannel()
        {
            return Command(new OpenNewChannelCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0x98)]
        public IActionResult OpenedNewChannel(OpenedNewChannel packet)
        {
            return Command(new OpenedNewChannelCommand(server) { Player = Context.Request.Connection.Client.Player, ChannelId = packet.ChannelId } );
        }

        [Packet(0x99)]
        public IActionResult CloseChannel(CloseChannel packet)
        {
            return Command(new CloseChannelCommand(server) { Player = Context.Request.Connection.Client.Player, ChannelId = packet.ChannelId } );
        }

        [Packet(0x9A)]
        public IActionResult OpenedPrivateChannel(OpenedPrivateChannel packet)
        {
            return Command(new OpenedPrivateChannelCommand(server) { Player = Context.Request.Connection.Client.Player, Name = packet.Name } );
        }

        [Packet(0x9B)]
        public IActionResult ProcessReportRuleViolation(ProcessReportRuleViolation packet)
        {
            return Command(new ProcessReportRuleViolationCommand(server) { Player = Context.Request.Connection.Client.Player, Name = packet.Name } );
        }

        [Packet(0x9C)]
        public IActionResult CloseReportRuleViolationChannelAnswer(CloseReportRuleViolationChannelAnswer packet)
        {
            return Command(new CloseReportRuleViolationChannelAnswerCommand(server) { Player = Context.Request.Connection.Client.Player, Name = packet.Name } );
        }

        [Packet(0x9D)]
        public IActionResult CloseReportRuleViolationChannelQuestion()
        {
            return Command(new CloseReportRuleViolationChannelQuestionCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0x9E)]
        public IActionResult CloseNpcsChannel()
        {
            //TODO

            return null;
        }

        [Packet(0xA0)]
        public IActionResult CombatControls(CombatControls packet)
        {
            return Command(new CombatControlsCommand(server) { Player = Context.Request.Connection.Client.Player, FightMode = packet.FightMode, ChaseMode = packet.ChaseMode, SafeMode = packet.SafeMode } );
        }

        [Packet(0xA1)]
        public IActionResult Attack(Attack packet)
        {
            //TODO

            return null;
        }

        [Packet(0xA2)]
        public IActionResult Follow(Follow packet)
        {
            //TODO

            return null;
        }

        [Packet(0xA3)]
        public IActionResult InviteToParty(InviteToParty packet)
        {
            //TODO

            return null;
        }

        [Packet(0xA4)]
        public IActionResult JoinParty(JoinParty packet)
        {
            //TODO

            return null;
        }

        [Packet(0xA5)]
        public IActionResult RevokeParty(RevokeParty packet)
        {
            //TODO

            return null;
        }

        [Packet(0xA6)]
        public IActionResult PassLeadershipTo(PassLeadershipTo packet)
        {
            //TODO

            return null;
        }

        [Packet(0xA7)]
        public IActionResult LeaveParty()
        {
            //TODO

            return null;
        }

        [Packet(0xA8)]
        public IActionResult SharedExperience(SharedExperience packet)
        {
            //TODO

            return null;
        }

        [Packet(0xAA)]
        public IActionResult OpenedMyPrivateChannel()
        {
            return Command(new OpenedMyPrivateChannelCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0xAB)]
        public IActionResult InvitePlayer(InvitePlayer packet)
        {
            return Command(new InvitePlayerCommand(server) { Player = Context.Request.Connection.Client.Player, Name = packet.Name } );
        }

        [Packet(0xAC)]
        public IActionResult ExcludePlayer(ExcludePlayer packet)
        {
            return Command(new ExcludePlayerCommand(server) { Player = Context.Request.Connection.Client.Player, Name = packet.Name } );
        }

        [Packet(0xBE)]
        public IActionResult Stop()
        {
            return Command(new StopWalkCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0xD2)]
        public IActionResult SelectOutfit()
        {
            return Command(new SelectOutfitCommand(server) { Player = Context.Request.Connection.Client.Player } );
        }

        [Packet(0xD3)]
        public IActionResult SelectedOutfit(SelectedOutfit packet)
        {
            return Command(new SelectedOutfitCommand(server) { Player = Context.Request.Connection.Client.Player, Outfit = packet.Outfit } );
        }

        [Packet(0xDC)]
        public IActionResult AddVip(AddVip packet)
        {
            //TODO

            return null;
        }

        [Packet(0xDD)]
        public IActionResult RemoveVip(RemoveVip packet)
        {
            //TODO

            return null;
        }

        [Packet(0xE6)]
        public IActionResult ReportBug(ReportBug packet)
        {
            //TODO

            return null;
        }

        [Packet(0xF0)]
        public IActionResult Quests()
        {
            //TODO

            return null;
        }

        [Packet(0xF1)]
        public IActionResult OpenQuest(OpenQuest packet)
        {
            //TODO

            return null;
        }
    }
}
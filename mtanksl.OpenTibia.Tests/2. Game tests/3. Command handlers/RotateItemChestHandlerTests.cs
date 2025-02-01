using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Common.Structures;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Tests
{
    [TestClass]
    [TestCategory("2.3. Command handlers")]
    public class RotateItemChestHandlerTests
    {
        [TestMethod]
        public void CantRotateQuestChest()
        {
            Test.Run(t =>
            {
                var player = t.Login("1", "1", "Gamemaster");

                t.Move(player, new Position(860, 730, 7) );

                var item = t.Server.Map.GetTile(new Position(860, 730, 7) ).TopItem;

                t.Using(player, a => a
                    .Execute(new PlayerRotateItemCommand(a.Connection.Client.Player, item), new RotateItemChestHandler() )
                    .ExpectSuccess(false)
                    .Observe(o => o
                        .ExpectPacket(1)
                        .ExpectPacket<ShowWindowTextOutgoingPacket>(1, o => o.Message == Constants.YouCanNotUseThisObject) ) )
                .Run();
            } );
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Tests
{
    [TestClass]
    [TestCategory("2.4. Event handlers")]
    public class AccountManagerLoginHandlerTests
    {
        [TestMethod]
        public void Success()
        {
            Test.Run(t =>
            {
                var player = t.Login("", "", "Account Manager");

                t.Using(player, a => a
                    .Execute(new PlayerLoginEventArgs(a.Connection.Client.Player), new AccountManagerLoginHandler() )
                    .ExpectSuccess()
                    .Observe(o => o
                        .ExpectPacket(1)
                        .ExpectPacket<ShowWindowTextOutgoingPacket>(1) ) )
                .Run();
            } );
        }
    }
}
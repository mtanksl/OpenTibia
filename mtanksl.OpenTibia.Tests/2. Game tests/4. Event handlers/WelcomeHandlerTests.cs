using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Tests
{
    [TestClass]
    [TestCategory("2.4. Event handlers")]
    public class WelcomeHandlerTests
    {
        [TestMethod]
        public void ShowMessageOnLogin()
        {
            Test.Run(t =>
            {
                var player = t.Login("1", "1", "Gamemaster");

                t.Using(player, a => a
                    .Execute(new PlayerLoginEventArgs(a.Connection.Client.Player), new WelcomeHandler() )
                    .ExpectSuccess()
                    .Observe(o => o
                        .ExpectPacket(1)
                        .ExpectPacket<ShowWindowTextOutgoingPacket>(1) ) )
                .Run();
            } );
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Tests
{
    [TestClass]
    [TestCategory("2.2. Outgoing commands")]
    public class ShowAnimatedTextCommandTests
    {
        [TestMethod]
        public void CanSee()
        {
            Test.Run(t =>
            {
                var player = t.Login("1", "1", "Gamemaster");

                t.Move(player, new Position(921, 771, 6) );

                t.Using(player, a => a
                    .Execute(new ShowAnimatedTextCommand(new Position(921, 771, 6), AnimatedTextColor.White, 123456) )
                    .ExpectSuccess()
                    .Observe(o => o
                        .ExpectPacket(1)
                        .ExpectPacket<ShowAnimatedTextOutgoingPacket>(1) ) )
                .Run();
            } );            
        }

        [TestMethod]
        public void CantSee()
        {
            Test.Run(t =>
            {
                var player = t.Login("1", "1", "Gamemaster");

                t.Move(player, new Position(921, 771, 6) );

                t.Using(player, a => a
                    .Execute(new ShowAnimatedTextCommand(new Position(0, 0, 0), AnimatedTextColor.White, 123456) )
                    .ExpectSuccess()
                    .Observe(o => o
                        .ExpectPacket(0) ) )
                .Run();
            } );
        }
    }
}
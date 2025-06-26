using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Tests
{
    [TestClass]
    [TestCategory("2.2. Outgoing commands")]
    public class ShowTextCommandTests
    {
        [TestMethod]
        public void CanSee()
        {
            Test.Run(t =>
            {
                var player = t.Login("1", "1", "Gamemaster");

                t.Move(player, new Position(921, 771, 6) );

                t.Using(player, a => a
                    .Execute(new ShowTextCommand(a.Connection.Client.Player, MessageMode.MonsterSay, "ABCDEFGH") )
                    .ExpectSuccess()
                    .Observe(o => o
                        .ExpectPacket(1)
                        .ExpectPacket<ShowTextOutgoingPacket>(1) ) )
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

                var npc = t.Server.GameObjects.GetNpcs().Where(n => n.Name == "Cipfried").FirstOrDefault();

                t.Using(player, a => a
                    .Execute(new ShowTextCommand(npc, MessageMode.MonsterSay, "ABCDEFGH") )
                    .ExpectSuccess()
                    .Observe(o => o
                        .ExpectPacket(0) ) )
                .Run();
            } );
        }
    }
}
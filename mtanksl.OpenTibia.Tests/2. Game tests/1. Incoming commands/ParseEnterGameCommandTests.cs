using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Incoming;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Tests
{
    [TestClass]
    [TestCategory("2.1. Incoming commands")]
    public class ParseEnterGameCommandTests
    {
        private EnterGameIncomingPacket CreatePacket()
        {
            return new EnterGameIncomingPacket()
            {
                OperatingSystem = OperatingSystem.Windows,

                Version = 860,

                TibiaDat = 1277983123,
                
                TibiaPic = 1256571859,
                
                TibiaSpr = 1277298068,

                Keys = new uint[] { 0, 0, 0, 0 },

                Account = "1",

                Password = "1"
            };
        }

        [TestMethod]
        public void CantLoginWithOtherVersion()
        {
            Test.Run(t =>
            {
                t.Using("127.0.0.1", a =>
                {
                    var packet = CreatePacket();

                    packet.Version = 772;

                    a.Execute(new ParseEnterGameCommand(a.Connection, packet) )
                     .ExpectSuccess(false)
                     .Observe(o => o
                         .ExpectPacket(1)
                         .ExpectPacket<OpenSorryDialogOutgoingPacket>(1, o => o.Message == string.Format(Constants.OnlyProtocolAllowed, t.Server.Config.ClientVersion) )
                         .ExpectConnected(false) );
                } )
                .Run();
            } );
        }

        [TestMethod]
        public void CanLoginWhileDownForMaintenance()
        {
            Test.Run(t =>
            {
                t.Using("127.0.0.1", a =>
                {
                    var packet = CreatePacket();

                    a.Execute(new ParseEnterGameCommand(a.Connection, packet) )
                     .ExpectSuccess()
                     .Observe(o => o
                         .ExpectPacket(2)
                         .ExpectPacket<OpenMessageOfTheDayDialogOutgoingPacket>(1)
                         .ExpectPacket<OpenSelectCharacterDialogOutgoingPacket>(1)
                         .ExpectConnected(false) );
                } )
                .Run();
            }, t =>
            {
                t.Server.Pause();
            }, t =>
            {
                t.Server.Continue();
            } );
        }

        [TestMethod]
        public void CantLoginWhileDownForMaintenance()
        {
            Test.Run(t =>
            {
                t.Using("192.168.0.1", a =>
                {
                    var packet = CreatePacket();

                    a.Execute(new ParseEnterGameCommand(a.Connection, packet) )
                     .ExpectSuccess(false)
                     .Observe(o => o
                         .ExpectPacket(1)
                         .ExpectPacket<OpenSorryDialogOutgoingPacket>(1, o => o.Message == Constants.TibiaIsCurrentlyDownForMaintenance)
                         .ExpectConnected(false) );
                } )
                .Run();
            }, t =>
            {
                t.Server.Pause();
            }, t =>
            {
                t.Server.Continue();
            } );
        }

        [TestMethod]
        public void CantLoginWithWrongAccountNameOrPassword()
        {
            Test.Run(t =>
            {
                t.Using("127.0.0.1", a =>
                {
                    var packet = CreatePacket();

                    packet.Password = "";

                    a.Execute(new ParseEnterGameCommand(a.Connection, packet) )
                     .ExpectSuccess(false)
                     .Observe(o => o
                         .ExpectPacket(1)
                         .ExpectPacket<OpenSorryDialogOutgoingPacket>(1, o => o.Message == Constants.AccountNameOrPasswordIsNotCorrect)
                         .ExpectConnected(false) );
                } )
                .Run();
            } );
        }

        [TestMethod]
        public void Success()
        {
            Test.Run(t =>
            {
                t.Using("127.0.0.1", a =>
                {              
                    var packet = CreatePacket();

                    a.Execute(new ParseEnterGameCommand(a.Connection, packet) )
                     .ExpectSuccess()
                     .Observe(o => o
                         .ExpectPacket(2)
                         .ExpectPacket<OpenMessageOfTheDayDialogOutgoingPacket>(1)
                         .ExpectPacket<OpenSelectCharacterDialogOutgoingPacket>(1)
                         .ExpectConnected(false) );
                } )
                .Run();
            } );
        }
    }
}
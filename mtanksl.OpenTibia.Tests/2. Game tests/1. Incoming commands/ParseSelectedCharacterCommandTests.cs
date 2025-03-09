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
    public class ParseSelectedCharacterCommandTests
    {        
        private SelectedCharacterIncomingPacket CreatePacket()
        {
            return new SelectedCharacterIncomingPacket()
            {
                OperatingSystem = OperatingSystem.Windows,

                Version = 860,

                Keys = new uint[] { 0, 0, 0, 0 },

                Gamemaster = true,

                Account = "1",

                Password = "1",

                Character = "Gamemaster",

                Timestamp = 0,
                
                Random = 0
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

                    a.Execute(new ParseSelectedCharacterCommand(a.Connection, packet) )
                     .ExpectSuccess(false)
                     .Observe(o => o
                         .ExpectPacket(1)
                         .ExpectPacket<OpenSorryDialogOutgoingPacket>(1, o => o.Message == Constants.OnlyProtocol86Allowed)
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

                    a.Execute(new ParseSelectedCharacterCommand(a.Connection, packet) )
                     .ExpectSuccess()
                     .Observe(o => o
                         .ExpectPacket(9)
                         .ExpectPacket<SendInfoOutgoingPacket>(1)
                         .ExpectPacket<SendTilesOutgoingPacket>(1)
                         .ExpectPacket<SetEnvironmentLightOutgoingPacket>(1)
                         .ExpectPacket<SendStatusOutgoingPacket>(1)
                         .ExpectPacket<SendSkillsOutgoingPacket>(1)
                         .ExpectPacket<SetSpecialConditionOutgoingPacket>(1)
                         .ExpectPacket<SlotAddOutgoingPacket>(1)
                         .ExpectPacket<ShowMagicEffectOutgoingPacket>(1)
                         .ExpectPacket<ShowWindowTextOutgoingPacket>(1)
                         .ExpectConnected() );
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

                    a.Execute(new ParseSelectedCharacterCommand(a.Connection, packet) )
                     .ExpectSuccess(false)
                     .Observe(o => o
                         .ExpectPacket(1)
                         .ExpectPacket<OpenSorryDialogOutgoingPacket>(1, o => o.Message == Constants.TibiaIsCurrentlyDownForMaintenance)
                         .ExpectConnected(false) ) ;
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

                    a.Execute(new ParseSelectedCharacterCommand(a.Connection, packet) )
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
        public void CantLoginWithWrongCharacter()
        {
            Test.Run(t =>
            {
                t.Using("127.0.0.1", a =>
                {
                    var packet = CreatePacket();

                    packet.Character = "";

                    a.Execute(new ParseSelectedCharacterCommand(a.Connection, packet) )
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

                    a.Execute(new ParseSelectedCharacterCommand(a.Connection, packet) )
                     .ExpectSuccess()
                     .Observe(o => o
                         .ExpectPacket(9)
                         .ExpectPacket<SendInfoOutgoingPacket>(1)
                         .ExpectPacket<SendTilesOutgoingPacket>(1)
                         .ExpectPacket<SetEnvironmentLightOutgoingPacket>(1)
                         .ExpectPacket<SendStatusOutgoingPacket>(1)
                         .ExpectPacket<SendSkillsOutgoingPacket>(1)
                         .ExpectPacket<SetSpecialConditionOutgoingPacket>(1)
                         .ExpectPacket<SlotAddOutgoingPacket>(1)
                         .ExpectPacket<ShowMagicEffectOutgoingPacket>(1)
                         .ExpectPacket<ShowWindowTextOutgoingPacket>(1)
                         .ExpectConnected() );
                } )
                .Run();
            } );
        }
    }
}

//TODO: More tests
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Incoming;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseInfoProtocolCommand : IncomingCommand
    {
        public ParseInfoProtocolCommand(IConnection connection, InfoIncomingPacket packet)
        {
            Connection = connection;

            Packet = packet;
        }

        public IConnection Connection { get; set; }

        public InfoIncomingPacket Packet { get; set; }

        public override Promise Execute()
        {
            if (Packet.Xml)
            {
                if (Packet.RequestedInfo != RequestedInfo.None)
                {
                    XElement xml = new XElement("tsqp", 
                        new XAttribute("version", "1.0"), 
                        new XElement("serverinfo",
                            new XAttribute("uptime", (uint)Context.Server.Uptime.TotalSeconds),
                            new XAttribute("ip", Context.Server.Config.InfoIPAddress),
                            new XAttribute("servername", Context.Server.Config.InfoServerName),
                            new XAttribute("port", Context.Server.Config.LoginPort),
                            new XAttribute("location", Context.Server.Config.InfoLocation),
                            new XAttribute("url", Context.Server.Config.InfoUrl),
                            new XAttribute("server", Context.Server.ServerName),
                            new XAttribute("version", Context.Server.ServerVersion),
                            new XAttribute("client", Context.Server.ClientVersion) ),
                        new XElement("owner",
                            new XAttribute("name", Context.Server.Config.InfoOwnerName),
                            new XAttribute("email", Context.Server.Config.InfoOwnerEmail) ),
                        new XElement("players",
                            new XAttribute("online", Context.Server.GameObjects.GetPlayers().Count() ),
                            new XAttribute("max", Context.Server.Config.GameplayMaxPlayers),
                            new XAttribute("peek", 0) ),
                        new XElement("monsters",
                            new XAttribute("total", Context.Server.GameObjects.GetMonsters().Count() ) ),
                        new XElement("npcs",
                            new XAttribute("total", Context.Server.GameObjects.GetNpcs().Count() ) ),
                        new XElement("rates",
                            new XAttribute("experience", Context.Server.Config.GameplayExperienceRate),
                            new XAttribute("skill", 1),
                            new XAttribute("loot", Context.Server.Config.GameplayLootRate),
                            new XAttribute("magic", 1),
                            new XAttribute("spawn", 1) ),
                        new XElement("map",
                            new XAttribute("name", Context.Server.Config.InfoMapName),
                            new XAttribute("author", Context.Server.Config.InfoMapAuthor),
                            new XAttribute("width", Context.Server.Map.Width),
                            new XAttribute("height", Context.Server.Map.Height) ),
                        new XElement("motd", Context.Server.Config.Motd) );

                    Context.AddPacket(Connection, new XmlInfoOutgoingPacket(xml) );
                }
            }
            else
            {
                if (Packet.RequestedInfo.Is(RequestedInfo.BasicInfo) )
                {
                    Context.AddPacket(Connection, new BasicInfoOutgoingPacket(Context.Server.Config.InfoServerName, Context.Server.Config.InfoIPAddress, Context.Server.Config.LoginPort) );
                }

                if (Packet.RequestedInfo.Is(RequestedInfo.OwnerInfo) )
                {
                    Context.AddPacket(Connection, new OwnerInfoOutgoingPacket(Context.Server.Config.InfoOwnerName, Context.Server.Config.InfoOwnerEmail) );
                }

                if (Packet.RequestedInfo.Is(RequestedInfo.MiscInfo) )
                {
                    Context.AddPacket(Connection, new MiscInfoOutgoingPacket(Context.Server.Config.Motd, Context.Server.Config.InfoLocation, Context.Server.Config.InfoUrl, (uint)Context.Server.Uptime.TotalSeconds) );
                }

                if (Packet.RequestedInfo.Is(RequestedInfo.PlayersInfo) )
                {
                    Context.AddPacket(Connection, new PlayersInfoOutgoingPacket( (uint)Context.Server.GameObjects.GetPlayers().Count(), (uint)Context.Server.Config.GameplayMaxPlayers, 0) );
                }

                if (Packet.RequestedInfo.Is(RequestedInfo.MapInfo) )
                {
                    Context.AddPacket(Connection, new MapInfoOutgoingPacket(Context.Server.Config.InfoMapName, Context.Server.Config.InfoMapAuthor, Context.Server.Map.Width, Context.Server.Map.Height) );
                }

                if (Packet.RequestedInfo.Is(RequestedInfo.ExtPlayersInfo) )
                {
                    List<ExtPlayersInfoDto> players = new List<ExtPlayersInfoDto>();

                    foreach (var player in Context.Server.GameObjectPool.GetPlayers() )
                    {
                        players.Add(new ExtPlayersInfoDto(player.Name, player.Level) );
                    }

                    Context.AddPacket(Connection, new ExtPlayersInfoOutgoingPacket(players) );
                }

                if (Packet.RequestedInfo.Is(RequestedInfo.PlayerStatusInfo) )
                {
                    Player player = Context.Server.GameObjects.GetPlayerByName(Packet.PlayerName);

                    Context.AddPacket(Connection, new PlayersStatusInfoOutgoingPacket(player != null) );
                }

                if (Packet.RequestedInfo.Is(RequestedInfo.SoftwareInfo) )
                {
                    Context.AddPacket(Connection, new SoftwareInfoOutgoingPacket(Context.Server.ServerName, Context.Server.ServerVersion, Context.Server.ClientVersion) );
                }
            }

            Context.Disconnect(Connection);

            return Promise.Completed;
        }
    }
}
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Incoming
{
    public class ReportRuleViolationIncomingPacket : IIncomingPacket
    {
        public byte Type { get; set; }

        public byte RuleViolation { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string Translation { get; set; }

        public uint StatmentId { get; set; }

        public void Read(ByteArrayStreamReader reader)
        {
            /*            
                0 = Name Report
                1 = Statement Report
                2 = Bot/Macro Report
            */

            Type = reader.ReadByte();

            /*             
                Name Related

                0 = "insulting"
                    "racist"
                    "sexually related"
                    "drug-related"
                    "generally objectionable"

                1 = "parts of sentence"
                    "badly formatted words"
                    "nonsensical combination of letters"

                2 = "religious or political view",
                    "illegal advertising",
                    "does not fit into Tibia's fantasy setting"

                3 = "implies or incites a violation of the Tibia Rules"

                Statement/Bot Related

                4 = "insulting"
                    "racist"
                    "sexually related"
                    "drug-related"
                    "harassing"
                    "generally objectionable"

                5 = "repeating identical or similar statements within a short period of time"
                    "using badly formatted or nonsensical text"

                6 = "advertising non-game related content"
                    "offering Tibia items for real Money"

                7 = "religious, political or other not topic-related public statements"

                8 = "non-English public statements where not explicitly allowed"

                9 = "statements that imply or incite a violation of the Tibia Rules"

                10 = "bug abuse"

                11 = "game weakness abuse"

                12 = "using unofficial software to play"

                13 = "hacking"

                14 = "multi-clienting"

                15 = "account trading or sharing"

                16 = "threatening a gamemaster because of his or her actions or position"

                17 = "pretending to be a gamemaster or to have influence on a gamemaster"
            */

            RuleViolation = reader.ReadByte();

            Name = reader.ReadString();

            Comment = reader.ReadString();

            if (Type == 0x00 || Type == 0x01)
            {
                Translation = reader.ReadString();

                if (Type == 0x01)
                {
                    StatmentId = reader.ReadUInt();
                }
            }
        }
    }
}
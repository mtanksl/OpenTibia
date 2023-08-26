using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Xml.Quests;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenQuestCommand : Command
    {
        public ParseOpenQuestCommand(Player player, ushort questId)
        {
            Player = player;

            QuestId = questId;
        }

        public Player Player { get; set; }

        public ushort QuestId { get; set; }

        public override Promise Execute()
        {
            Quest quest = Context.Server.Quests.GetQuestById(QuestId);

            if (quest != null)
            {
                List<MissionDto> missions = new List<MissionDto>();

                foreach (var mission in quest.Missions)
                {
                    int value;

                    if (Player.Client.Storages.TryGetValue(mission.StorageKey, out value) )
                    {
                        if (mission.StorageValue == value)
                        {
                            missions.Add(new MissionDto(mission.Name, mission.Description) );
                        }
                    }
                }

                Context.AddPacket(Player.Client.Connection, new OpenQuestLineDialogOutgoingPacket(QuestId, missions) );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}
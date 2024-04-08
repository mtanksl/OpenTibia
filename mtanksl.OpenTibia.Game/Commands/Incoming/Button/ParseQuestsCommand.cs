using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ParseQuestsCommand : IncomingCommand
    {
        public ParseQuestsCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }
                
        public override Promise Execute()
        {
            List<QuestDto> quests = new List<QuestDto>();

            foreach (var quest in Context.Server.Quests.GetQuests() )
            {
                int missions = 0;
                
                int completed = 0;

                foreach (var mission in quest.Missions)
                {
                    int value;

                    if (Player.Client.Storages.TryGetValue(mission.StorageKey, out value) )
                    {
                        missions++;

                        if (mission.StorageValue != value)
                        {
                            completed++;
                        }
                    }
                }

                if (missions > 0)
                {
                    quests.Add(new QuestDto(quest.Id, quest.Name, missions == completed) );
                }
            }

            Context.AddPacket(Player, new OpenQuestLogDialogOutgoingPacket(quests) );

            return Promise.Completed;
        }
    }
}
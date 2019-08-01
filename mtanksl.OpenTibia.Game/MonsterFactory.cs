using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.Components;
using System.Collections.Generic;
using Monster = OpenTibia.Common.Objects.Monster;

namespace OpenTibia.Game
{
    public class MonsterFactory
    {
        private Dictionary<string, MonsterMetadata> metadatas;

        public MonsterFactory(MonsterFile monsterFile)
        {
            metadatas = new Dictionary<string, MonsterMetadata>(monsterFile.Monsters.Count);

            foreach (var xmlMonster in monsterFile.Monsters)
            {
                metadatas.Add(xmlMonster.Name, new MonsterMetadata()
                {
                    Name = xmlMonster.Name,

                    Health = xmlMonster.Health,

                    MaxHealth = xmlMonster.MaxHealth,

                    Outfit = xmlMonster.Outfit,

                    Speed = xmlMonster.Speed
                } );
            }
        }

        public Monster Create(string name)
        {
            MonsterMetadata metadata;

            if ( !metadatas.TryGetValue(name, out metadata) )
            {
                return null;                
            }

            Monster monster = new Monster(metadata);

            monster.AddComponent<WalkBehaviour>();

            return monster;
        }
    }
}
using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class MonsterFactory
    {
        private Dictionary<string, MonsterMetadata> metadatas;

        public MonsterFactory(OpenTibia.FileFormats.Xml.Monsters.MonsterFile monsterFile)
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

                    Speed = xmlMonster.Speed,

                    //TODO: Set other properties
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

            return new Monster(metadata);
        }
    }
}
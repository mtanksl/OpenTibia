using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.Components;
using System.Collections.Generic;
using System.Linq;
using Monster = OpenTibia.Common.Objects.Monster;

namespace OpenTibia.Game
{
    public class MonsterFactory
    {
        private Server server;

        public MonsterFactory(Server server, MonsterFile monsterFile)
        {
            this.server = server;

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

                    Sentences = xmlMonster.Voices?.Select(v => v.Sentence).ToArray()
                } );
            }
        }

        private Dictionary<string, MonsterMetadata> metadatas;

        public Monster Create(string name)
        {
            MonsterMetadata metadata;

            if ( !metadatas.TryGetValue(name, out metadata) )
            {
                return null;                
            }

            Monster monster = new Monster(metadata);
                        
            server.GameObjects.AddGameObject(monster);

            if (monster.Name == "Amazon" || monster.Name == "Valkyrie")
            {
                server.Components.AddComponent(monster, new AttackBehaviour() );
            }

            server.Components.AddComponent(monster, new AutoWalkBehaviour() );

            if (monster.Metadata.Sentences != null)
            {
                server.Components.AddComponent(monster, new AutoTalkBehaviour(monster.Metadata.Sentences) );
            }

            return monster;
        }

        public void Destroy(Monster monster)
        {
            server.GameObjects.RemoveGameObject(monster);

            server.Components.ClearComponents(monster);
        }
    }
}
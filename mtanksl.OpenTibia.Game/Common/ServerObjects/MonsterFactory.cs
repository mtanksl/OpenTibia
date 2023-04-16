using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.Components;
using OpenTibia.Game.Strategies;
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

                    Speed = (ushort)xmlMonster.Speed,

                    Description = xmlMonster.NameDescription,

                    Health = (ushort)xmlMonster.Health.Now,

                    MaxHealth = (ushort)xmlMonster.Health.Max,

                    Outfit = xmlMonster.Look.TypeEx != 0 ? new Outfit(xmlMonster.Look.TypeEx) : new Outfit(xmlMonster.Look.Type, xmlMonster.Look.Head, xmlMonster.Look.Body, xmlMonster.Look.Legs, xmlMonster.Look.Feet, Addon.None),

                    Corpse = (ushort)xmlMonster.Look.Corpse,

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

            if (monster.Name == "Amazon")
            {
                server.Components.AddComponent(monster, new CreatureAttackBehaviour(new DistantAttackStrategy(ProjectileType.ThrowingKnife, 2000, (attacker, target) => -server.Randomization.Take(0, 30) ) ) );

                server.Components.AddComponent(monster, new CreatureWalkBehaviour(new KeepDistanceWalkStrategy(3) ) );
            }
            else if (monster.Name == "Valkyrie")
            {
                server.Components.AddComponent(monster, new CreatureAttackBehaviour(new DistantAttackStrategy(ProjectileType.Spear, 2000, (attacker, target) => -server.Randomization.Take(0, 45) ) ) );

                server.Components.AddComponent(monster, new CreatureWalkBehaviour(new KeepDistanceWalkStrategy(3) ) );
            }
            else if (monster.Name == "Deer")
            {
                server.Components.AddComponent(monster, new CreatureWalkBehaviour(new RunAwayWalkStrategy() ) );
            }
            else if (monster.Name == "Dog")
            {
                server.Components.AddComponent(monster, new CreatureWalkBehaviour(new ApproachWalkStrategy() ) );
            }
            else
            {
                server.Components.AddComponent(monster, new CreatureAttackBehaviour(new CloseAttackStrategy(2000, (attacker, target) => -server.Randomization.Take(0, 20) ) ) );

                server.Components.AddComponent(monster, new CreatureWalkBehaviour(new FollowWalkStrategy() ) );
            }

            if (monster.Metadata.Sentences != null)
            {
                server.Components.AddComponent(monster, new CreatureTalkBehaviour(monster.Metadata.Sentences) );
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
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.Components;
using System;
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

            List<CreatureAction> actions = new List<CreatureAction>();

            if (monster.Name == "Amazon")
            {
                actions.Add(new AttackCreatureAction(new DistanceAttackStrategy(ProjectileType.ThrowingKnife, 0, 20), TimeSpan.FromSeconds(2) ) );

                actions.Add(new WalkCreatureAction(new KeepDistanceWalkStrategy(3) ) );
            }
            else if (monster.Name == "Valkyrie")
            {
                actions.Add(new AttackCreatureAction(new DistanceAttackStrategy(ProjectileType.Spear, 0, 30), TimeSpan.FromSeconds(2) ) );

                actions.Add(new WalkCreatureAction(new KeepDistanceWalkStrategy(3) ) );
            }
            else if (monster.Name == "Deer")
            {
                actions.Add(new WalkCreatureAction(new RunAwayWalkStrategy() ) );
            }
            else if (monster.Name == "Dog")
            {
                actions.Add(new WalkCreatureAction(new ApproachWalkStrategy() ) );
            }
            else
            {
                actions.Add(new AttackCreatureAction(new MeleeAttackStrategy(0, 20), TimeSpan.FromSeconds(2) ) );

                actions.Add(new WalkCreatureAction(new FollowWalkStrategy() ) );
            }

            if (monster.Metadata.Sentences != null)
            {
                actions.Add(new TalkCreatureAction(monster.Metadata.Sentences) );
            }

            if (actions.Count > 0)
            {
                server.Components.AddComponent(monster, new CreatureThinkBehaviour(new RandomChooseTargetStrategy(), actions.ToArray() ) );
            }

            return monster;
        }

        public bool Detach(Monster monster)
        {
            return server.GameObjects.RemoveGameObject(monster);
        }

        public void Destroy(Monster monster)
        {
            server.Components.ClearComponents(monster);
        }
    }
}
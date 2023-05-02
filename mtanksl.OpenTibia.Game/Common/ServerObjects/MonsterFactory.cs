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

            List<TargetAction> targetActions = new List<TargetAction>();

            List<NonTargetAction> nonTargetActions = new List<NonTargetAction>();

            if (monster.Name == "Amazon")
            {
                targetActions.Add(new KeepDistanceWalkTargetAction(3) );

                targetActions.Add(new DistanceAttackTargetAction(ProjectileType.ThrowingKnife, 0, 20, TimeSpan.FromSeconds(2) ) );
            }
            else if (monster.Name == "Valkyrie")
            {
                targetActions.Add(new KeepDistanceWalkTargetAction(3) );

                targetActions.Add(new DistanceAttackTargetAction(ProjectileType.Spear, 0, 30, TimeSpan.FromSeconds(2) ) );
            }
            else if (monster.Name == "Deer")
            {
                targetActions.Add(new RunAwayWalkTargetAction() );
            }
            else if (monster.Name == "Dog")
            {
                targetActions.Add(new ApproachWalkTargetAction() );
            }
            else
            {
                targetActions.Add(new FollowWalkTargetAction() );

                targetActions.Add(new MeleeAttackTargetAction(0, 20, TimeSpan.FromSeconds(2) ) );
            }

            if (monster.Metadata.Sentences != null)
            {
                nonTargetActions.Add(new MonsterSayNonTargetAction(monster.Metadata.Sentences, TimeSpan.FromSeconds(30) ) );
            }

            server.GameObjectComponents.AddComponent(monster, new MonsterThinkBehaviour(targetActions.ToArray(), nonTargetActions.ToArray() ) );

            return monster;
        }

        public bool Detach(Monster monster)
        {
            return server.GameObjects.RemoveGameObject(monster);
        }

        public void Destroy(Monster monster)
        {
            server.GameObjectComponents.ClearComponents(monster);

            server.GameObjectEventHandlers.ClearEventHandlers(monster);
        }
    }
}
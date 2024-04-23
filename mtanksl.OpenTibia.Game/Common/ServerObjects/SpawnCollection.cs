using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Spawns;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;
using System.Collections.Generic;
using Monster = OpenTibia.Common.Objects.Monster;
using Npc = OpenTibia.Common.Objects.Npc;
using Tile = OpenTibia.Common.Objects.Tile;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class SpawnCollection : ISpawnCollection
    {
        private IServer server;

        public SpawnCollection(IServer server)
        {
            this.server = server;
        }

        private Guid globalSpawnEventArgs;

        public void Start(SpawnFile spawnFile)
        {
            foreach (var xmlSpawn in spawnFile.Spawns)
            {
                foreach (var xmlMonster in xmlSpawn.Monsters)
                {
                    Tile tile = server.Map.GetTile(xmlMonster.Position);

                    if (tile != null)
                    {
                        Monster monster = server.MonsterFactory.Create(xmlMonster.Name, tile);

                        if (monster != null)
                        {
                            server.MonsterFactory.Attach(monster);

                            tile.AddContent(monster);

                            Spawner spawner = new Spawner()
                            {
                                SpawnTime = xmlMonster.SpawnTime,

                                Tile = tile,

                                Monster = monster,

                                NextSpawn = null
                            };

                            spawners.Add(spawner);
                        }
                        else
                        {
                            unknownMonsters.Add(xmlMonster.Name);
                        }
                    }
                }

                foreach (var xmlNpc in xmlSpawn.Npcs)
                {
                    Tile tile = server.Map.GetTile(xmlNpc.Position);

                    if (tile != null)
                    {
                        Npc npc = server.NpcFactory.Create(xmlNpc.Name, tile);

                        if (npc != null)
                        {
                            server.NpcFactory.Attach(npc);

                            tile.AddContent(npc);
                        }
                        else
                        {
                            unknownNpcs.Add(xmlNpc.Name);
                        }
                    }
                }
            }

            globalSpawnEventArgs = server.EventHandlers.Subscribe<GlobalSpawnEventArgs>(async (context, e) =>
            {
                foreach (var spawner in GetSpawners() )
                {
                    if (spawner.Monster == null || spawner.Monster.Tile == null || spawner.Monster.IsDestroyed)
                    {
                        if (spawner.NextSpawn == null)
                        {
                            spawner.NextSpawn = DateTime.UtcNow.AddSeconds(spawner.SpawnTime);
                        }
                        else
                        {
                            if (DateTime.UtcNow >= spawner.NextSpawn.Value)
                            {
                                await Context.Current.AddCommand(new ShowMagicEffectCommand(spawner.Tile.Position, MagicEffectType.Teleport) );

                                spawner.Monster = await Context.Current.AddCommand(new TileCreateMonsterCommand(spawner.Tile, spawner.Monster.Name) );

                                spawner.NextSpawn = null;
                            }
                        }
                    }
                }
            } );
        }

        private HashSet<string> unknownMonsters = new HashSet<string>();

        public HashSet<string> UnknownMonsters
        {
            get
            {
                return unknownMonsters;
            }
        }

        private HashSet<string> unknownNpcs = new HashSet<string>();

        public HashSet<string> UnknownNpcs
        {
            get
            {
                return unknownNpcs;
            }
        }

        private List<Spawner> spawners = new List<Spawner>();

        public IEnumerable<Spawner> GetSpawners()
        {
            return spawners;
        }

        public void Stop()
        {
            server.EventHandlers.Unsubscribe<GlobalSpawnEventArgs>(globalSpawnEventArgs);
        }
    }
}
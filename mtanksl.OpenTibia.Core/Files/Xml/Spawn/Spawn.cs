using System.Xml.Linq;
using System.Collections.Generic;

namespace OpenTibia.Xml.Spawn
{
    public class Spawn
    {
        public static Spawn Load(XElement spawnNode)
        {
            Spawn spawn = new Spawn();

            spawn.Center = new Position( (int)spawnNode.Attribute("centerx"), (int)spawnNode.Attribute("centery"), (int)spawnNode.Attribute("centerz") );

            spawn.Radius = (uint)spawnNode.Attribute("radius");

            spawn.monsters = new List<Monster>();

            foreach (var monsterNode in spawnNode.Elements("monster") )
            {
                spawn.monsters.Add( Monster.Load(spawn, monsterNode) );
            }

            spawn.npcs = new List<Npc>();

            foreach (var npcNode in spawnNode.Elements("npc") )
            {
                spawn.npcs.Add( Npc.Load(spawn, npcNode) );
            }
            return spawn;
        }

        public Position Center { get; set; }

        public uint Radius { get; set; }
        
        private List<Monster> monsters;

        public List<Monster> Monsters
        {
            get
            {
                return monsters;
            }
        }

        private List<Npc> npcs;

        public List<Npc> Npcs
        {
            get
            {
                return npcs;
            }
        }
    }
}
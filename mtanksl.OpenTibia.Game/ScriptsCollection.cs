using OpenTibia.Game.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class ScriptsCollection
    {
        private List<IPlayerLogoutScript> playerLogoutScripts = new List<IPlayerLogoutScript>();

        public List<IPlayerLogoutScript> PlayerLogoutScripts
        {
            get
            {
                return playerLogoutScripts;
            }
        }
        
        private List<ICreatureWalkScript> creatureWalkScripts = new List<ICreatureWalkScript>();

        public List<ICreatureWalkScript> CreatureWalkScripts
        {
            get
            {
                return creatureWalkScripts;
            }
        }

        private List<IItemMoveScript> itemMoveScripts = new List<IItemMoveScript>();

        public List<IItemMoveScript> ItemMoveScripts
        {
            get
            {
                return itemMoveScripts;
            }
        }

        private Dictionary<ushort, IItemRotateScript> itemRotateScripts = new Dictionary<ushort, IItemRotateScript>();

        public Dictionary<ushort, IItemRotateScript> ItemRotateScripts
        {
            get
            {
                return itemRotateScripts;
            }
        }

        private Dictionary<ushort, IItemUseScript> itemUseScript = new Dictionary<ushort, IItemUseScript>();

        public Dictionary<ushort, IItemUseScript> ItemUseScripts
        {
            get
            {
                return itemUseScript;
            }
        }

        private Dictionary<ushort, IItemUseWithItemScript> itemUseWithItemScripts = new Dictionary<ushort, IItemUseWithItemScript>();

        public Dictionary<ushort, IItemUseWithItemScript> ItemUseWithItemScripts
        {
            get
            {
                return itemUseWithItemScripts;
            }
        }

        private Dictionary<ushort, IItemUseWithCreatureScript> itemUseWithCreatureScripts = new Dictionary<ushort, IItemUseWithCreatureScript>();

        public Dictionary<ushort, IItemUseWithCreatureScript> ItemUseWithCreatureScripts
        {
            get
            {
                return itemUseWithCreatureScripts;
            }
        }

        private Dictionary<string, ISpeechScript> speechScripts = new Dictionary<string, ISpeechScript>();

        public Dictionary<string, ISpeechScript> SpeechScripts
        {
            get
            {
                return speechScripts;
            }
        }

        private List<IScript> scripts = new List<IScript>();

        public void Start(Server server)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IScript).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract ) )
            {
                IScript script = (IScript)Activator.CreateInstance(type);

                scripts.Add(script);
            }

            foreach (var script in scripts)
            {
                script.Start(server);
            }
        }

        public T GetScript<T>()
        {
            return scripts.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetScripts<T>()
        {
            return scripts.OfType<T>();
        }

        public void Stop(Server server)
        {
            foreach (var script in scripts)
            {
                script.Stop(server);
            }
        }
    }
}
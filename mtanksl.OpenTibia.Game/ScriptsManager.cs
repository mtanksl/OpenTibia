using OpenTibia.Game.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class ScriptsManager
    {
        private Server server;

        public ScriptsManager(Server server)
        {
            this.server = server;

            CreatureWalkScripts = new List<ICreatureWalkScript>();

            TileAddCreatureScripts = new List<ITileAddCreatureScript>();

            TileRemoveCreatureScripts = new List<ITileRemoveCreatureScript>();

            ItemMoveScripts = new List<IItemMoveScript>();

            TileAddItemScripts = new List<ITileAddItemScript>();

            TileRemoveItemScripts = new List<ITileRemoveItemScript>();

            ItemRotateScripts = new Dictionary<ushort, IItemRotateScript>();

            ItemUseScripts = new Dictionary<ushort, IItemUseScript>();

            ItemUseWithItemScripts = new Dictionary<ushort, IItemUseWithItemScript>();

            ItemUseWithCreatureScripts = new Dictionary<ushort, IItemUseWithCreatureScript>();

            SpeechScripts = new Dictionary<string, ISpeechScript>();
        }

        public List<ICreatureWalkScript> CreatureWalkScripts { get; set; }

        public List<ITileAddCreatureScript> TileAddCreatureScripts { get; set; }

        public List<ITileRemoveCreatureScript> TileRemoveCreatureScripts { get; set; }

        public List<IItemMoveScript> ItemMoveScripts { get; set; }

        public List<ITileAddItemScript> TileAddItemScripts { get; set; }

        public List<ITileRemoveItemScript> TileRemoveItemScripts { get; set; }

        public Dictionary<ushort, IItemRotateScript> ItemRotateScripts { get; set; }

        public Dictionary<ushort, IItemUseScript> ItemUseScripts { get; set; }

        public Dictionary<ushort, IItemUseWithItemScript> ItemUseWithItemScripts { get; set; }

        public Dictionary<ushort, IItemUseWithCreatureScript> ItemUseWithCreatureScripts { get; set; }

        public Dictionary<string, ISpeechScript> SpeechScripts { get; set; }

        public void Start()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IScript).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract ) )
            {
                IScript script = (IScript)Activator.CreateInstance(type);

                script.Register(server);
            }
        }
    }
}
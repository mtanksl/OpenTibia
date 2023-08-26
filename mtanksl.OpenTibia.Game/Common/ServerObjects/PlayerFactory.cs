using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private Server server;

        public PlayerFactory(Server server)
        {
            this.server = server;

            scripts = new Dictionary<string, GameObjectScript<string, Player> >();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(GameObjectScript<string, Player>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                GameObjectScript<string, Player> script = (GameObjectScript<string, Player>)Activator.CreateInstance(type);

                scripts.Add(script.Key, script);
            }
        }

        private Dictionary<string, GameObjectScript<string, Player> > scripts;

        public GameObjectScript<string, Player> GetPlayerScript(string name)
        {
            GameObjectScript<string, Player> script;

            if (scripts.TryGetValue(name, out script) )
            {
                return script;
            }
            
            if (scripts.TryGetValue("", out script) )
            {
                return script;
            }

            return null;
        }

        public Player Create(IConnection connection, DbPlayer databasePlayer, Tile spawn)
        {
            Client client = new Client(server);

            Player player = new Player()
            {
                DatabasePlayerId = databasePlayer.Id,

                Name = databasePlayer.Name,

                Health = (ushort)databasePlayer.Health,

                MaxHealth = (ushort)databasePlayer.MaxHealth,

                Direction = (Direction)databasePlayer.Direction,

                BaseOutfit = databasePlayer.BaseOutfitId == 0 ? new Outfit(databasePlayer.BaseOutfitItemId) : new Outfit(databasePlayer.BaseOutfitId, databasePlayer.BaseOutfitHead, databasePlayer.BaseOutfitBody, databasePlayer.BaseOutfitLegs, databasePlayer.BaseOutfitFeet, (Addon)databasePlayer.BaseOutfitAddon),
               
                Outfit = databasePlayer.OutfitId == 0 ? new Outfit(databasePlayer.OutfitItemId) : new Outfit(databasePlayer.OutfitId, databasePlayer.OutfitHead, databasePlayer.OutfitBody, databasePlayer.OutfitLegs, databasePlayer.OutfitFeet, (Addon)databasePlayer.OutfitAddon),

                BaseSpeed = (ushort)databasePlayer.BaseSpeed,

                Speed = (ushort)databasePlayer.Speed,
                
                Invisible = databasePlayer.Invisible,

                Skills = {

                    MagicLevel = (byte)databasePlayer.SkillMagicLevel,

                    MagicLevelPercent = (byte)databasePlayer.SkillMagicLevelPercent,

                    Fist = (byte)databasePlayer.SkillFist,

                    FistPercent = (byte)databasePlayer.SkillFistPercent,

                    Club = (byte)databasePlayer.SkillClub,

                    ClubPercent = (byte)databasePlayer.SkillClubPercent,

                    Sword = (byte)databasePlayer.SkillSword,

                    SwordPercent = (byte)databasePlayer.SkillSwordPercent,

                    Axe = (byte)databasePlayer.SkillAxe,

                    AxePercent = (byte)databasePlayer.SkillAxePercent,

                    Distance = (byte)databasePlayer.SkillDistance,

                    DistancePercent = (byte)databasePlayer.SkillDistancePercent,

                    Shield = (byte)databasePlayer.SkillShield,

                    ShieldPercent = (byte)databasePlayer.SkillShieldPercent,

                    Fish = (byte)databasePlayer.SkillFish,

                    FishPercent = (byte)databasePlayer.SkillFishPercent
                },

                Experience = (uint)databasePlayer.Experience,

                Level = (ushort)databasePlayer.Level,

                LevelPercent = (byte)databasePlayer.LevelPercent,

                Mana = (ushort)databasePlayer.Mana,

                MaxMana = (ushort)databasePlayer.MaxMana,

                Soul = (byte)databasePlayer.Soul,

                Capacity = (uint)databasePlayer.Capacity,

                Stamina = (ushort)databasePlayer.Stamina,

                Gender = (Gender)databasePlayer.Gender,

                Vocation = (Vocation)databasePlayer.Vocation
            };
                 
            client.Connection = connection;

            player.Client = client;

            player.Spawn = spawn;

            server.GameObjects.AddGameObject(player);

            GameObjectScript<string, Player> script = GetPlayerScript(player.Name);

            if (script != null)
            {
                script.Start(player);
            }

            return player;
        }

        public bool Detach(Player player)
        {
            if (server.GameObjects.RemoveGameObject(player) )
            {
                GameObjectScript<string, Player> script = GetPlayerScript(player.Name);

                if (script != null)
                {
                    script.Stop(player);
                }

                return true;
            }

            return false;
        }

        public void Destroy(Player player)
        {
            server.GameObjectComponents.ClearComponents(player);

            server.GameObjectEventHandlers.ClearEventHandlers(player);
        }
    }
}
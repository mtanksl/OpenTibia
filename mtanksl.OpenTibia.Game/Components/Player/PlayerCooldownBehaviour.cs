using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class PlayerCooldownBehaviour : Behaviour
    {
        private Dictionary<string, DateTime> cooldowns = new Dictionary<string, DateTime>();

        public bool HasCooldown(string name)
        {
            DateTime cooldown;

            if (cooldowns.TryGetValue(name, out cooldown) )
            {
                return DateTime.UtcNow < cooldown;
            }

            return false;
        }

        public void AddCooldown(string name, TimeSpan cooldown)
        {
            cooldowns[name] = DateTime.UtcNow.Add(cooldown);
        }

        public override void Start()
        {

        }

        public override void Stop()
        {
            
        }
    }
}
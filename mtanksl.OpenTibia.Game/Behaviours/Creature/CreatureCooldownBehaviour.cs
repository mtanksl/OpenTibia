using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class CreatureCooldownBehaviour : Behaviour
    {
        public override void Start(Server server)
        {

        }

        private Dictionary<string, DateTime> cooldowns = new Dictionary<string, DateTime>();

        public bool HasCooldown(string name)
        {
            DateTime cooldown;

            if ( cooldowns.TryGetValue(name, out cooldown) )
            {
               return DateTime.UtcNow < cooldown;
            }

            return false;
        }

        public void AddCooldown(string name, int cooldownInMilliseconds)
        {
            cooldowns[name] = DateTime.UtcNow.AddMilliseconds(cooldownInMilliseconds);
        }

        public override void Stop(Server server)
        {
            
        }
    }
}
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class RaidCollection : IRaidCollection
    {
        private IServer server;

        public RaidCollection(IServer server)
        {
            this.server = server;
        }

        private Guid globalRaidEventArgs;

        public void Start()
        {
            DateTime nextExecution = DateTime.MinValue;

            HashSet<Raid> executed = new HashSet<Raid>();

            Raid current = null;
            
            globalRaidEventArgs = server.EventHandlers.Subscribe<GlobalRaidEventArgs>( (context, e) =>
            {
                if (DateTime.UtcNow >= nextExecution)
                {
                    if (current == null)
                    {
                        foreach (var raid in server.Plugins.Raids)
                        {
                            if ( !executed.Contains(raid) )
                            {
                                if (server.Randomization.HasProbability(1.0 / raid.Chance) )
                                {
                                    if ( !raid.Repeatable )
                                    {
                                        executed.Add(raid);
                                    }

                                    current = raid;

                                    break;
                                }
                            }
                        }

                        if (current != null)
                        {
                            var raidPlugin = server.Plugins.GetRaidPlugin(current.Name);

                            if (raidPlugin != null)
                            {
                                return raidPlugin.OnRaid().Then( () =>
                                {
                                    nextExecution = DateTime.UtcNow.AddSeconds(current.Cooldown);

                                    current = null;
                                                       
                                    return Promise.Completed;
                                } );
                            }
                        }
                    }
                }

                return Promise.Completed;
            } );
        }

        public void Stop()
        {
            server.EventHandlers.Unsubscribe(globalRaidEventArgs);
        }
    }
}
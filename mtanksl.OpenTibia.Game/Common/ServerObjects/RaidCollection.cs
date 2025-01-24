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

        private Raid current;

        private DateTime lastExecution = DateTime.MinValue;

        public bool Start(string name)
        {
            if (current == null)
            {
                var raidPlugin = server.Plugins.GetRaidPlugin(name);

                if (raidPlugin != null)
                {
                    current = raidPlugin.Raid;

                    raidPlugin.OnRaid().Then( () =>
                    {
                        current = null;

                        lastExecution = DateTime.UtcNow;

                        return Promise.Completed;

                    } ).Catch( (ex) =>
                    {
                        current = null;

                        lastExecution = DateTime.UtcNow;

                        if (ex is PromiseCanceledException)
                        {
                            //
                        }
                        else
                        {
                            server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                        }
                    } );

                    return true;
                }
            }

            return false;
        }

        private HashSet<Raid> executed = new HashSet<Raid>();

        private Guid globalRaid;

        public void Start()
        {
            globalRaid = server.EventHandlers.Subscribe<GlobalRaidEventArgs>( (context, e) =>
            {
                if (current == null)
                {
                    foreach (var raid in server.Plugins.Raids)
                    {
                        if (raid.Enabled && !executed.Contains(raid) && DateTime.UtcNow >= lastExecution.AddSeconds(raid.Interval) && server.Randomization.HasProbability(raid.Chance / 100.0) )
                        {
                            if ( !raid.Repeatable)
                            {
                                executed.Add(raid);
                            }

                            Start(raid.Name);

                            break;
                        }
                    }
                }

                return Promise.Completed;
            } );
        }

        public void Stop()
        {
            server.EventHandlers.Unsubscribe(globalRaid);
        }
    }
}
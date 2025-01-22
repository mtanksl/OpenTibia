using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Threading.Tasks;

namespace OpenTibia.Game.Scripts
{
    public class GlobalScripts : Script
    {
        public override void Start()
        {
            RealClockTick();
            
            TibiaClockTick();
             
            Tick(0);
             
            Spawn();

            Raid();

            Light();

            Ping();

            Save(0);

            Clean(0);
        }

        private void RealClockTick()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetSecondInterval(now, 60 * 1000);

            Promise.Delay("RealClockTick", next - now).Then( () =>
            {
                RealClockTick();

                Context.AddEvent(new GlobalRealClockTickEventArgs(next.Hour, next.Minute) );

                return Promise.Completed;
            } );
        }

        private void TibiaClockTick()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetSecondInterval(now, Clock.Interval);

            Promise.Delay("TibiaClockTick", next - now).Then( () =>
            {
                TibiaClockTick();

                Context.Server.Clock.Tick();

                Context.AddEvent(new GlobalTibiaClockTickEventArgs(Context.Server.Clock.Hour, Context.Server.Clock.Minute) );

                return Promise.Completed;
            } );
        }

        private void Tick(int index)
        {
            DateTime now = DateTime.Now;

            DateTime next = GetSecondInterval(now, GlobalTickEventArgs.Interval / GlobalTickEventArgs.Count);

            Promise.Delay("Tick", next - now).Then( () =>
            {
                Tick( (index + 1) % GlobalTickEventArgs.Count);

                Context.AddEvent(GlobalTickEventArgs.Instance(index) );

                return Promise.Completed;
            } );
        }

        private void Spawn()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetSecondInterval(now, GlobalSpawnEventArgs.Interval);

            Promise.Delay("Spawn", next - now).Then( () =>
            {
                Spawn();

                Context.AddEvent(GlobalSpawnEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        private void Raid()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetSecondInterval(now, GlobalRaidEventArgs.Interval);

            Promise.Delay("Raid", next - now).Then( () =>
            {
                Raid();

                Context.AddEvent(GlobalRaidEventArgs.Instance);

                return Promise.Completed;
            } );
        }
        
        private void Light()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetSecondInterval(now, GlobalEnvironmentLightEventArgs.Interval);

            Promise.Delay("Light", next - now).Then( () =>
            {
                Light();

                Context.AddEvent(GlobalEnvironmentLightEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        private void Ping()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetSecondInterval(now, GlobalPingEventArgs.Interval);

            Promise.Delay("Ping", next - now).Then( () =>
            {
                Ping();

                Context.AddEvent(GlobalPingEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        private void Save(int state)
        {
            if (state == 0)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now, 24 * 60 * 60 * 1000);

                if ( (next - now).TotalMinutes > 5)
                {
                    Save(1);
                }
                else if ( (next - now).TotalMinutes > 3)
                {
                    Save(2);
                }
                else if ( (next - now).TotalMinutes > 1)
                {
                    Save(3);
                }
                else
                {
                    Save(4);
                }
            }
            else if (state == 1)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now, 24 * 60 * 60 * 1000).AddMinutes(-5);

                Promise.Delay("Save", next - now).Then( () =>
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Server is saving game in 5 minutes. Please come back in 10 minutes.") );
                    }

                    Save(2);
                                    
                    return Promise.Completed;
                } );
            }
            else if (state == 2)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now, 24 * 60 * 60 * 1000).AddMinutes(-3);

                Promise.Delay("Save", next - now).Then( () =>
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Server is saving game in 3 minutes. Please come back in 10 minutes.") );
                    }

                    Save(3);

                    return Promise.Completed;
                } );
            }
            else if (state == 3)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now, 24 * 60 * 60 * 1000).AddMinutes(-1);

                Promise.Delay("Save", next - now).Then( () =>
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Server is saving game in 1 minute. Please log out.") );
                    }

                    Save(4);

                    return Promise.Completed;
                } );
            }
            else if (state == 4)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now, 24 * 60 * 60 * 1000);

                Promise.Delay("Save", next - now).Then( () =>
                {
                    IServer server = Context.Server;

                    Task.Run( () =>
                    {
                        server.Pause();

                        server.KickAll();

                        server.Save();

                        server.Continue();
                    } );

                    Save(0);

                    return Promise.Completed;
                } );
            }
        }

        private void Clean(int state)
        {
            if (state == 0)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now.AddHours(3), 6 * 60 * 60 * 1000).AddHours(-3);

                if ( (next - now).TotalMinutes > 1)
                {
                    Clean(1);
                }
                else
                {
                    Clean(2);
                }
            }
            else if (state == 1)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now.AddHours(3), 6 * 60 * 60 * 1000).AddHours(-3).AddMinutes(-1);

                Promise.Delay("Clean", next - now).Then( () =>
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Server is cleaning floor in 1 minute. Please take all your items.") );
                    }

                    Clean(2);

                    return Promise.Completed;
                } );
            }
            else if (state == 2)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetDayInterval(now.AddHours(3), 6 * 60 * 60 * 1000).AddHours(-3);

                Promise.Delay("Clean", next - now).Then( () =>
                {
                    IServer server = Context.Server;

                    Task.Run( () =>
                    {
                        server.Pause();

                        server.Clean();

                        server.Continue();
                    } );

                    Clean(0);

                    return Promise.Completed;
                } );
            }
        }

        public override void Stop()
        {
            Context.Server.CancelQueueForExecution("RealClockTick");

            Context.Server.CancelQueueForExecution("TibiaClockTick");

            Context.Server.CancelQueueForExecution("Tick");

            Context.Server.CancelQueueForExecution("Spawn");

            Context.Server.CancelQueueForExecution("Raid");

            Context.Server.CancelQueueForExecution("Light");

            Context.Server.CancelQueueForExecution("Ping");

            Context.Server.CancelQueueForExecution("Save");

            Context.Server.CancelQueueForExecution("Clean");
        }

        private static DateTime GetDayInterval(DateTime now, int millisecond)
        {
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0).AddMilliseconds(Round(now.Hour * 60 * 60 * 1000 + now.Minute * 60 * 1000 + now.Second * 1000 + now.Millisecond, millisecond) ).AddMilliseconds(millisecond);
        }

        private static DateTime GetSecondInterval(DateTime now, int millisecond)
        {
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0).AddMilliseconds(Round(now.Second * 1000 + now.Millisecond, millisecond) ).AddMilliseconds(millisecond);
        }

        private static int Round(double value, int round)
        {
            return (int)Math.Floor(value / round) * round;
        }
    }
}
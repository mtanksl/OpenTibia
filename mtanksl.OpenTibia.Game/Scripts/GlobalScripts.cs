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

            ServerSave(0);
        }

        private void RealClockTick()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNextMinute(now);

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

            DateTime next = GetNextSecond(now, Clock.Interval);

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

            DateTime next = GetNextSecond(now, 100);

            Promise.Delay("Tick", next - now).Then( () =>
            {
                Tick( (index + 1) % GlobalTickEventArgs.Instance.Length);

                Context.AddEvent(GlobalTickEventArgs.Instance[index] );

                return Promise.Completed;
            } );
        }

        private void Spawn()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNextSecond(now, 10 * 1000);

            Promise.Delay("Spawn", next - now).Then(() =>
            {
                Spawn();

                Context.AddEvent(GlobalSpawnEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        private void Raid()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNextMinute(now);

            Promise.Delay("Raid", next - now).Then(() =>
            {
                Raid();

                Context.AddEvent(GlobalRaidEventArgs.Instance);

                return Promise.Completed;
            } );
        }
        
        private void Light()
        {
            DateTime now = DateTime.Now;

            DateTime next = GetNextSecond(now, 10 * 1000);

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

            DateTime next = GetNextSecond(now, 10 * 1000);

            Promise.Delay("Ping", next - now).Then( () =>
            {
                Ping();

                Context.AddEvent(GlobalPingEventArgs.Instance);

                return Promise.Completed;
            } );
        }

        private void ServerSave(int state)
        {
            if (state == 0)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetNextDay(now);

                if ( (next - now).TotalMinutes > 5)
                {
                    ServerSave(1);
                }
                else if ( (next - now).TotalMinutes > 3)
                {
                    ServerSave(2);
                }
                else if ( (next - now).TotalMinutes > 1)
                {
                    ServerSave(3);
                }
                else
                {
                    ServerSave(4);
                }
            }
            else if (state == 1)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetNextDay(now).AddMinutes(-5);

                Promise.Delay("ServerSave", next - now).Then( () =>
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Server is saving game in 5 minutes. Please come back in 10 minutes.") );
                    }

                    ServerSave(2);
                                    
                    return Promise.Completed;
                } );
            }
            else if (state == 2)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetNextDay(now).AddMinutes(-3);

                Promise.Delay("ServerSave", next - now).Then( () =>
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Server is saving game in 3 minutes. Please come back in 10 minutes.") );
                    }

                    ServerSave(3);

                    return Promise.Completed;
                } );
            }
            else if (state == 3)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetNextDay(now).AddMinutes(-1);

                Promise.Delay("ServerSave", next - now).Then( () =>
                {
                    foreach (var observer in Context.Server.GameObjects.GetPlayers() )
                    {
                        Context.AddPacket(observer, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, "Server is saving game in 1 minute. Please log out.") );
                    }

                    ServerSave(4);

                    return Promise.Completed;
                } );
            }
            else if (state == 4)
            {
                DateTime now = DateTime.Now;

                DateTime next = GetNextDay(now);

                Promise.Delay("ServerSave", next - now).Then( () =>
                {
                    IServer server = Context.Server;

                    Task.Run( () =>
                    {
                        server.Pause();

                        server.KickAll();

                        server.Save();

                        server.Continue();
                    } );

                    ServerSave(0);

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

            Context.Server.CancelQueueForExecution("ServerSave");
        }

        private static DateTime GetNextDay(DateTime now)
        {
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0).AddDays(1);
        }

        private static DateTime GetNextHour(DateTime now)
        {
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, 0).AddHours(1);
        }

        private static DateTime GetNextMinute(DateTime now)
        {
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0).AddMinutes(1);
        }

        private static DateTime GetNextSecond(DateTime now)
        {
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, 0).AddSeconds(1);
        }

        private static DateTime GetNextSecond(DateTime now, int millisecond)
        {
            if (millisecond < 0 || millisecond > 60000)
            {
                throw new ArgumentException("Millisecond must be between 0 and 60000");
            }

            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0).AddMilliseconds(Round(now.Second * 1000 + now.Millisecond, millisecond) ).AddMilliseconds(millisecond);
        }

        private static int Round(double value, int round)
        {
            return (int)Math.Floor(value / round) * round;
        }
    }
}
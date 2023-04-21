using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System;

namespace OpenTibia.Game.Components
{
    public abstract class ScheduledBehaviour : Behaviour
    {
        private int hour;

        private int minute;

        public ScheduledBehaviour(int hour, int minute)
        {
            this.hour = hour;
            
            this.minute = minute;
        }

        public override bool IsUnique
        {
            get
            {
                return false;
            }
        }

        private Guid token;

        public override void Start(Server server)
        {
            token = server.EventHandlers.Subscribe<GlobalClockTickEventArgs>( (context, e) =>
            {
                if (e.Hour == hour && e.Minute == minute)
                {
                    return Update().Catch( (ex) =>
                    {
                        if (ex is PromiseCanceledException)
                        {
                            //
                        }
                        else
                        {
                            server.Logger.WriteLine(ex.ToString(), LogLevel.Error);
                        }
                    } );
                }

                return Promise.Completed;
            } );
        }

        public abstract Promise Update();

        public override void Stop(Server server)
        {
            server.EventHandlers.Unsubscribe<GlobalClockTickEventArgs>(token);
        }
    }
}
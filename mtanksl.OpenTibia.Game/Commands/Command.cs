using OpenTibia.Game.Events;
using OpenTibia.Web;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        public abstract void Execute(Context context);
        
        private List<IEvent> events = new List<IEvent>();

        public void AddEvent(IEvent e)
        {
            events.Add(e);
        }

        public IEnumerable<IEvent> GetEvents()
        {
            return events;
        }
    }
}
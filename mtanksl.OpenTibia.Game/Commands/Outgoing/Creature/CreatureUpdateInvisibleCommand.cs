using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Commands
{
    public class CreatureUpdateInvisibleCommand : Command
    {
        public CreatureUpdateInvisibleCommand(Creature creature, bool invisible)
        {
            Creature = creature;

            Invisible = invisible;
        }

        public Creature Creature { get; set; }

        public bool Invisible { get; set; }

        public override Promise Execute()
        {
            if (Creature.Invisible != Invisible)
            {
                Creature.Invisible = Invisible;

                //TODO

                Context.AddEvent(new CreatureUpdateInvisibleEventArgs(Creature, Invisible) );
            }

            return Promise.Completed;
        }
    }
}
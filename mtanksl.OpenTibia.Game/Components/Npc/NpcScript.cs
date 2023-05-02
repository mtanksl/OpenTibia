using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Components
{
    public abstract class NpcScript
    {
        public abstract string Address(Npc npc, Player player);

        public abstract string Busy(Npc npc, Player player);

        public abstract string Handle(Npc npc, Player player, string message);

        public abstract string Idle(Npc npc, Player player);

        public abstract string Vanish(Npc npc);
    }

    public class CipfriedNpcScript : NpcScript
    {
        public override string Address(Npc npc, Player player)
        {
            return "Hello, " + player.Name + "! Feel free to ask me for help.";
        }

        public override string Busy(Npc npc, Player player)
        {
            return "Please wait, " + player.Name + ". I already talk to someone!";
        }

        public override string Handle(Npc npc, Player player, string message)
        {
            if (message == "name")
            {
                return "My name is Cipfried.";
            }
            else if (message == "job")
            {
                return "I am just a humble monk. Ask me if you need help or healing.";
            }
            else if (message == "monk")
            {
                return "I sacrifice my life to serve the good gods of Tibia.";
            }
            else if (message == "tibia")
            {
                return "That's where we are. The world of Tibia.";
            }

            return null;
        }

        public override string Idle(Npc npc, Player player)
        {
            return "Farewell, " + player.Name + "!";
        }

        public override string Vanish(Npc npc)
        {
            return "Well, bye then.";
        }
    }

    public class AldeeNpcScript : NpcScript
    {
        public override string Address(Npc npc, Player player)
        {
            return "Hello, hello, " + player.Name + "! Please come in, look, and buy!";
        }

        public override string Busy(Npc npc, Player player)
        {
            return "I'll be with you in a moment, " + player.Name + ".";
        }

        public override string Handle(Npc npc, Player player, string message)
        {
            if (message == "name")
            {
                return "My name is Al Dee, but you can call me Al. Do you want to buy something?";
            }
            else if (message == "job")
            {
                return "I am a merchant. What can I do for you?";
            }

            return null;
        }

        public override string Idle(Npc npc, Player player)
        {
            return "Bye, bye.";
        }

        public override string Vanish(Npc npc)
        {
            return "Bye, bye.";
        }
    }
}
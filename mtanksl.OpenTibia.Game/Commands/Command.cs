namespace OpenTibia.Game.Commands
{
    public abstract class Command
    {
        public abstract void Execute(Server server, CommandContext context);
    }
}
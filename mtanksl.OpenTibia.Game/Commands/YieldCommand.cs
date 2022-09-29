namespace OpenTibia.Game.Commands
{
    public class YieldCommand : Command
    {
        public YieldCommand()
        {
            
        }
        
        public override void Execute(Context context)
        {
            context.Server.QueueForExecution(ctx =>
            {
                base.Execute(ctx);
            } );
        }
    }
}
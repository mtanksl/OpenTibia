namespace OpenTibia.Game.Commands
{
    public class DelayCommand : Command
    {
        private string key;

        private int executeInMilliseconds;

        public DelayCommand(string key, int executeInMilliseconds)
        {
            this.key = key;

            this.executeInMilliseconds = executeInMilliseconds;
        }
        
        public override void Execute(Context context)
        {
            context.Server.QueueForExecution(key, executeInMilliseconds, ctx =>
            {
                OnComplete(ctx);
            } );
        }
    }
}
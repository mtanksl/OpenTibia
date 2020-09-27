namespace OpenTibia.Game.Commands
{
    public class DelayCommand : Command
    {
        private string key;

        private int executeIn;

        public DelayCommand(string key, int executeIn)
        {
            this.key = key;

            this.executeIn = executeIn;
        }

        private int index = 0;

        public override void Execute(Context context)
        {
            if (index++ == 0)
            {
                context.Server.QueueForExecution(key, executeIn, this);
            }
            else
            {
                base.OnCompleted(context);
            }
        }
    }
}
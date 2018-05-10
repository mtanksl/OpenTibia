namespace OpenTibia.Game
{
    public class FlushResult : IActionResult
    {
        public void Execute(Context context)
        {
            context.Response.Flush();
        }
    }
}
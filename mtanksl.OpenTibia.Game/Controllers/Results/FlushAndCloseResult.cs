namespace OpenTibia.Game
{
    public class FlushAndCloseResult : IActionResult
    {
        public void Execute(Context context)
        {
            context.Response.Flush();
        }
    }
}
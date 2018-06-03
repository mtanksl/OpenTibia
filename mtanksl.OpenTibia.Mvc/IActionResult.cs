using OpenTibia.Web;

namespace OpenTibia.Mvc
{
    public interface IActionResult
    {
        void Execute(Context context);
    }
}
using OpenTibia.Common.Objects;

namespace OpenTibia.Game
{
    public abstract class Controller
    {
        public Context Context { get; set; }

        public Request Request
        {
            get
            {
                return Context.Request;
            }
        }

        public Response Response
        {
            get
            {
                return Context.Response;
            }
        }

        public IConnection Connection
        {
            get
            {
                return Context.Request.Connection;
            }
        }

        public IClient Client
        {
            get
            {
                return Context.Request.Connection.Client;
            }
        }

        public Player Player
        {
            get
            {
                return Context.Request.Connection.Client.Player;
            }
        }

        public IActionResult Flush()
        {
            return new FlushResult();
        }

        public IActionResult FlushAndClose()
        {
            return new FlushAndCloseResult();
        }
    }
}
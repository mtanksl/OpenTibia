using OpenTibia.Common.Objects;
using OpenTibia.Web;

namespace OpenTibia.Mvc
{
    public abstract class Controller
    {
        public Controller(IConnection connection)
        {
            this.context = new Context(connection);
        }

        private Context context;

        public Context Context
        {
            get
            {
                return context;
            }
        }

        public Request Request
        {
            get
            {
                return context.Request;
            }
        }

        public Response Response
        {
            get
            {
                return context.Response;
            }
        }

        public IConnection Connection
        {
            get
            {
                return context.Request.Connection;
            }
        }

        public IClient Client
        {
            get
            {
                return context.Request.Connection.Client;
            }
        }

        public Player Player
        {
            get
            {
                return context.Request.Connection.Client.Player;
            }
        }

        public IActionResult Empty()
        {
            return null;
        }
    }
}
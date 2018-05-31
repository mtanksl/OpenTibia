using OpenTibia.Common.Objects;

namespace OpenTibia.Web
{
    public class Context
    {
        public Context(IConnection connection)
        {
            this.request = new Request(this, connection);

            this.response = new Response(this);
        }
        
        private Request request;

        public Request Request
        {
            get
            {
                return request;
            }
        }

        private Response response;

        public Response Response
        {
            get
            {
                return response;
            }
        }
    }
}
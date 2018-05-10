namespace OpenTibia.Game
{
    public class Context
    {
        private Request request;

        public Request Request
        {
            get
            {
                return request;
            }
            set
            {
                request = value;

                if (request != null && request.Context != this)
                {
                    request.Context = this;
                }
            }
        }

        private Response response;

        public Response Response
        {
            get
            {
                return response;
            }
            set
            {
                response = value;

                if (response != null && response.Context != this)
                {
                    response.Context = this;
                }
            }
        }
    }
}
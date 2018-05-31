using OpenTibia.Common.Objects;

namespace OpenTibia.Web
{
    public class Request
    {
        public Request(Context context, IConnection connection)
        {
            this.context = context;

            this.connection = connection;
        }

        private Context context;

        public Context Context
        {
            get
            {
                return context;
            }
        }

        private IConnection connection;

        public IConnection Connection
        {
            get
            {
                return connection;
            }
        }
    }
}
using OpenTibia.Common.Objects;

namespace OpenTibia.Game
{
    public class Request
    {
        private Context context;

        public Context Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;

                if (context != null && context.Request != this)
                {
                    context.Request = this;
                }
            }
        }

        public IConnection Connection { get; set; }
    }
}
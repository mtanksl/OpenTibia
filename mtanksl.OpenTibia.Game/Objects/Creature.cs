using System;

namespace OpenTibia.Game.Objects
{
    public abstract class Creature : IContent
    {
        public TopOrder TopOrder
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IContainer Container
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
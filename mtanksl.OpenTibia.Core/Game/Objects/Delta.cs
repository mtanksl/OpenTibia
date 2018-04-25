using System;

namespace OpenTibia
{
    public class Delta
    {
        public Delta(int x, int y, int z)
        {
            this.x = x;

            this.y = y;

            this.z = z;
        }

        private int x;

        public int X
        {
            get
            {
                return x;
            }
        }

        private int y;

        public int Y
        {
            get
            {
                return y;
            }
        }

        private int z;

        public int Z
        {
            get
            {
                return z;
            }
        }

        public int ModuleX
        {
            get
            {
                return Math.Abs(x);
            }
        }

        public int ModuleY
        {
            get
            {
                return Math.Abs(y);
            }
        }

        public int ModuleZ
        {
            get
            {
                return Math.Abs(z);
            }
        }
    }
}
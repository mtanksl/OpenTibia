namespace OpenTibia.Common.Structures
{
    public struct Offset
    {
        public Offset(int x, int y) : this(x, y, 0) 
        {
        
        }

        public Offset(int x, int y, int z)
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

        public static bool operator ==(Offset a, Offset b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Offset a, Offset b)
        {
            return !(a == b);
        }

        public override bool Equals(object offset)
        {
            return offset is Offset && Equals( (Offset)offset );
        }

        public bool Equals(Offset offset)
        {
            return (x == offset.x) && (y == offset.y) && (z == offset.z);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            hashCode = hashCode * 23 + x.GetHashCode();

            hashCode = hashCode * 23 + y.GetHashCode();

            hashCode = hashCode * 23 + z.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return "Offset x: " + x + " y: " + y + " z: " + z;
        }
    }
}
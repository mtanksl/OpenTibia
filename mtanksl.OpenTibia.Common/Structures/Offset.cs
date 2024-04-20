namespace OpenTibia.Common.Structures
{
    public struct Offset
    {
        public static readonly Offset[] Line5 = new Offset[]
        {
            new Offset(-2, 0), new Offset(-1, 0), new Offset(0, 0), new Offset(1, 0), new Offset(2, 0)
        };

        public static readonly Offset[] Square1 = new Offset[]
        {
            new Offset(0, 0)
        };

        public static readonly Offset[] Square3 = new Offset[]
        {
            new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1),
            new Offset(-1, 0) , new Offset(0, 0) , new Offset(1, 0),
            new Offset(-1, 1) , new Offset(0, 1) , new Offset(1, 1)
        };

        public static readonly Offset[] Circle3 = new Offset[]
        {
                                new Offset(0, -1),
            new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),
                                new Offset(0, 1)
        };

        public static readonly Offset[] Circle5 = new Offset[]
        {
                                new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2),
            new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1),
            new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),
            new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),
                                new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2)
        };

        public static readonly Offset[] Circle7 = new Offset[]
        {
                                                    new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3),
			                    new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2),
		    new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1),
		    new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),
		    new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),
			                    new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),
			                                        new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3)
        };

        public static readonly Offset[] Circle11 = new Offset[]
        {
                                                                                                               new Offset(0, -5),
                                                                       new Offset(-2, -4), new Offset(-1, -4), new Offset(0, -4), new Offset(1, -4), new Offset(2, -4),
                                                   new Offset(-3, -3), new Offset(-2, -3), new Offset(-1, -3), new Offset(0, -3), new Offset(1, -3), new Offset(2, -3), new Offset(3, -3),
                               new Offset(-4, -2), new Offset(-3, -2), new Offset(-2, -2), new Offset(-1, -2), new Offset(0, -2), new Offset(1, -2), new Offset(2, -2), new Offset(3, -2), new Offset(4, -2),
                               new Offset(-4, -1), new Offset(-3, -1), new Offset(-2, -1), new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(2, -1), new Offset(3, -1), new Offset(4, -1),
            new Offset(-5, 0), new Offset(-4, 0),  new Offset(-3, 0),  new Offset(-2, 0),  new Offset(-1, 0),  new Offset(0, 0),  new Offset(1, 0),  new Offset(2, 0),  new Offset(3, 0),  new Offset(4, 0),  new Offset(5, 0),
                               new Offset(-4, 1),  new Offset(-3, 1),  new Offset(-2, 1),  new Offset(-1, 1),  new Offset(0, 1),  new Offset(1, 1),  new Offset(2, 1),  new Offset(3, 1),  new Offset(4, 1),
                               new Offset(-4, 2),  new Offset(-3, 2),  new Offset(-2, 2),  new Offset(-1, 2),  new Offset(0, 2),  new Offset(1, 2),  new Offset(2, 2),  new Offset(3, 2),  new Offset(4, 2),
                                                   new Offset(-3, 3),  new Offset(-2, 3),  new Offset(-1, 3),  new Offset(0, 3),  new Offset(1, 3),  new Offset(2, 3),  new Offset(3, 3),
                                                                       new Offset(-2, 4),  new Offset(-1, 4),  new Offset(0, 4),  new Offset(1, 4),  new Offset(2, 4),
                                                                                                               new Offset(0, 5),

        };

        public static readonly Offset[] Beam1 = new Offset[]
        {
            new Offset(0, 1)
        };

        public static readonly Offset[] Beam5 = new Offset[]
        {
            new Offset(0, 1),
            new Offset(0, 2),
            new Offset(0, 3),
            new Offset(0, 4),
            new Offset(0, 5)
        };

        public static readonly Offset[] Beam7 = new Offset[]
        {
            new Offset(0, 1),
            new Offset(0, 2),
            new Offset(0, 3),
            new Offset(0, 4),
            new Offset(0, 5),
            new Offset(0, 6),
            new Offset(0, 7)
        };

        public static readonly Offset[] Wave1133 = new Offset[]
        {
                               new Offset(0, 1),
                               new Offset(0, 2),
            new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
            new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
        };

        public static readonly Offset[] Wave1335 = new Offset[]
        {
                                                  new Offset(0, 1),
                               new Offset(-1, 2), new Offset(0, 2), new Offset(-1, 2),
                               new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
            new Offset(-2, 4), new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4), new Offset(2, 4),
        };

        public static readonly Offset[] Wave11333 = new Offset[]
        {
                               new Offset(0, 1),
                               new Offset(0, 2),
            new Offset(-1, 3), new Offset(0, 3), new Offset(1, 3),
            new Offset(-1, 4), new Offset(0, 4), new Offset(1, 4),
            new Offset(-1, 5), new Offset(0, 5), new Offset(1, 5),
        };
                
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
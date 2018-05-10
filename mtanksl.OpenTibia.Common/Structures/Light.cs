namespace OpenTibia.Common.Structures
{
    public class Light
    {
        public static readonly Light Day = new Light(250, 215);

        public static readonly Light Night = new Light(40, 215);
        
        public Light(byte level, byte color)
        {
            this.level = level;

            this.color = color;
        }

        private byte level;

        public byte Level
        {
            get
            {
                return level;
            }
        }

        private byte color;

        public byte Color
        {
            get
            {
                return color;
            }
        }

        public static bool operator ==(Light a, Light b)
        {
            if ( object.ReferenceEquals(a, b) )
            {
                return true;
            }

            if ( object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null) )
	        {
                return false;
	        }

            return (a.level == b.level) && (a.color == b.color);
        }

        public static bool operator !=(Light a, Light b)
        {
            return !(a == b);
        }
        
        public override bool Equals(object light)
        {
            return Equals(light as Light);
        }

        public bool Equals(Light light)
        {
            if (light == null)
            {
                return false;
            }

            return (level == light.level) && (color == light.color);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            hashCode = hashCode * 23 + level.GetHashCode();

            hashCode = hashCode * 23 + color.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            return "Level: " + level + " Color: " + color;
        }
    }
}
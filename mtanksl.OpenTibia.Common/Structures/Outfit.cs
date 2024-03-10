namespace OpenTibia.Common.Structures
{
    public class Outfit
    {
        public static readonly Outfit Invisible = new Outfit(0, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit Swimming = new Outfit(267, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit MaleCitizen = new Outfit(128, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit MaleHunter = new Outfit(129, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit MaleMage = new Outfit(130, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit MaleKnight = new Outfit(131, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit FemaleCitizen = new Outfit(136, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit FemaleHunter = new Outfit(137, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit FemaleMage = new Outfit(138, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit FemaleKnight = new Outfit(139, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit GamemasterRed = new Outfit(266, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit GamemasterGreen = new Outfit(302, 0, 0, 0, 0, Addon.None);

        public static readonly Outfit GamemasterBlue = new Outfit(75, 0, 0, 0, 0, Addon.None);

        public Outfit(int tibiaId) : this( (ushort)tibiaId)
        {

        }

        public Outfit(ushort tibiaId)
        {
            this.tibiaId = tibiaId;
        }

        private ushort tibiaId;

        public ushort TibiaId
        {
            get
            {
                return tibiaId;
            }
        }

        public Outfit(int id, int head, int body, int legs, int feet, Addon addon) : this( (ushort)id, (byte)head, (byte)body, (byte)legs, (byte)feet, addon )
        {

        }

        public Outfit(ushort id, byte head, byte body, byte legs, byte feet, Addon addon)
        {
            this.id = id;

            this.head = head;

            this.body = body;

            this.legs = legs;

            this.feet = feet;

            this.addon = addon;
        }
        
        private ushort id;

        public ushort Id
        {
            get
            {
                return id;
            }
        }
        
        private byte head;

        public byte Head
        {
            get
            {
                return head;
            }
        }

        private byte body;

        public byte Body
        {
            get
            {
                return body;
            }
        }

        private byte legs;

        public byte Legs
        {
            get
            {
                return legs;
            }
        }

        private byte feet;

        public byte Feet
        {
            get
            {
                return feet;
            }
        }

        private Addon addon;

        public Addon Addon
        {
            get
            {
                return addon;
            }
        }

        public static bool operator ==(Outfit a, Outfit b)
        {
            if ( object.ReferenceEquals(a, b) )
            {
                return true;
            }

            if ( object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null) )
            {
                return false;
            }

            return (a.tibiaId == b.tibiaId) && (a.id == b.id) && (a.head == b.head) && (a.body == b.body) && (a.legs == b.legs) && (a.feet == b.feet) && (a.addon == b.addon);
        }

        public static bool operator !=(Outfit a, Outfit b)
        {
            return !(a == b);
        }
        
        public override bool Equals(object outfit)
        {
            return Equals(outfit as Outfit);
        }

        public bool Equals(Outfit outfit)
        {
            if (outfit is null)
            {
                return false;
            }

            return (tibiaId == outfit.tibiaId) && (id == outfit.id) && (head == outfit.head) && (body == outfit.body) && (legs == outfit.legs) && (feet == outfit.feet) && (addon == outfit.addon);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            hashCode = hashCode * 23 + tibiaId.GetHashCode();

            hashCode = hashCode * 23 + id.GetHashCode();

            hashCode = hashCode * 23 + head.GetHashCode();

            hashCode = hashCode * 23 + body.GetHashCode();

            hashCode = hashCode * 23 + legs.GetHashCode();

            hashCode = hashCode * 23 + feet.GetHashCode();

            hashCode = hashCode * 23 + addon.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            if (tibiaId == 0)
            {
                return "Id: " + id + " Head: " + head + " Body: " + body + " Legs: " + legs + " Feet: " + feet + " Addons: " + addon;
            }

            return "Item id: " + tibiaId;
        }
    }
}
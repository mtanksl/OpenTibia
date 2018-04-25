namespace OpenTibia
{
    public class Outfit
    {
        public Outfit(int itemId) : this( (ushort)itemId )
        {

        }

        public Outfit(ushort itemId)
        {
            this.itemId = itemId;
        }

        private ushort itemId;

        public ushort ItemId
        {
            get
            {
                return itemId;
            }
        }

        public Outfit(int id, int head, int body, int legs, int feet, Addons addons) : this( (ushort)id, (byte)head, (byte)body, (byte)legs, (byte)feet, addons )
        {

        }

        public Outfit(ushort id, byte head, byte body, byte legs, byte feet, Addons addons)
        {
            this.id = id;

            this.head = head;

            this.body = body;

            this.legs = legs;

            this.feet = feet;

            this.addons = addons;
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

        private Addons addons;

        public Addons Addons
        {
            get
            {
                return addons;
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

            return (a.itemId == b.itemId) && (a.id == b.id) && (a.head == b.head) && (a.body == b.body) && (a.legs == b.legs) && (a.feet == b.feet) && (a.addons == b.addons);
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
            if (outfit == null)
            {
                return false;
            }

            return (itemId == outfit.itemId) && (id == outfit.id) && (head == outfit.head) && (body == outfit.body) && (legs == outfit.legs) && (feet == outfit.feet) && (addons == outfit.addons);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            hashCode = hashCode * 23 + itemId.GetHashCode();

            hashCode = hashCode * 23 + id.GetHashCode();

            hashCode = hashCode * 23 + head.GetHashCode();

            hashCode = hashCode * 23 + body.GetHashCode();

            hashCode = hashCode * 23 + legs.GetHashCode();

            hashCode = hashCode * 23 + feet.GetHashCode();

            hashCode = hashCode * 23 + addons.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            if (itemId == 0)
            {
                return "Id: " + id + " Head: " + head + " Body: " + body + " Legs: " + legs + " Feet: " + feet + " Addons: " + addons;
            }

            return "Item id: " + itemId;
        }
    }
}
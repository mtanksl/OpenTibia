using System.Drawing;

namespace OpenTibia.Common.Structures
{
    public class Outfit
    {
        public static readonly Color[] Colors = new Color[]
        {
            Color.FromArgb(255, 255, 255),
            Color.FromArgb(255, 212, 191),
            Color.FromArgb(255, 233, 191),
            Color.FromArgb(255, 255, 191),
            Color.FromArgb(233, 255, 191),
            Color.FromArgb(212, 255, 191),
            Color.FromArgb(191, 255, 191),
            Color.FromArgb(191, 255, 212),
            Color.FromArgb(191, 255, 233),
            Color.FromArgb(191, 255, 255),
            Color.FromArgb(191, 233, 255),
            Color.FromArgb(191, 212, 255),
            Color.FromArgb(191, 191, 255),
            Color.FromArgb(212, 191, 255),
            Color.FromArgb(233, 191, 255),
            Color.FromArgb(255, 191, 255),
            Color.FromArgb(255, 191, 233),
            Color.FromArgb(255, 191, 212),
            Color.FromArgb(255, 191, 191),
            Color.FromArgb(218, 218, 218),
            Color.FromArgb(191, 159, 143),
            Color.FromArgb(191, 175, 143),
            Color.FromArgb(191, 191, 143),
            Color.FromArgb(175, 191, 143),
            Color.FromArgb(159, 191, 143),
            Color.FromArgb(143, 191, 143),
            Color.FromArgb(143, 191, 159),
            Color.FromArgb(143, 191, 175),
            Color.FromArgb(143, 191, 191),
            Color.FromArgb(143, 175, 191),
            Color.FromArgb(143, 159, 191),
            Color.FromArgb(143, 143, 191),
            Color.FromArgb(159, 143, 191),
            Color.FromArgb(175, 143, 191),
            Color.FromArgb(191, 143, 191),
            Color.FromArgb(191, 143, 175),
            Color.FromArgb(191, 143, 159),
            Color.FromArgb(191, 143, 143),
            Color.FromArgb(182, 182, 181),
            Color.FromArgb(191, 127, 95),
            Color.FromArgb(191, 159, 95),
            Color.FromArgb(191, 191, 95),
            Color.FromArgb(159, 191, 95),
            Color.FromArgb(127, 191, 95),
            Color.FromArgb(95, 191, 95),
            Color.FromArgb(95, 191, 127),
            Color.FromArgb(95, 191, 159),
            Color.FromArgb(95, 191, 191),
            Color.FromArgb(95, 159, 191),
            Color.FromArgb(95, 127, 191),
            Color.FromArgb(95, 95, 191),
            Color.FromArgb(127, 95, 191),
            Color.FromArgb(159, 95, 191),
            Color.FromArgb(191, 95, 191),
            Color.FromArgb(191, 95, 159),
            Color.FromArgb(191, 95, 127),
            Color.FromArgb(191, 95, 95),
            Color.FromArgb(145, 145, 144),
            Color.FromArgb(191, 106, 63),
            Color.FromArgb(191, 148, 63),
            Color.FromArgb(191, 191, 63),
            Color.FromArgb(148, 191, 63),
            Color.FromArgb(106, 191, 63),
            Color.FromArgb(63, 191, 63),
            Color.FromArgb(63, 191, 106),
            Color.FromArgb(63, 191, 148),
            Color.FromArgb(63, 191, 191),
            Color.FromArgb(63, 148, 191),
            Color.FromArgb(63, 106, 191),
            Color.FromArgb(63, 63, 191),
            Color.FromArgb(106, 63, 191),
            Color.FromArgb(148, 63, 191),
            Color.FromArgb(191, 63, 191),
            Color.FromArgb(191, 63, 148),
            Color.FromArgb(191, 63, 106),
            Color.FromArgb(191, 63, 63),
            Color.FromArgb(109, 109, 109),
            Color.FromArgb(255, 85, 0),
            Color.FromArgb(255, 170, 0),
            Color.FromArgb(255, 255, 0),
            Color.FromArgb(170, 255, 0),
            Color.FromArgb(84, 255, 0),
            Color.FromArgb(0, 255, 0),
            Color.FromArgb(0, 255, 84),
            Color.FromArgb(0, 255, 170),
            Color.FromArgb(0, 255, 255),
            Color.FromArgb(0, 169, 255),
            Color.FromArgb(0, 85, 255),
            Color.FromArgb(0, 0, 255),
            Color.FromArgb(85, 0, 255),
            Color.FromArgb(169, 0, 255),
            Color.FromArgb(254, 0, 255),
            Color.FromArgb(255, 0, 170),
            Color.FromArgb(255, 0, 85),
            Color.FromArgb(255, 0, 0),
            Color.FromArgb(72, 72, 68),
            Color.FromArgb(191, 63, 0),
            Color.FromArgb(191, 127, 0),
            Color.FromArgb(191, 191, 0),
            Color.FromArgb(127, 191, 0),
            Color.FromArgb(63, 191, 0),
            Color.FromArgb(0, 191, 0),
            Color.FromArgb(0, 191, 63),
            Color.FromArgb(0, 191, 127),
            Color.FromArgb(0, 191, 191),
            Color.FromArgb(0, 127, 191),
            Color.FromArgb(0, 63, 191),
            Color.FromArgb(0, 0, 191),
            Color.FromArgb(63, 0, 191),
            Color.FromArgb(127, 0, 191),
            Color.FromArgb(191, 0, 191),
            Color.FromArgb(191, 0, 127),
            Color.FromArgb(191, 0, 63),
            Color.FromArgb(191, 0, 0),
            Color.FromArgb(36, 36, 36),
            Color.FromArgb(127, 42, 0),
            Color.FromArgb(127, 85, 0),
            Color.FromArgb(127, 127, 0),
            Color.FromArgb(85, 127, 0),
            Color.FromArgb(42, 127, 0),
            Color.FromArgb(0, 127, 0),
            Color.FromArgb(0, 127, 42),
            Color.FromArgb(0, 127, 85),
            Color.FromArgb(0, 127, 127),
            Color.FromArgb(0, 84, 127),
            Color.FromArgb(0, 42, 127),
            Color.FromArgb(0, 0, 127),
            Color.FromArgb(42, 0, 127),
            Color.FromArgb(84, 0, 127),
            Color.FromArgb(127, 0, 127),
            Color.FromArgb(127, 0, 85),
            Color.FromArgb(127, 0, 42),
            Color.FromArgb(127, 0, 0)
        };

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
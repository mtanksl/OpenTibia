using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Inventory : IContainer
    {
        public Inventory(Player player)
        {
            this.player = player;
        }

        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
        }

        private IContent[] contents = new IContent[11];

        /// <exception cref="NotSupportedException"></exception>

        public int AddContent(IContent content)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="ArgumentException"></exception>
        
        public void AddContent(IContent content, int index)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            contents[index] = content;

            content.Parent = this;
        }

        /// <exception cref="ArgumentException"></exception>

        public void ReplaceContent(int index, IContent content)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            IContent oldContent = GetContent(index);

            contents[index] = content;

            oldContent.Parent = null;

            content.Parent = this;
        }

        public void RemoveContent(int index)
        {
            IContent content = GetContent(index);

            contents[index] = null;

            content.Parent = null;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public int GetIndex(IContent content)
        {
            for (int index = 0; index < contents.Length; index++)
            {
                if (contents[index] == content)
                {
                    return index;
                }
            }

            throw new InvalidOperationException("Content not found.");
        }

        public bool TryGetIndex(IContent content, out int _index)
        {
            for (int index = 0; index < contents.Length; index++)
            {
                if (contents[index] == content)
                {
                    _index = index;

                    return true;
                }
            }

            _index = 0;

            return false;
        }

        public IContent GetContent(int index)
        {
            if (index < 0 || index > contents.Length - 1)
            {
                return null;
            }

            return contents[index];
        }

        public IEnumerable<IContent> GetContents()
        {
            foreach (var content in contents)
            {
                if (content != null)
                {
                    yield return content;
                }
            }
        }

        public IEnumerable< KeyValuePair<int, IContent> > GetIndexedContents()
        {
            for (int index = 0; index < contents.Length; index++)
            {
                if (contents[index] != null)
                {
                    yield return new KeyValuePair<int, IContent>( index, contents[index] );
                }
            }
        }

        public IEnumerable<Item> GetItems()
        {
            return GetContents().OfType<Item>();
        }

        public uint GetWeight()
        {
            uint weight = 0;

            foreach (var item in GetItems() )
            {
                weight += item.GetWeight();
            }

            return weight;
        }

        public int GetDefense()
        {
            Item GetWeapon()
            {
                foreach (var slot in new[] { Slot.Left, Slot.Right } )
                {
                    Item item = (Item)contents[ (int)slot ];

                    if (item != null && (item.Metadata.WeaponType == WeaponType.Sword || item.Metadata.WeaponType == WeaponType.Club || item.Metadata.WeaponType == WeaponType.Axe) )
                    {
                        return item;
                    }
                }

                return null;
            }

            Item GetShield()
            {
                foreach (var slot in new[] { Slot.Left, Slot.Right } )
                {
                    Item item = (Item)contents[ (int)slot ];

                    if (item != null && item.Metadata.WeaponType == WeaponType.Shield)
                    {
                        return item;
                    }
                }

                return null;
            }

            int defense = 0;

            int defenseModifier = 0;

            Item weapon = GetWeapon();

            Item shield = GetShield();

            if (weapon != null)
            {
                defense = weapon.Metadata.Defense ?? 0;

                defenseModifier = weapon.Metadata.DefenseModifier ?? 0;
            }

            if (shield != null)
            {
                defense = shield.Metadata.Defense ?? 0;

                defenseModifier = Math.Max(defenseModifier, shield.Metadata.DefenseModifier ?? 0);
            }

            return defense + defenseModifier;
        }

        public double GetArmorReductionPercent(DamageType damageType)
        {
            double armorReductionPercent = 1;

            foreach (var item in GetItems() )
            {
                double elementPercent;

                if (item.Metadata.DamageTakenFromElements.TryGetValue(damageType, out elementPercent) )
                {
                    armorReductionPercent *= elementPercent;
                }
            }

            return armorReductionPercent;
        }

        public int GetArmor()
        {
            int armor = 0;

            foreach (var item in GetItems() )
            {
                if (item.Metadata.Armor != null)
                {
                    armor += item.Metadata.Armor.Value;
                }
            }

            return armor;           
        }
    }
}
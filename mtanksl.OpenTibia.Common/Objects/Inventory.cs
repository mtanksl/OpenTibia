﻿using OpenTibia.Common.Structures;
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

            defense = null;

            armor = null;
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

            defense = null;

            armor = null;
        }

        public void RemoveContent(int index)
        {
            IContent content = GetContent(index);

            contents[index] = null;

            content.Parent = null;

            defense = null;

            armor = null;
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

        private int? defense;

        public int GetDefense()
        {
            int Defense()
            {
                int defense = 0;

                int extraDefense = 0;

                Item weapon = null;

                Item shield = null;

                Item left = (Item)contents[ (int)Slot.Left ];

                Item right = (Item)contents[ (int)Slot.Right ];

                if (left != null)
                {
                    if (left.Metadata.WeaponType == WeaponType.Sword || left.Metadata.WeaponType == WeaponType.Club || left.Metadata.WeaponType == WeaponType.Axe)
                    {
                        weapon = left;
                    }
                    else if (left.Metadata.WeaponType == WeaponType.Shield)
                    {
                        shield = left;
                    }
                }

                if (right != null)
                {
                    if (right.Metadata.WeaponType == WeaponType.Sword || right.Metadata.WeaponType == WeaponType.Club || right.Metadata.WeaponType == WeaponType.Axe)
                    {
                        weapon = right;
                    }
                    else if (right.Metadata.WeaponType == WeaponType.Shield)
                    {
                        shield = right;
                    }
                }

                if (weapon != null)
                {
                    defense = weapon.Metadata.Defense ?? 0;

                    extraDefense = weapon.Metadata.ExtraDefense ?? 0;
                }

                if (shield != null)
                {
                    defense = shield.Metadata.Defense ?? 0;

                    int shieldExtraDefense = shield.Metadata.ExtraDefense ?? 0;

                    if (shieldExtraDefense > extraDefense)
                    {
                        extraDefense = shieldExtraDefense;
                    }
                }

                return defense + extraDefense;
            }

            if (defense == null)
            {
                defense = Defense();
            }

            return defense.Value;
        }

        private int? armor;

        public int GetArmor()
        {
            int Armor()
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

            if (armor == null)
            {
                armor = Armor();
            }

            return armor.Value;
        }
    }
}
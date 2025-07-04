﻿using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.FileFormats.Dat
{
    public class DatFile
    {
        public static DatFile Load(string path, bool spritesUInt32, bool idleAnimations, bool enhancedAnimations, bool noMovementAnimation)
        {
            using ( ByteArrayFileStream stream = new ByteArrayFileStream(path) )
            {
                ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

                DatFile file = new DatFile();

                file.signature = reader.ReadUInt();

                ushort items = reader.ReadUShort();

                ushort outfits = reader.ReadUShort();

                ushort effects = reader.ReadUShort();

                ushort projectiles = reader.ReadUShort();

                file.items = new List<Item>(items);

                for (ushort itemId = 100; itemId <= items; itemId++)
                {
                    Item item = Item.Load(reader, spritesUInt32, false, enhancedAnimations, noMovementAnimation);

                        item.TibiaId = itemId;

                    file.items.Add(item);
                }

                file.outfits = new List<Item>(outfits);

                for (ushort outfitId = 1; outfitId <= outfits; outfitId++)
                {
                    Item item = Item.Load(reader, spritesUInt32, idleAnimations, enhancedAnimations, noMovementAnimation);

                        item.TibiaId = outfitId;

                    file.outfits.Add(item);
                }

                file.effects = new List<Item>(effects);

                for (ushort effectId = 1; effectId <= effects; effectId++)
                {
                    Item item = Item.Load(reader, spritesUInt32, false, enhancedAnimations, noMovementAnimation);

                        item.TibiaId = effectId;

                    file.effects.Add(item);
                }

                file.projectiles = new List<Item>(projectiles);

                for (ushort projectileId = 1; projectileId <= projectiles; projectileId++)
                {
                    Item item = Item.Load(reader, spritesUInt32, false, enhancedAnimations, noMovementAnimation);

                        item.TibiaId = projectileId;

                    file.projectiles.Add(item);
                }

                return file;
            }
        }

        private uint signature;

        public uint Signature
        {
            get
            {
                return signature;
            }
        }

        private List<Item> items;

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }

        private List<Item> outfits;

        public List<Item> Outfits
        {
            get
            {
                return outfits;
            }
        }

        private List<Item> effects;

        public List<Item> Effects
        {
            get
            {
                return effects;
            }
        }

        private List<Item> projectiles;

        public List<Item> Projectiles
        {
            get
            {
                return projectiles;
            }
        }
    }
}
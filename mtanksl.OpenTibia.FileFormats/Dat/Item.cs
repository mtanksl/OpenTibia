using OpenTibia.FileFormats.Spr;
using OpenTibia.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace OpenTibia.FileFormats.Dat
{
    public class Item
    {
        public static Item Load(ByteArrayStreamReader reader)
        {
            Item item = new Item();

            while (true)
            {
                switch ( (DatAttribute)reader.ReadByte() )
                {
                    case DatAttribute.IsGround:

                        item.Flags |= ItemFlags.IsGround;

                        item.Speed = reader.ReadUShort();

                        break;

                    case DatAttribute.AlwaysOnTop1:

                        item.Flags |= ItemFlags.AlwaysOnTop1;

                        break;

                    case DatAttribute.AlwaysOnTop2:

                        item.Flags |= ItemFlags.AlwaysOnTop2;

                        break;

                    case DatAttribute.AlwaysOnTop3:

                        item.Flags |= ItemFlags.AlwaysOnTop3;

                        break;

                    case DatAttribute.IsContainer:

                        item.Flags |= ItemFlags.IsContainer;

                        break;

                    case DatAttribute.Stackable:

                        item.Flags |= ItemFlags.Stackable;

                        break;

                    case DatAttribute.IsCorpse:

                        break;

                    case DatAttribute.Useable:

                        item.Flags |= ItemFlags.Useable;

                        break;

                    case DatAttribute.Writeable:

                        item.Flags |= ItemFlags.Writeable;

                        item.MaxWriteChars = reader.ReadUShort();

                        break;

                    case DatAttribute.Readable:

                        item.Flags |= ItemFlags.Readable;

                        item.MaxReadChars = reader.ReadUShort();

                        break;

                    case DatAttribute.IsFluid:

                        item.Flags |= ItemFlags.IsFluid;

                        break;

                    case DatAttribute.IsSplash:

                        item.Flags |= ItemFlags.IsSplash;

                        break;

                    case DatAttribute.NotWalkable:

                        item.Flags |= ItemFlags.NotWalkable;

                        break;

                    case DatAttribute.NotMoveable:

                        item.Flags |= ItemFlags.NotMoveable;

                        break;

                    case DatAttribute.BlockProjectile:

                        item.Flags |= ItemFlags.BlockProjectile;

                        break;

                    case DatAttribute.BlockPathFinding:

                        item.Flags |= ItemFlags.BlockPathFinding;

                        break;

                    case DatAttribute.Pickupable:

                        item.Flags |= ItemFlags.Pickupable;

                        break;

                    case DatAttribute.Hangable:

                        item.Flags |= ItemFlags.Hangable;

                        break;

                    case DatAttribute.Horizontal:

                        item.Flags |= ItemFlags.Horizontal;

                        break;

                    case DatAttribute.Vertical:

                        item.Flags |= ItemFlags.Vertical;

                        break;

                    case DatAttribute.Rotatable:

                        item.Flags |= ItemFlags.Rotatable;

                        break;

                    case DatAttribute.Light:

                        item.LightLevel = reader.ReadUShort();

                        item.LightColor = reader.ReadUShort();

                        break;

                    case DatAttribute.Offset:

                        item.OffsetX = reader.ReadUShort();

                        item.OffsetY = reader.ReadUShort();

                        break;

                    case DatAttribute.HasHeight:

                        item.ItemHeight = reader.ReadUShort();

                        break;

                    case DatAttribute.IdleAnimation:

                        item.Flags |= ItemFlags.IdleAnimation;

                        break;

                    case DatAttribute.MinimapColor:

                        item.MinimapColor = reader.ReadUShort();

                        break;

                    case DatAttribute.ExtraInfo:

                        item.ExtraInfo = (ExtraInfo)reader.ReadUShort();

                        break;
                        
                    case DatAttribute.SolidGround:

                        item.Flags |= ItemFlags.SolidGround;

                        break;

                    case DatAttribute.LookThrough:

                        item.Flags |= ItemFlags.LookThrough;

                        break;
                        
                    case DatAttribute.End:

                        item.Width = reader.ReadByte();

                        item.Height = reader.ReadByte();

                        if (item.Width > 1 || item.Height > 1)
                        {
                            item.CropSize = reader.ReadByte();
                        }

                        item.Layers = reader.ReadByte();

                        item.XRepeat = reader.ReadByte();

                        item.YRepeat = reader.ReadByte();

                        item.ZRepeat = reader.ReadByte();

                        item.Animations = reader.ReadByte();

                        int sprites = item.Width * item.Height * item.Layers * item.XRepeat * item.YRepeat * item.ZRepeat * item.Animations;

                        item.spriteIds = new List<ushort>(sprites);

                        for (int i = 0; i < sprites; i++)
                        {
                            item.spriteIds.Add( reader.ReadUShort() );
                        }

                        return item;
                }
            }
        }

        public ushort TibiaId { get; set; }

        public ItemFlags Flags { get; set; }

        public ushort Speed { get; set; }

        public ushort MaxWriteChars { get; set; }

        public ushort MaxReadChars { get; set; }

        public ushort LightLevel { get; set; }

        public ushort LightColor { get; set; }

        public ushort OffsetX { get; set; }

        public ushort OffsetY { get; set; }

        public ushort ItemHeight { get; set; }

        public ushort MinimapColor { get; set; }

        public ExtraInfo ExtraInfo { get; set; }

        public byte Width { get; set; }

        public byte Height { get; set; }

        public byte CropSize { get; set; }

        public byte Layers { get; set; }

        public byte XRepeat { get; set; }

        public byte YRepeat { get; set; }

        public byte ZRepeat { get; set; }

        public byte Animations { get; set; }

        private List<ushort> spriteIds;

        public List<ushort> SpriteIds
        {
            get
            {
                return spriteIds;
            }
        }

        public Bitmap GetImage(List<Sprite> sprites, int animation, int z, int y, int x, int layer)
        {
            animation = Math.Min(Animations - 1, animation);

            z = Math.Min(ZRepeat - 1, z);

            y = Math.Min(YRepeat - 1, y);

            x = Math.Min(XRepeat - 1, x);

            layer = Math.Min(Layers - 1, layer);
            
            Bitmap bitmap = new Bitmap(32 * Width, 32 * Height);

            using (Graphics graphics = Graphics.FromImage(bitmap) )
            {
                /*
                int index = ZRepeat * YRepeat * XRepeat * Layers * Width * Height * animation +

                            YRepeat * XRepeat * Layers * Width * Height * z +

                            XRepeat * Layers * Width * Height * y +

                            Layers * Width * Height * x +

                            Width * Height * layer;
                */

                int index = Width * Height * (Layers * (XRepeat * (YRepeat * (ZRepeat * animation + z) + y) + x) + layer);

                for (int j = Height - 1; j >= 0; j--)
                {
                    for (int i = Width - 1; i >= 0; i--)
                    {
                        ushort spriteId = spriteIds[index++];

                        if (spriteId > 0)
                        {
                            Bitmap image = sprites.First(sprite => sprite.Id == spriteId).GetImage();

                            graphics.DrawImage(image, 32 * i, 32 * j);
                        }
                    }
                }
            }

            return bitmap;
        }    
        
        public Bitmap GetImage(List<Sprite> sprites, int animation, int z, int y, int x)
        {
            Bitmap bitmap = new Bitmap(32 * Width, 32 * Height);

            using (Graphics graphics = Graphics.FromImage(bitmap) )
            {
                for (int l = 0; l < Layers; l++)
                {
                    Bitmap image = GetImage(sprites, animation, z, y, x, l);

                    graphics.DrawImage(image, 0, 0);
                }
            }

            return bitmap;
        }  

        public Bitmap GetImage(List<Sprite> sprites, int animation, int z, int y, int x, Color replaceYellow, Color replaceRed, Color replaceGreen, Color replaceBlue)
        {
            if (Layers == 2)
            {
                Bitmap bitmap = new Bitmap(32 * Width, 32 * Height);

                using (Graphics graphics = Graphics.FromImage(bitmap) )
                {
                    Bitmap imageMask = GetImage(sprites, animation, z, y, x, 1);

                        ReplaceColor(imageMask, Color.FromArgb(255, 255, 0), replaceYellow);

                        ReplaceColor(imageMask, Color.FromArgb(255, 0, 0), replaceRed);

                        ReplaceColor(imageMask, Color.FromArgb(0, 255, 0), replaceGreen);

                        ReplaceColor(imageMask, Color.FromArgb(0, 0, 255), replaceBlue);

                    Bitmap imageBase = GetImage(sprites, animation, z, y, x, 0);

                        MultiplicativeBlending(imageBase, imageMask);

                    graphics.DrawImage(imageBase, 0, 0);
                }

                return bitmap;
            }
                
            return GetImage(sprites, animation, z, y, x);           
        }   

        private void ReplaceColor(Bitmap bitmap, Color oldColor, Color newColor)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            byte[] pixels = new byte[bitmapData.Stride /* = 4 * bitmap.Width */ * bitmap.Height];

            Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // 0 1 2 3
                // B G R A

                if (pixels[i + 2] == oldColor.R && 
                    pixels[i + 1] == oldColor.G &&
                    pixels[i + 0] == oldColor.B)
                {
                    pixels[i + 2] = newColor.R;
                    pixels[i + 1] = newColor.G;
                    pixels[i + 0] = newColor.B;
                }
            }

            Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);

            bitmap.UnlockBits(bitmapData);
        }

        private void MultiplicativeBlending(Bitmap bitmap, Bitmap mask)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            byte[] pixels = new byte[bitmapData.Stride /* = 4 * bitmap.Width */ * bitmap.Height];

            Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);


            BitmapData bitmapData2 = mask.LockBits(new Rectangle(0, 0, mask.Width, mask.Height), ImageLockMode.ReadOnly, mask.PixelFormat);

            byte[] pixels2 = new byte[bitmapData2.Stride /* = 4 * mask.Width */ * mask.Height];

            Marshal.Copy(bitmapData2.Scan0, pixels2, 0, pixels2.Length);

            mask.UnlockBits(bitmapData2);


            for (int i = 0; i < pixels.Length; i += 4)
            {
                // 0 1 2 3
                // B G R A

                if (pixels2[i + 2] != 0 || 
                    pixels2[i + 1] != 0 || 
                    pixels2[i + 0] != 0)
                {
                    pixels[i + 2] = (byte)(pixels[i + 2] * pixels2[i + 2] / 255);
                    pixels[i + 1] = (byte)(pixels[i + 1] * pixels2[i + 1] / 255);
                    pixels[i + 0] = (byte)(pixels[i + 0] * pixels2[i + 0] / 255);
                }
            }

            Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);

            bitmap.UnlockBits(bitmapData);
        }
    }
}
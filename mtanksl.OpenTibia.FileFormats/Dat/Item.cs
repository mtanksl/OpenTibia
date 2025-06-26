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
        public static Item Load(ByteArrayStreamReader reader, bool spritesUInt32, bool idleAnimations, bool enhancedAnimations, bool noMovementAnimation)
        {
            Item item = new Item();

            while (true)
            {
                int attribute = (int)reader.ReadByte();

                if (noMovementAnimation)
                {
                    if (attribute == 16)
                    {
                        attribute = 253;
                    }
                    else if (attribute > 16 && attribute < 255)
                    {
                        attribute -= 1;
                    }
                }

                switch ( (DatAttribute)attribute)
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

                    case DatAttribute.Cloth:

                        item.Cloth = reader.ReadUShort();

                        break;

                    case DatAttribute.Market:

                        item.Category = reader.ReadUShort();

                        item.TradeAs = reader.ReadUShort(); 

                        item.ShowAs = reader.ReadUShort();

                        item.Name = reader.ReadString();

                        item.RestrictVocation = reader.ReadUShort();

                        item.RequiredLevel = reader.ReadUShort();

                        break;

                    case DatAttribute.Usable:

                        reader.ReadUShort();

                        break;

                    case DatAttribute.Wrappable:
                    case DatAttribute.Unwrappable:
                    case DatAttribute.TopEffect:
                    case DatAttribute.NoMoveAnimation:

                        break;

                    case DatAttribute.End:

                        int groupCount;

                        if (idleAnimations)
                        {
                            groupCount = (int)reader.ReadByte();
                        }
                        else
                        {
                            groupCount = 1;
                        }

                        for (int j = 0; j < groupCount; j++)
                        {
                            if (idleAnimations)
                            {
                                byte frameGroupType = reader.ReadByte(); // 0 = Idle, 1 = Moving


                            }

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

                            if (enhancedAnimations)
                            {
                                if (item.Animations > 1)
                                {
                                    bool async = reader.ReadBool();

                                    uint loopCount = reader.ReadUInt();

                                    byte startPhase = reader.ReadByte();

                                    for (int i = 0; i < item.Animations; i++)
                                    {
                                        uint minimum = reader.ReadUInt();

                                        uint maximum = reader.ReadUInt();


                                    }
                                }
                            }

                            int sprites = item.Width * item.Height * item.Layers * item.XRepeat * item.YRepeat * item.ZRepeat * item.Animations;

                            if (item.spriteIds == null)
                            {
                                item.spriteIds = new List<int>(sprites);
                            }

                            for (int i = 0; i < sprites; i++)
                            {
                                int spriteId;

                                if (spritesUInt32)
                                {
                                    spriteId = reader.ReadInt();
                                }
                                else
                                {
                                    spriteId = reader.ReadUShort();
                                }

                                item.spriteIds.Add(spriteId);
                            }
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

        public ushort Cloth { get; set; }

        public ushort Category { get; set; }

        public ushort TradeAs { get; set; }

        public ushort ShowAs { get; set; }

        public string Name { get; set; }

        public ushort RestrictVocation { get; set; }

        public ushort RequiredLevel { get; set; }

        public byte Width { get; set; }

        public byte Height { get; set; }

        public byte CropSize { get; set; }

        public byte Layers { get; set; }

        public byte XRepeat { get; set; }

        public byte YRepeat { get; set; }

        public byte ZRepeat { get; set; }

        public byte Animations { get; set; }

        private List<int> spriteIds;

        public List<int> SpriteIds
        {
            get
            {
                return spriteIds;
            }
        }

        private Bitmap GetEnvironmentLight()
        {
            Bitmap bitmap = new Bitmap(32 * Width, 32 * Height);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            byte[] pixels = new byte[bitmapData.Stride /* = 4 * bitmap.Width */ * bitmap.Height];

            int lightColor = 215;

            int lightLevel = 250;

            Color color = ColorFrom8Bit(lightColor);

            for (int j = 0; j < bitmap.Height; j++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    // 0 1 2 3
                    // B G R A

                    pixels[4 * (j * bitmap.Width + i) + 2] = (byte)(lightLevel * color.R / 255);
                    pixels[4 * (j * bitmap.Width + i) + 1] = (byte)(lightLevel * color.G / 255);
                    pixels[4 * (j * bitmap.Width + i) + 0] = (byte)(lightLevel * color.B / 255);
                    pixels[4 * (j * bitmap.Width + i) + 3] = 255;
                }
            }

            Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        private Bitmap GetLight()
        {
            Bitmap bitmap = new Bitmap(32 * Width, 32 * Height);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            byte[] pixels = new byte[bitmapData.Stride /* = 4 * bitmap.Width */ * bitmap.Height];

            Color color = ColorFrom8Bit( (int)LightColor);

            int cxy = Math.Max(bitmap.Width, bitmap.Height) / 2;

            int maxradius = Math.Min(8, Math.Max(1, (int)LightLevel) ) * 32;

            int minradius = maxradius * 10 / 100;

            for (int j = 0; j < bitmap.Height; j++)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    int dx = cxy - i;

                    int dy = cxy - j;

                    double radius = Math.Sqrt(dx * dx + dy * dy);

                    double intensity = Math.Min(1, Math.Max(0, (maxradius - radius) / (maxradius - minradius) ) );

                    // 0 1 2 3
                    // B G R A

                    pixels[4 * (j * bitmap.Width + i) + 2] = (byte)(intensity * color.R);
                    pixels[4 * (j * bitmap.Width + i) + 1] = (byte)(intensity * color.G);
                    pixels[4 * (j * bitmap.Width + i) + 0] = (byte)(intensity * color.B);
                    pixels[4 * (j * bitmap.Width + i) + 3] = 255;
                }
            }

            Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        private byte ColorTo8Bit(Color color)
        {
            int c = 0;

            c += (color.R / 51) * 36;

            c += (color.G / 51) * 6;

            c += (color.B / 51);

            return (byte)c;
        }

        private Color ColorFrom8Bit(int color)
        {
            if (color <= 0 || color >= 216)
            {
                return Color.FromArgb(0, 0, 0);
            }

            int r = (color / 36) % 6 * 51;

            int g = (color / 6) % 6 * 51;

            int b = color % 6 * 51;

            return Color.FromArgb(r, g, b);
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
                        int spriteId = spriteIds[index++];

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

        public Bitmap GetImageItem(List<Sprite> sprites, int animation, int z, int y, int x)
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

        public Bitmap GetImageOutfit(List<Sprite> sprites, int animation, int addon, int direction, Color replaceYellow, Color replaceRed, Color replaceGreen, Color replaceBlue)
        {
            if (Layers == 2)
            {
                Bitmap bitmap = new Bitmap(32 * Width, 32 * Height);

                using (Graphics graphics = Graphics.FromImage(bitmap) )
                {
                    {
                        Bitmap imageMask = GetImage(sprites, animation, 0, 0, direction, 1);

                            ReplaceColor(imageMask, Color.FromArgb(255, 255, 0), replaceYellow);

                            ReplaceColor(imageMask, Color.FromArgb(255, 0, 0), replaceRed);

                            ReplaceColor(imageMask, Color.FromArgb(0, 255, 0), replaceGreen);

                            ReplaceColor(imageMask, Color.FromArgb(0, 0, 255), replaceBlue);

                        Bitmap imageBase = GetImage(sprites, animation, 0, 0, direction, 0);

                            Blending(imageBase, imageMask, BlendingType.Multiplicative);

                        graphics.DrawImage(imageBase, 0, 0);
                    }

                    if (YRepeat == 3)
                    {
                        if ( (addon & 1) == 1)
                        {
                            Bitmap imageMask = GetImage(sprites, animation, 0, 1, direction, 1);

                                ReplaceColor(imageMask, Color.FromArgb(255, 255, 0), replaceYellow);

                                ReplaceColor(imageMask, Color.FromArgb(255, 0, 0), replaceRed);

                                ReplaceColor(imageMask, Color.FromArgb(0, 255, 0), replaceGreen);

                                ReplaceColor(imageMask, Color.FromArgb(0, 0, 255), replaceBlue);

                            Bitmap imageBase = GetImage(sprites, animation, 0, 1, direction, 0);

                                Blending(imageBase, imageMask, BlendingType.Multiplicative);

                            graphics.DrawImage(imageBase, 0, 0);
                        }

                        if ( (addon & 2) == 2)
                        {
                            Bitmap imageMask = GetImage(sprites, animation, 0, 2, direction, 1);

                                ReplaceColor(imageMask, Color.FromArgb(255, 255, 0), replaceYellow);

                                ReplaceColor(imageMask, Color.FromArgb(255, 0, 0), replaceRed);

                                ReplaceColor(imageMask, Color.FromArgb(0, 255, 0), replaceGreen);

                                ReplaceColor(imageMask, Color.FromArgb(0, 0, 255), replaceBlue);

                            Bitmap imageBase = GetImage(sprites, animation, 0, 2, direction, 0);

                                Blending(imageBase, imageMask, BlendingType.Multiplicative);

                            graphics.DrawImage(imageBase, 0, 0);
                        }
                    }
                }

                return bitmap;
            }
                
            return GetImageItem(sprites, animation, 0, addon, direction);           
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

        private enum BlendingType
        {
            Multiplicative,

            Additive
        }

        private void Blending(Bitmap bitmap, Bitmap mask, BlendingType type)
        {            
            BitmapData bitmapData2 = mask.LockBits(new Rectangle(0, 0, mask.Width, mask.Height), ImageLockMode.ReadOnly, mask.PixelFormat);

            byte[] pixels2 = new byte[bitmapData2.Stride /* = 4 * mask.Width */ * mask.Height];

            Marshal.Copy(bitmapData2.Scan0, pixels2, 0, pixels2.Length);

            mask.UnlockBits(bitmapData2);


            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            byte[] pixels = new byte[bitmapData.Stride /* = 4 * bitmap.Width */ * bitmap.Height];

            Marshal.Copy(bitmapData.Scan0, pixels, 0, pixels.Length);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // 0 1 2 3
                // B G R A

                if (pixels2[i + 2] != 0 || 
                    pixels2[i + 1] != 0 || 
                    pixels2[i + 0] != 0)
                {
                    if (type == BlendingType.Multiplicative)
                    {
                        pixels[i + 2] = (byte)(pixels[i + 2] * pixels2[i + 2] / 255);
                        pixels[i + 1] = (byte)(pixels[i + 1] * pixels2[i + 1] / 255);
                        pixels[i + 0] = (byte)(pixels[i + 0] * pixels2[i + 0] / 255);
                    }
                    else if (type == BlendingType.Additive)
                    {
                        pixels[i + 2] = (byte)Math.Min(pixels[i + 2] + pixels2[i + 2], 255);
                        pixels[i + 1] = (byte)Math.Min(pixels[i + 1] + pixels2[i + 1], 255);
                        pixels[i + 0] = (byte)Math.Min(pixels[i + 0] + pixels2[i + 0], 255);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }                    
                }
            }

            Marshal.Copy(pixels, 0, bitmapData.Scan0, pixels.Length);

            bitmap.UnlockBits(bitmapData);
        }
    }
}
using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.FileFormats.Spr
{
    public class SprFile
    {
        public static SprFile Load(string path, bool gameSpritesUInt32)
        {
            using ( ByteArrayFileStream stream = new ByteArrayFileStream(path) )
            {
                ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);

                SprFile file = new SprFile();

                file.signature = reader.ReadUInt();

                int sprites;

                if (gameSpritesUInt32)
                {
                    sprites = reader.ReadInt();
                }
                else
                {
                    sprites = reader.ReadUShort();
                }

                file.sprites = new List<Sprite>(sprites);

                for (int spriteId = 1; spriteId <= sprites; spriteId++)
                {
                    int index = reader.ReadInt();

                    if (index > 0)
                    {
                        int returnIndex = stream.Position;

                        stream.Seek(Origin.Begin, index);

                            Sprite sprite = Sprite.Load(true, reader);

                                sprite.Id = spriteId;

                            file.sprites.Add(sprite);

                            stream.Seek(Origin.Begin, returnIndex);
                    }
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

        private List<Sprite> sprites;
        
        public List<Sprite> Sprites
        {
            get
            {
                return sprites;
            }
        }
    }
}
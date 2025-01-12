using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.FileFormats.Otb
{
    public class OtbFile
    {
        public static OtbFile Load(string path)
        {
            using ( ByteArrayFileTreeStream stream = new ByteArrayFileTreeStream(path) )
            {
                ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);
            
                OtbFile file = new OtbFile();

                stream.Seek(Origin.Current, 4); // Empty

                if ( stream.Child() )
                {
                    file.otbInfo = OtbInfo.Load(stream, reader);

                    if ( stream.Child() )
                    {
                        file.items = new List<Item>();

                        while (true)
                        {
                            file.items.Add( Item.Load(stream, reader) );

                            if ( !stream.Next() )
                            {
                                break;
                            }
                        }
                    }
                }

                return file;  
            }
        }

        private OtbInfo otbInfo;

        public OtbInfo OtbInfo
        {
            get
            {
                return otbInfo;
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
    }
}
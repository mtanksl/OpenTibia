using OpenTibia.IO;
using System.Collections.Generic;
using System.IO;

namespace OpenTibia.FileFormats.Otbm
{
    public class OtbmFile
    {
        public static OtbmFile Load(string path)
        {
            using ( ByteArrayFileTreeStream stream = new ByteArrayFileTreeStream(path) )
            {
                ByteArrayStreamReader reader = new ByteArrayStreamReader(stream);
            
                OtbmFile file = new OtbmFile();

                stream.Seek(Origin.Current, 4); // Empty

                if ( stream.Child() )
                {
                    file.otbmInfo = OtbmInfo.Load(stream, reader);

                    if ( stream.Child() )
                    {
                        file.mapInfo = MapInfo.Load(stream, reader);

                        if ( stream.Child() )
                        {
                            file.areas = new List<Area>();

                            while(true)
                            {
                                switch ( (OtbmType)reader.ReadByte() )
                                {
                                    case OtbmType.Area:

                                        file.areas.Add( Area.Load(stream, reader) );

                                        break;

                                    case OtbmType.Towns:

                                        if ( stream.Child() )
                                        {
                                            file.towns = new List<Town>();

                                            while (true)
                                            {
                                                file.towns.Add( Town.Load(stream, reader) );

                                                if ( !stream.Next() )
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        break;

                                    case OtbmType.Waypoints:

                                        if ( stream.Child() )
                                        {
                                            file.waypoints = new List<Waypoint>();

                                            while (true)
                                            {
                                                file.waypoints.Add( Waypoint.Load(stream, reader) );

                                                if ( !stream.Next() )
                                                {
                                                    break;
                                                }
                                            }                                
                                        }

                                        break;
                                }

                                if ( !stream.Next() )
                                {
                                    break;
                                }
                            } 
                        }               
                    }
                }

                return file;      
            }
        }

        public static void Save(OtbmFile file, string path)
        {
            ByteArrayMemoryFileTreeStream stream = new ByteArrayMemoryFileTreeStream();

            ByteArrayStreamWriter writer = new ByteArrayStreamWriter(stream);

            writer.Write( (uint)0);

            stream.StartChild();

            OtbmInfo.Save(file.otbmInfo, stream, writer);

            stream.StartChild();

            MapInfo.Save(file.mapInfo, stream, writer);

            if (file.areas != null)
            {
                foreach (var area in file.areas)
                {
                    stream.StartChild();

                    Area.Save(area, stream, writer);

                    stream.EndChild();
                }
            }

            if (file.towns != null)
            {
                stream.StartChild();

                writer.Write( (byte)OtbmType.Towns);

                foreach (var town in file.towns)
                {
                    stream.StartChild();

                    Town.Save(town, stream, writer);

                    stream.EndChild();
                }

                stream.EndChild();
            }

            if (file.waypoints != null)
            {
                stream.StartChild();

                writer.Write( (byte)OtbmType.Waypoints);

                foreach (var waypoint in file.waypoints)
                {
                    stream.StartChild();

                    Waypoint.Save(waypoint, stream, writer);

                    stream.EndChild();
                }

                stream.EndChild();
            }

            stream.EndChild();

            stream.EndChild();

            File.WriteAllBytes(path, stream.GetBytes() );
        }

        private OtbmInfo otbmInfo;

        public OtbmInfo OtbmInfo
        {
            get
            {
                return otbmInfo;
            }
        }

        private MapInfo mapInfo;

        public MapInfo MapInfo
        {
            get
            {
                return mapInfo;
            }
        }

        private List<Area> areas;

        public List<Area> Areas
        {
            get
            {
                return areas;
            }
        }

        private List<Town> towns;

        public List<Town> Towns 
        {
            get
            {
                return towns;
            }
        }

        private List<Waypoint> waypoints;

        public List<Waypoint> Waypoints 
        {
            get
            {
                return waypoints;
            }
        }
    }
}
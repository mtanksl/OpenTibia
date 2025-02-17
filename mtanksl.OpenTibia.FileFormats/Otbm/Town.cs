﻿using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.FileFormats.Otbm
{
    public class Town
    {
        public static Town Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Town town = new Town();

            stream.Seek(Origin.Current, 1); // OtbmType.Town

            town.Id = reader.ReadUInt();

            town.Name = reader.ReadString();

            town.Position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

            return town;
        }

        public static void Save(Town town, ByteArrayMemoryFileTreeStream stream, ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)OtbmType.Town);

            writer.Write( (uint)town.Id);
            
            writer.Write( (string)town.Name);

            writer.Write( (ushort)town.Position.X);

            writer.Write( (ushort)town.Position.Y);

            writer.Write( (byte)town.Position.Z);
        }

        public uint Id { get; set; }

        public string Name { get; set; }

        public Position Position { get; set; }
    }
}
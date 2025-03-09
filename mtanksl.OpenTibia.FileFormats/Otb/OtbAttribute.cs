namespace OpenTibia.FileFormats.Otb
{
    public enum OtbAttribute : byte
    {
    Empty = 0,

        OpenTibiaId = 16,

        TibiaId = 17,

        Speed = 20,

        SpriteHash = 32,

        MinimapColor = 33,

        MaxReadWriteChars = 34,

        MaxReadChars = 35,

        Light = 42,

        TopOrder = 43,

    Start = 254,

    End = 255
    }
}
namespace OpenTibia.Common.Objects
{
    public interface IContent
    {
        TopOrder TopOrder { get; }

        IContainer Container { get; set; }
    }
}
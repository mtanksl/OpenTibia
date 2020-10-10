/*
foreach (var pair in observer.Client.ContainerCollection.GetIndexedContainers() )
{
    if (pair.Value.GetRootContainer() is Tile tile)
    {
        if ( !tile.Position.IsNextTo(toPosition) )
        {
            observer.Client.ContainerCollection.CloseContainer(pair.Key);

            context.WritePacket(observer.Client.Connection, new CloseContainerOutgoingPacket(pair.Key) );
        }
    }
}
*/
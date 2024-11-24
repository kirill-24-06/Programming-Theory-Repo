namespace Match3
{
    public interface IGridData
    {
        int Width { get; }
        int Height { get; }

        GridElement[] Prefabs { get; }
    }
}

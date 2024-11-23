
namespace Match3
{
    /// <summary>
    /// The array where the cells are stored
    /// </summary>
    public class GridSystem
    {
        private GridTile[,] _grid;

        public GridSystem(int width, int height)
        {
            _grid = new GridTile[width, height];
        }

        public static GridSystem NewGrid(int width, int height) => new(width, height);

        public void SetValue(int x, int y, GridTile tile) => _grid[x, y] = tile;

        public GridTile GetValue(int x, int y) => _grid[x, y];
    }
}

using UnityEngine;

namespace Match3
{
    /// <summary>
    /// The array where the cells are stored
    /// </summary>
    public class GridSystem
    {
        private GridCell[,] _grid;

        private int _width;
        private int _height;

        public GridSystem(int width, int height)
        {
            _width = width;
            _height = height;
            _grid = new GridCell[width, height];
        }

        public static GridSystem NewGrid(int width, int height) => new(width, height);

        public void SetValue(int x, int y, GridCell tile) => _grid[x, y] = tile;

        public GridCell GetValue(int x, int y)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                return _grid[x, y];
            }
            else
            {
                return null;
            }
        }

        public GridCell GetValue(Vector2Int vector)
        {
            if (vector.x >= 0 && vector.x < _width && vector.y >= 0 && vector.y < _height)
            {
                return _grid[vector.x, vector.y];
            }
            else
            {
                return null;
            }
        }

        public bool ValueExist(Vector2Int vector) => vector.x >= 0 && vector.x < _width && vector.y >= 0 && vector.y < _height;
    }
}

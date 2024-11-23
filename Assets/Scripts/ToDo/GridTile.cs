using UnityEngine;

namespace Match3
{
    /// <summary>
    /// The cell in which the object that the player interacts with is stored
    /// </summary>
    public class GridTile
    {
        private GridSystem _grid;

        private int x;
        private int y;

        public GridElement Value {  get; private set; }

        public GridTile(GridSystem grid, int x, int y)
        {
            _grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(GridElement value) => Value = Value != null ? Value : value;
    }
}

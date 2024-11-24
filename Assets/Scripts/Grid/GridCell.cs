using UnityEngine;

namespace Match3
{
    /// <summary>
    /// The cell in which the object that the player interacts with is stored
    /// </summary>
    public class GridCell
    {
        private GridSystem _grid;

        private int x;
        private int y;

        public GridElement Value {  get; private set; }

        public GridCell(GridSystem grid, int x, int y)
        {
            _grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(GridElement value) => Value = value;
    }
}

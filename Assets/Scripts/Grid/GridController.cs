using UnityEngine;

namespace Match3
{
    /// <summary>
    /// Performs actions on the grid
    /// </summary>
    public class GridController
    {
        IGridData _data;
        private GridSystem _grid;

        public void CreateNewGrid(IGridData data, Transform parrent)
        {
            _data = data;
            _grid = GridSystem.NewGrid(_data.Width, _data.Height);
            var gridPos = new Vector2Int();

            for (int j = 0; j < _data.Width; j++)
            {
                for (int i = 0; i < _data.Height; i++)
                {
                    var tile = new GridCell(_grid, i, j);
                    gridPos.Set(i, j);


                    //ToDo spawner.SpawnElements();
                    var element = GetElement(_grid, gridPos);
                    element.transform.SetParent(parrent);
                    element.name = $"{i},{j}";

                    tile.SetValue(element);
                    _grid.SetValue(i, j, tile);
                }
            }
        }

        private GridElement GetElement(GridSystem grid, Vector2Int gridPos)
        {
            int index = CheckImmediateMatches(grid, gridPos);

            var element = GameObject.Instantiate(_data.Prefabs[index], new Vector2(gridPos.x, gridPos.y), _data.Prefabs[index].transform.rotation);
            return element;
        }

        /// <summary>
        /// Returns an index where the elements will not be repeated more than twice vertically or horizontally.
        /// </summary>
        private int CheckImmediateMatches(GridSystem grid, Vector2Int gridPos)
        {
            Element prevElementHorizontal = Element.None;
            Element prevElementVertical = Element.None;
            var maxIndex = _data.Prefabs.Length;
            int matchCount = 0;

            if (gridPos.x > 1)
            {
                prevElementHorizontal = grid.GetValue(gridPos.x - 1, gridPos.y).Value.ElementType;
                if (prevElementHorizontal == grid.GetValue(gridPos.x - 2, gridPos.y).Value.ElementType)
                {
                    matchCount = 1;
                }
            }

            if (gridPos.y > 1)
            {
                prevElementVertical = grid.GetValue(gridPos.x, gridPos.y - 1).Value.ElementType;

                if (prevElementVertical == grid.GetValue(gridPos.x, gridPos.y - 2).Value.ElementType)
                {
                    matchCount++;

                    if (matchCount == 1)
                    {
                        prevElementHorizontal = prevElementVertical;
                    }
                }
            }

            int index = Random.Range(0, maxIndex - matchCount);
            var type = _data.Prefabs[index].ElementType;

            if (matchCount > 0 && type == prevElementHorizontal)
            {
                index++;
                type = _data.Prefabs[index].ElementType;
            }

            if (matchCount == 2 && type == prevElementVertical)
            {
                index++;
            }

            return index;
        }

        public GridElement GetElement(Vector2Int position) => _grid.GetValue(position).Value;

        public void SetElement(GridElement element, Vector2Int position)
        {
           var cell = _grid.GetValue(position);

            cell.SetValue(element);
        }

        public bool IsValidPositon(Vector2Int position) => _grid.ValueExist(position);

    }
}

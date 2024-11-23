using UnityEngine;

namespace Match3
{
    /// <summary>
    /// Responsible for starting and completing the level.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelData _data;

        private void Start() => Init();

        private void Init()
        {
            CreateGrid();

            //ToDo:
            //GridController.CreateGrid();
            //LevelStage.Handle();
            //StartTimer();
            //StopGame();
        }

        private void CreateGrid()
        {
            var grid = GridSystem.NewGrid(_data.Width, _data.Height);

            for (int j = 0; j < _data.Width; j++)
            {
                for (int i = 0; i < _data.Height; i++)
                {
                    var tile = new GridTile(grid, i, j);

                    var element = GetElement(grid, i, j);
                    element.transform.SetParent(this.transform);
                    element.name = $"{i},{j}";

                    tile.SetValue(element);
                    grid.SetValue(i, j, tile);
                }
            }
        }

        private GridElement GetElement(GridSystem grid,int i, int j)
        {
            int index = CheckImmediateMatches(grid,i, j);

            var element = Instantiate(_data.Prefabs[index], new Vector2(i, j), _data.Prefabs[index].transform.rotation);
            return element;
        }

        /// <summary>
        /// Returns an index where the elements will not be repeated more than twice vertically or horizontally.
        /// </summary>
        private int CheckImmediateMatches(GridSystem grid,int i, int j)
        {
            Element prevElementHorizontal = Element.None;
            Element prevElementVertical = Element.None;
            var maxIndex = _data.Prefabs.Length;
            int matchCount = 0;

            if (i > 1)
            {
                prevElementHorizontal = grid.GetValue(i - 1, j).Value.ElementType;
                if (prevElementHorizontal == grid.GetValue(i - 2, j).Value.ElementType)
                {
                    matchCount = 1;
                }
            }

            if (j > 1)
            {
                prevElementVertical = grid.GetValue(i, j - 1).Value.ElementType;

                if (prevElementVertical == grid.GetValue(i, j - 2).Value.ElementType)
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
    }
}
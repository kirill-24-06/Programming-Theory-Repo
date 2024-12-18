using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
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
        private Transform _parrent;
        private EventManager _events;

        public GridController(Transform parrent)
        {
            _parrent = parrent;
            _events = EntryPoint.Instance.Events;
        }

        public void CreateNewGrid(IGridData data)
        {
            _data = data;
            _grid = GridSystem.NewGrid(_data.Width, _data.Height);
            var gridPos = new Vector2Int();

            for (int j = 0; j < _data.Height; j++)
            {
                for (int i = 0; i < _data.Width; i++)
                {
                    var cell = new GridCell();
                    gridPos.Set(i, j);


                    //ToDo spawner.SpawnElements();
                    var element = SpawnElement(_grid, gridPos);
                    element.transform.SetParent(_parrent);

                    cell.SetValue(element);
                    _grid.SetValue(i, j, cell);
                }
            }
        }

        private GridElement SpawnElement(GridSystem grid, Vector2Int gridPos)
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

        public async UniTask SwapAsync(Vector2Int position1, Vector2Int position2, float swapTime)
        {
            var element1 = GetElement(position1);
            var element2 = GetElement(position2);

            var tasks = new UniTask[2];

            tasks[0] = ElementMover.MoveAsync(element1.transform, position2, swapTime);
            tasks[1] = ElementMover.MoveAsync(element2.transform, position1, swapTime);

            SetElement(element1, position2);
            SetElement(element2, position1);

            await UniTask.WhenAll(tasks);
        }

        public List<Vector2Int> FindMatches()
        {
            HashSet<Vector2Int> matches = new();

            for (int j = 0; j < _grid.Height; j++)
            {
                for (int i = 0; i < _grid.Width - 2; i++)
                {
                    var element1 = _grid.GetValue(i, j).Value;
                    var element2 = _grid.GetValue(i + 1, j).Value;
                    var element3 = _grid.GetValue(i + 2, j).Value;

                    if (element1 == null || element2 == null || element3 == null) continue;

                    if (element1.ElementType == element2.ElementType
                        && element2.ElementType == element3.ElementType)
                    {
                        matches.Add(new Vector2Int(i, j));
                        matches.Add(new Vector2Int(i + 1, j));
                        matches.Add(new Vector2Int(i + 2, j));
                    }
                }
            }

            for (int i = 0; i < _grid.Width; i++)
            {
                for (int j = 0; j < _grid.Height - 2; j++)
                {
                    var element1 = _grid.GetValue(i, j).Value;
                    var element2 = _grid.GetValue(i, j + 1).Value;
                    var element3 = _grid.GetValue(i, j + 2).Value;


                    if (element1 == null || element2 == null || element3 == null) continue;

                    if (element1.ElementType == element2.ElementType
                        && element2.ElementType == element3.ElementType)
                    {
                        matches.Add(new Vector2Int(i, j));
                        matches.Add(new Vector2Int(i, j + 1));
                        matches.Add(new Vector2Int(i, j + 2));
                    }
                }
            }

            return new List<Vector2Int>(matches);
        }

        public async UniTask DestroyMatches(List<Vector2Int> matches,CancellationToken token)
        {
            foreach (var match in matches)
            {
                var cell = _grid.GetValue(match);

                var element = cell.Value;
                cell.SetValue(null);

                //ToDo: Spawner.SpawnParticle();
                EntryPoint.Instance.AudioController.PlayDestroy();

                await element.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f, 1, 0.5f).ToUniTask(cancellationToken: token);

                GameObject.Instantiate(EntryPoint.Instance.DestroyParticle,
                    element.transform.position, EntryPoint.Instance.DestroyParticle.transform.rotation);
                
                GameObject.Destroy(element.gameObject);
            }
        }

        public async UniTask MoveElements()
        {
            var tasks = new List<UniTask>();

            for (int i = 0; i < _grid.Width; i++)
            {
                for (int j = 0; j < _grid.Height; j++)
                {
                    if (_grid.GetValue(i, j).Value == null)
                    {
                        for (int k = j + 1; k < _grid.Height; k++)
                        {

                            if (_grid.GetValue(i, k).Value != null)
                            {
                                var cell1 = _grid.GetValue(i, k);
                                var cell2 = _grid.GetValue(i, j);
                                var element = cell1.Value;

                                cell2.SetValue(element);
                                cell1.SetValue(null);

                                tasks.Add(ElementMover.MoveAsync(element.transform,
                                      VectorConverter.ToVector2Int(i, j)));

                                break;
                            }
                        }
                    }
                }
            }

            await UniTask.WhenAll(tasks);
        }

        /// <summary>
        /// Creates new elements and places them in the grid until it is full.
        /// </summary>
        public async UniTask FillEmpties(CancellationToken token)
        {
            var isFilled = false;

            while (!isFilled && !token.IsCancellationRequested)
            {
                //ToDo:
                // Make an animation of a fall when instantiating
                isFilled = await FillTask(token);
            }
        }

        private async UniTask<bool> FillTask(CancellationToken token)
        {
            var isFilled = true;
            var j = _grid.Height - 1;

            for (int i = 0; i < _grid.Width; i++)
            {
                var cell = _grid.GetValue(i, j);

                if (cell.Value == null)
                {
                    isFilled = false;
                    int index = Random.Range(0, _data.Prefabs.Length);

                    EntryPoint.Instance.AudioController.PlaySpawnSound();
                    var element = GameObject.Instantiate(_data.Prefabs[index],
                        new Vector2(i, j), _data.Prefabs[index].transform.rotation);

                    element.transform.SetParent(_parrent);

                    cell.SetValue(element);

                    if (token.IsCancellationRequested) break;

                    await MoveElements();
                    var matches = FindMatches();
                    await DestroyMatches(matches,token);
                    _events.AddScore(matches.Count);
                }
            }

            return isFilled;
        }
    }
}
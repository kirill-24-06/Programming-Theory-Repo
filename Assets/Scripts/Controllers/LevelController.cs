using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Match3
{
    /// <summary>
    /// Responsible for starting and completing the level.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelData _data;
        [SerializeField] private InputManager _inputHandler;
        [SerializeField] protected float _swapTime;

        private GridController _controller;
        private Camera _camera;

        private Vector2Int _selectedElement = Vector2Int.one * -1;

        private bool _isSwaping = false;

        private void Start() => Init();

        private void Init()
        {
            _camera = Camera.main;
            CreateGrid();

            _inputHandler.Select += OnElementSelected;

            //ToDo:
            //LevelStage.Handle();
            //StartTimer();
            //StopGame();
        }

        private void CreateGrid()
        {
            _controller = new GridController();
            _controller.CreateNewGrid(_data, transform);
        }

        public void OnElementSelected()
        {
            if (_isSwaping) return;

            var position = VectorConverter.ToVector2Int(_camera.ScreenToWorldPoint(_inputHandler.SelectedPos));

            if (!_controller.IsValidPositon(position)) return;

            if (_selectedElement == position)
            {
                _selectedElement = Vector2Int.one * -1;
            }

            else if (_selectedElement == Vector2Int.one * -1)
            {
                _selectedElement = position;
            }

            else
            {
                SwapLoop(_selectedElement, position).Forget();
            }
        }

        private async UniTaskVoid SwapLoop(Vector2Int selectedElement, Vector2Int position)
        {
            _isSwaping = true;

            await SwapAsync(selectedElement, position);

            //ToDo:
            //FindMatches();
            //DestroyMatches();
            //MakeElemetsFall();
            //SpawnNewElements();
            // Improve swap

            Debug.Log("Done");
            _selectedElement = Vector2Int.one * -1;
            _isSwaping = false;
        }

        private async UniTask SwapAsync(Vector2Int position1, Vector2Int position2)
        {
            var element1 = _controller.GetElement(position1);
            var element2 = _controller.GetElement(position2);

            var tasks = new UniTask[2];

            tasks[0] = MoveAsync(element1.transform, position1, position2);
            tasks[1] = MoveAsync(element2.transform, position2, position1);

            await UniTask.WhenAll(tasks);

            _controller.SetElement(element1, position2);
            _controller.SetElement(element2, position1);
        }

        private async UniTask MoveAsync(Transform element, Vector2Int from, Vector2Int to)
        {
            float count = 0;

            while (count < _swapTime)
            {
                element.transform.position = Vector2.LerpUnclamped(from, to, count / _swapTime);

                count += Time.deltaTime;

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private void OnDestroy()
        {
            _inputHandler.Select -= OnElementSelected;
        }

    }
}
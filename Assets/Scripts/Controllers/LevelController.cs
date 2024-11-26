using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    /// <summary>
    /// Responsible for starting and completing the level.
    /// </summary>
    public class LevelController : IResetable
    {
        private LevelData _data;
        private InputManager _inputHandler;

        private GridController _controller;
        private Camera _camera;

        private Timer _timer;

        private EventManager _events;

        private Vector2Int _selectedElement = Vector2Int.one * -1;

        private bool _isSwaping = false;
        private bool _isGameActive = false;

        public LevelController(LevelData data, Transform elementsParrent)
        {
            _data = data;
            _inputHandler = EntryPoint.Instance.InputHandler;
            _events = EntryPoint.Instance.Events;

            Init(elementsParrent);
        }

        private void Init(Transform elementsParrent)
        {
            _timer = EntryPoint.Instance.Timer;
            _camera = Camera.main;
            CreateGrid(elementsParrent);

            _inputHandler.Select += OnElementSelected;
            _events.Start += OnStart;
            _timer.TimeIsOver += OnStop;

            //ToDo:
            //LevelStage.Handle();
            //StartTimer();
            //StopGame();
        }

        private void OnStart()
        {
            _timer.SetTimer(_data.LevelDuration);
            _timer.StartTimer();

            _isGameActive = true;
        }

        private void OnStop()
        {
            _isGameActive = false;
            _events.Stop?.Invoke();
        }

        private void CreateGrid(Transform transform)
        {
            _controller = new GridController(transform);
            _controller.CreateNewGrid(_data);
        }

        public void OnElementSelected()
        {
            if (_isSwaping || !_isGameActive) return;

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

            List<Vector2Int> matches = _controller.FindMatches();

            _controller.DestroyMatches(matches);

            _controller.MoveElements();

            _events.AddScore?.Invoke(matches.Count);

            await UniTask.Delay(500, cancellationToken: EntryPoint.Instance.SceneExitToken);

            _controller.FillEmpties();

            //ToDo:
            // Improve swap

            _selectedElement = Vector2Int.one * -1;
            _isSwaping = false;
        }

        private async UniTask SwapAsync(Vector2Int position1, Vector2Int position2)
        {
            var element1 = _controller.GetElement(position1);
            var element2 = _controller.GetElement(position2);

            var tasks = new UniTask[2];

            tasks[0] = ElementMover.MoveAsync(element1.transform, position1, position2, _data.SwapTime);
            tasks[1] = ElementMover.MoveAsync(element2.transform, position2, position1, _data.SwapTime);

            await UniTask.WhenAll(tasks);

            _controller.SetElement(element1, position2);
            _controller.SetElement(element2, position1);
        }

        public void Reset()
        {
            _timer.TimeIsOver -= OnStop;
            _inputHandler.Select -= OnElementSelected;
            _events.Start-= OnStart;
        }
    }
}
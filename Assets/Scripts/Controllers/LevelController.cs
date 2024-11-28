using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
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
        private AudioController _audioController;

        private CancellationToken _token;

        private Vector2Int _selectedElement = Vector2Int.one * -1;

        private UniTask _swapLoop;

        private bool _isSwaping = false;
        private bool _isGameActive = false;

        public LevelController(LevelData data, Transform elementsParrent)
        {
            _data = data;
            _inputHandler = EntryPoint.Instance.InputHandler;
            _events = EntryPoint.Instance.Events;
            _audioController = EntryPoint.Instance.AudioController;

            Init(elementsParrent);
        }

        private void Init(Transform elementsParrent)
        {
            _timer = EntryPoint.Instance.Timer;
            _camera = Camera.main;
            CreateGrid(elementsParrent);

            _inputHandler.Select += OnElementSelected;
            _events.Start += OnStart;
            _timer.TimeIsOver += OnTimerEnd;
            _events.Exit += OnExit;

            _token = EntryPoint.Instance.SceneExitToken;

            //ToDo:
            //LevelStage.Handle();
        }

        private void OnExit() => SaveLoad.Save();

        private void OnStart()
        {
            _timer.SetTimer(_data.LevelDuration);
            _timer.StartTimer();

            _isGameActive = true;
        }

        private void OnTimerEnd() => StopGame().Forget();

        private async UniTaskVoid StopGame()
        {
            await _swapLoop;

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
                _audioController.PlayDeselect();
            }

            else if (_selectedElement == Vector2Int.one * -1)
            {
                _selectedElement = position;
                _audioController.PlaySelect();
            }

            else
            {
                _swapLoop = SwapLoop(_selectedElement, position, _token);
            }
        }

        private async UniTask SwapLoop(Vector2Int selectedElement, Vector2Int position, CancellationToken token)
        {
            _isSwaping = true;

            await _controller.SwapAsync(selectedElement, position, _data.SwapTime);

            List<Vector2Int> matches = _controller.FindMatches();
            await _controller.DestroyMatches(matches, token);

            _events.AddScore?.Invoke(matches.Count);

            await _controller.MoveElements();

            await _controller.FillEmpties(token);

            _selectedElement = Vector2Int.one * -1;
            _isSwaping = false;
        }

        public void Reset()
        {
            _timer.TimeIsOver -= OnTimerEnd;
            _inputHandler.Select -= OnElementSelected;
            _events.Start -= OnStart;
            _events.Exit -= OnExit;
        }
    }
}
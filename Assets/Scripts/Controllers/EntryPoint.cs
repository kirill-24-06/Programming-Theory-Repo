using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private LevelData _data;
        [SerializeField] private InputManager _inputHandler;

        [Header("UI")]
        [SerializeField] private GameObject _timeBarGO;
        [SerializeField] private Image _timeBarImage;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private Button _exitButton;

        [Header("Audio")]
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _soundFX;

        public static EntryPoint Instance;

        private LevelController _levelController;
        private ScoreController _scoreController;
        private EventManager _events;
        private UIController _uiController;
        private TimeBar _timeBar;
        private Timer _timer;
        private AudioController _audioController;

        private CancellationToken _sceneExitToken;

        private List<IResetable> _resetables;

        public AudioController AudioController => _audioController;

        public ScoreController ScoreController => _scoreController;

        public InputManager InputHandler => _inputHandler;

        public EventManager Events => _events;

        public Timer Timer => _timer;

        public GameObject DestroyParticle => _data.ParticlePrefab;

        public CancellationToken SceneExitToken => _sceneExitToken;

        private void Awake()
        {
            Instance = this;

            Init();
        }

        private void Start()
        {
            _events.Start?.Invoke();
        }

        private void Init()
        {
            _sceneExitToken = destroyCancellationToken;
            _resetables = new List<IResetable>();

            _events = new EventManager();

            _audioController = new AudioController(_music, _soundFX, _data.Audio);

            _timer = new Timer(this);

            _scoreController = new ScoreController(_data);
            _resetables.Add(_scoreController);

            _inputHandler.Initialize();

            _levelController = new LevelController(_data, transform);
            _resetables.Add(_levelController);

            _timeBar = new TimeBar(_timeBarGO, _timeBarImage);
            _resetables.Add(_timeBar);

            _uiController = new UIController(_scoreText, _bestScoreText, _exitButton, _timeBar);
            _resetables.Add(_uiController);

            ElementMover.GetCancellationToken(_sceneExitToken);
        }

        private void OnApplicationQuit() => _events.Exit?.Invoke();

        private void OnDestroy()
        {
            foreach (var resetable in _resetables)
                resetable.Reset();
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    /// <summary>
    /// Displays the user interface
    /// </summary>
    public class UIController : IResetable
    {
        private TextMeshProUGUI _scoreText;
        private TextMeshProUGUI _bestScoreText;
        private Button _exitButton;
        private TimeBar _timeBar;

        private GameObject _gameOverWindow;

        public UIController(TextMeshProUGUI scoreText, TextMeshProUGUI bestScoreText, Button exitButton, TimeBar timeBar)
        {
            _scoreText = scoreText;
            _bestScoreText = bestScoreText;
            _exitButton = exitButton;
            _timeBar = timeBar;
            _gameOverWindow = (GameObject)Resources.Load(GlobalConstants.GameOverWindowPrefabPath);

            Initialize();
        }

        private void Initialize()
        {
            EntryPoint.Instance.Events.Start += OnStart;
            EntryPoint.Instance.Events.Stop += OnStop;
            EntryPoint.Instance.Events.ChangeScore += ChangeScore;
            EntryPoint.Instance.Events.ChangeBestScore += ChangeBestScore;
            _exitButton.onClick.AddListener(GoToMenu);

            _timeBar.Initialize(EntryPoint.Instance.Timer);
        }

        private void OnStart() => _timeBar.ShowBar();

        private void OnStop() => GameObject.Instantiate(_gameOverWindow);

        private void ChangeScore(int newScore) => _scoreText.text = newScore.ToString();

        private void ChangeBestScore(int newScore) => _bestScoreText.text = newScore.ToString();

        private void GoToMenu() { }

        public void Reset()
        {
            EntryPoint.Instance.Events.Start -= OnStart;
            EntryPoint.Instance.Events.Stop -= OnStop;
            EntryPoint.Instance.Events.ChangeScore -= ChangeScore;
            EntryPoint.Instance.Events.ChangeBestScore -= ChangeBestScore;
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}
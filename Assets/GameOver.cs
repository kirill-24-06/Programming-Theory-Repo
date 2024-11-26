using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.SceneLoading;

namespace Match3
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _bestScoreText;
        [SerializeField] private GameObject _newRecordText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;

        private ScoreController _scoreController;

        private readonly string _score = "Score: ";

        private readonly string _bestScore = "Best Score:";

        private void Start()
        {
            _scoreController = EntryPoint.Instance.ScoreController;
            SetScore();

            _restartButton.onClick.AddListener(Restart);
            _mainMenuButton.onClick.AddListener(GoToMenu);
        }

        private void SetScore()
        {
            _scoreText.text = _score + _scoreController.Score;

            if (SessionData.GetNewBestScore(out int bestScore))
            {
                _bestScoreText.gameObject.SetActive(false);
                _newRecordText.SetActive(true);
            }

            else
                _bestScoreText.text = _bestScore + bestScore;
        }

        private void Restart() => SceneLoader.LoadScene(GlobalConstants.MainSceneName);

        private void GoToMenu() { }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveAllListeners();
            _mainMenuButton.onClick.RemoveAllListeners();
        }

    }
}

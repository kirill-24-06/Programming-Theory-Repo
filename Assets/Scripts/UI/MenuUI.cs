using UnityEngine;
using UnityEngine.UI;
using Utils.SceneLoading;


#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Match3
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitGameButton;

        private void Start()
        {
            _newGameButton.onClick.AddListener(NewGame);
            _exitGameButton.onClick.AddListener(ExitGame);
        }

        private void NewGame() => SceneLoader.LoadScene(GlobalConstants.MainSceneName);

        private void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }
}
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utils.SceneLoading
{
    public static class SceneLoader
    {
        private static Image _loadingImage;
        private static GameObject _loadingScreen;
        private static readonly int _transitionDelay = 1300;

        //Require Loading Screen prefab with image in Resources folder
        static SceneLoader() => _loadingScreen = (GameObject)Resources.Load(GlobalConstants.LoadingScreenPrefabPath);

        public static void LoadScene(string sceneName)
        {
            var loadingScreen = GameObject.Instantiate(_loadingScreen).GetComponent<LoadingScreen>();
            _loadingImage = loadingScreen.LoadingImage;

            LoadSceneAsync(sceneName, loadingScreen.destroyCancellationToken).Forget();
        }

        private static async UniTaskVoid LoadSceneAsync(string sceneName, CancellationToken token)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (!operation.isDone && !token.IsCancellationRequested)
            {
                _loadingImage.fillAmount = operation.progress;

                if (operation.progress >= 0.9f && !operation.allowSceneActivation)
                {
                    await UniTask.Delay(_transitionDelay, cancellationToken: token);
                    operation.allowSceneActivation = true;
                }

                await UniTask.Yield();
            }
        }
    }
}
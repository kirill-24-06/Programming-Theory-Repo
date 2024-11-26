using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
    public class TimeBar: IResetable
    {
        private Image _filler;

        private GameObject _fillerGO;

        private Timer _timer;

        public TimeBar(GameObject filler, Image fillerImage)
        {
            _fillerGO = filler;
            _filler = fillerImage;
        }

        public void Initialize(Timer timer)
        {
            _timer = timer;
            _timer.HasBeenUpdated += OnValueChange;
            _timer.TimeIsOver += OnTimerEnd;
        }

        public void ShowBar() => _fillerGO.SetActive(true);

        private void OnValueChange(float value) => _filler.fillAmount = value;

        private void OnTimerEnd() => _fillerGO.SetActive(false);

        public void Reset()
        {
            _timer.HasBeenUpdated -= OnValueChange;
            _timer.TimeIsOver -= OnTimerEnd;
        }
    }
}

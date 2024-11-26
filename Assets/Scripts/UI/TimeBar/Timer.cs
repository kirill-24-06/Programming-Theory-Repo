using System;
using System.Collections;
using UnityEngine;

namespace Match3
{
    public class Timer
    {
        private float _time;
        private float _remainingTime;

        private IEnumerator _countdown;
        private MonoBehaviour _client;

        public Action TimeIsOver = delegate () { };
        public Action<float> HasBeenUpdated;

        public Timer(MonoBehaviour client) => _client = client;

        public void SetTimer(float time)
        {
            _time = time;
            _remainingTime = _time;
        }

        public void StartTimer()
        {
            _countdown = Countdown();
            _client.StartCoroutine(_countdown);
        }

        public void StopTimer()
        {
            if (_countdown != null)
            {
                _client.StopCoroutine(_countdown);
            }
        }

        private IEnumerator Countdown()
        {
            while (_remainingTime >= 0)
            {
                _remainingTime -= Time.deltaTime;

                HasBeenUpdated?.Invoke(_remainingTime / _time);

                yield return null;
            }

            TimeIsOver?.Invoke();
        }
    }
}

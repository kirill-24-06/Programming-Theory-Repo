using Match3;
using UnityEngine;
public class ScoreController : IResetable
{
    private EventManager _events;

    private float _scoreModifier;
    private float _modifierIncrease;

    private int _scorePerElement;
    private int _score;
    private int BestScore => SessionData.BestScore;

    public int Score => _score;

    public ScoreController(IScoreControllerData data)
    {
        _scoreModifier = data.ScoreModifier;
        _modifierIncrease = data.ModifierIncrease;
        _scorePerElement = data.ScorePerElement;

        Initialize();
    }

    public void Initialize()
    {
        _events = EntryPoint.Instance.Events;

        _events.Start += OnGameStarted;
        _events.AddScore += OnScoreAdded;
    }

    private void OnGameStarted()
    {
        _score = 0;
        _events.ChangeScore?.Invoke(_score);
        _events.ChangeBestScore?.Invoke(BestScore);
    }

    private void OnScoreAdded(int matchesAmount)
    {
        if (matchesAmount > 3)
            _score += Mathf.RoundToInt(_scorePerElement * (_scoreModifier + _modifierIncrease * matchesAmount) * matchesAmount);
        else
            _score += Mathf.RoundToInt(_scorePerElement * _scoreModifier * matchesAmount);

        _events.ChangeScore(_score);

        if (_score > BestScore)
        {
            SessionData.SetBestScore(_score);
            _events.ChangeBestScore?.Invoke(BestScore);
        }
    }

    public void Reset()
    {
        _events.Start -= OnGameStarted;
        _events.AddScore -= OnScoreAdded;
    }
}

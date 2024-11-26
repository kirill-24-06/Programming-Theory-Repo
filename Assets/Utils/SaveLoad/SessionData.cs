public static class SessionData
{
    private static int _bestScore = 0;
    public static int BestScore => _bestScore;

    private static bool _isScoreUpdated = false;

    public static void LoadData(int score) => _bestScore = score;

    public static void SetBestScore(int newScore)
    {
        if (newScore > _bestScore)
        {
            _bestScore = newScore;
            _isScoreUpdated = true;
        }
    }

    public static bool GetNewBestScore(out int bestScore)
    {
        if (_isScoreUpdated)
        {
            _isScoreUpdated = false;
            bestScore = _bestScore;
            return true;
        }

        else
        {
            bestScore = _bestScore;
            return false;
        }
    }
}
using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "new LevelData", menuName = "Match3/LevelData", order = 53)]
    public class LevelData : ScriptableObject,IGridData, IScoreControllerData
    {
        [Header("GridData")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private GridElement[] _prefabs;

        [Header("Score data")]
        [SerializeField] private float _scoreModifier;
        [SerializeField] private float _modifierIncrease;


        [SerializeField] private int _baseScore;
        [SerializeField] private float _swapTime;

        [Header("TimeBar")]
        [SerializeField] private float _levelDuration;



        public int Width => _width;
        public int Height => _height;

        public GridElement[] Prefabs => _prefabs;

        public float ScoreModifier => _scoreModifier;

        public float ModifierIncrease => _modifierIncrease;

        public float SwapTime => _swapTime;

        public int ScorePerElement => _baseScore;

        public float LevelDuration => _levelDuration;
    }
}

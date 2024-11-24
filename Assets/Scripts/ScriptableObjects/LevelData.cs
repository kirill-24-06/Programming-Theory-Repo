using UnityEngine;

namespace Match3
{
    [CreateAssetMenu(fileName = "new LevelData", menuName = "Match3/LevelData", order = 53)]
    public class LevelData : ScriptableObject,IGridData
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private GridElement[] _prefabs;

        public int Width => _width;
        public int Height => _height;

        public GridElement[] Prefabs => _prefabs;

    }
}

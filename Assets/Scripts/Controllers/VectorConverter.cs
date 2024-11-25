using UnityEngine;

namespace Match3
{
    public static class VectorConverter
    {
        private static Vector2Int _vector2Int = new();

        public static Vector2Int ToVector2Int(Vector3 vector)
        {
            _vector2Int.x = Mathf.RoundToInt(vector.x);
            _vector2Int.y = Mathf.RoundToInt(vector.y);

            return _vector2Int;
        }

        public static Vector2Int ToVector2Int(int x, int y)
        {
            _vector2Int.x = x;
            _vector2Int.y = y;

            return _vector2Int;
        }
    }
}
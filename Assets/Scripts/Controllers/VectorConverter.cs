using UnityEngine;

namespace Match3
{
    public static class VectorConverter
    {
        public static Vector2Int ToVector2Int(Vector3 vector)
        {
            int x = Mathf.RoundToInt(vector.x);
            int y = Mathf.RoundToInt(vector.y);

            return new Vector2Int(x, y);
        }
    }
}
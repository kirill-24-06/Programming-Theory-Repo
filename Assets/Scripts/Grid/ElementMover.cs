using Cysharp.Threading.Tasks;
using UnityEngine;

public static class ElementMover
{
    public static async UniTask MoveAsync(Transform element, Vector2Int from, Vector2Int to, float swapTime)
    {
        float count = 0;

        while (count < swapTime && element != null)
        {
            element.transform.position = Vector2.LerpUnclamped(from, to, count / swapTime);

            count += Time.deltaTime;

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public static class ElementMover
{
    private static CancellationToken _token;

    public static async UniTask MoveAsync(Transform element, Vector2Int from, Vector2Int to, float swapTime)
    {
        float count = 0;

        while (count < swapTime && element != null && !_token.IsCancellationRequested)
        {
            element.transform.position = Vector2.LerpUnclamped(from, to, count / swapTime);

            count += Time.deltaTime;

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    public static void GetCancellationToken(CancellationToken token) => _token = token;
}

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using DG.Tweening;

public static class ElementMover
{
    private static CancellationToken _token;

    private static Vector2 _endValue;

    public static async UniTask MoveAsync(Transform element, Vector2Int to, float swapTime = 0.2f)
    {
        _endValue.Set(to.x, to.y);

        await element
            .DOMove(_endValue,swapTime)
            .SetEase(Ease.Linear)
            .ToUniTask(cancellationToken: _token);
    }

    public static void GetCancellationToken(CancellationToken token) => _token = token;

}

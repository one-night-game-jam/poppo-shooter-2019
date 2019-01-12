using Characters;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Cameras
{
    public class CameraOperator : MonoBehaviour
    {
        [Inject]
        IReadOnlyPlayerCore player;

        void Start()
        {
            var y = transform.position.y;
            var diff = transform.position - player.Position;
            var position = player.ObserveEveryValueChanged(p => p.Position)
                .TakeUntil(player.Dead)
                .Select(p => p + diff)
                .Select(p =>
                {
                    p.y = y;
                    return p;
                });

            this.LateUpdateAsObservable().WithLatestFrom(position, (_, p) => p)
                .Subscribe(p => transform.position = p)
                .AddTo(this);
        }
    }
}
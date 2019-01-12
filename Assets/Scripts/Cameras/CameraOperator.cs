using Characters;
using Characters.Players;
using UniRx;
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
            player.ObserveEveryValueChanged(p => p.Position)
                .TakeUntil(player.Dead)
                .Select(p => p + diff)
                .Select(p =>
                {
                    p.y = y;
                    return p;
                })
                .ObserveOn(Scheduler.MainThreadEndOfFrame)
                .Subscribe(p => transform.position = p)
                .AddTo(this);
        }
    }
}
using Characters.Players;
using UniRx;
using UnityEngine;
using Zenject;

namespace Cameras
{
    public class CameraOperator : MonoBehaviour
    {
        [Inject]
        PlayerCore player;

        void Start()
        {
            var y = transform.position.y;
            var diff = transform.position - player.transform.position;
            player.transform.ObserveEveryValueChanged(t => t.position)
                .TakeUntilDestroy(player.transform)
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
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Characters.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField]
        double dueTimeSeconds;
        [SerializeField]
        double updateLogicSpanMillis;
        [SerializeField]
        Range targetDistance;
        [SerializeField]
        float targetRotationDotThreshold;
        [SerializeField]
        float rotateSpeed;

        readonly ReactiveProperty<Vector2> moveDirection = new ReactiveProperty<Vector2>();
        readonly ReactiveProperty<Vector2> targetDirection = new ReactiveProperty<Vector2>();
        readonly ReactiveProperty<bool> fire = new ReactiveProperty<bool>();

        public IReadOnlyReactiveProperty<Vector2> MoveDirection => moveDirection;
        public IReadOnlyReactiveProperty<Vector2> TargetDirection => targetDirection;
        public IReadOnlyReactiveProperty<bool> Fire => fire;

        [Inject]
        IReadOnlyPlayerCore player;

        void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(dueTimeSeconds), TimeSpan.FromMilliseconds(updateLogicSpanMillis))
                .TakeUntil(player.Dead)
                .Select(_ => UpdateLogic())
                .Switch()
                .Finally(Stop)
                .Subscribe()
                .AddTo(this);
        }

        IObservable<Unit> UpdateLogic()
        {
            return Observable.Defer(() =>
            {
                if (targetDistance.IsCover(Distance()))
                {
                    if (IsRotationCover())
                    {
                        return ShootState();
                    }

                    return LockState();
                }

                return MoveState();
            });
        }

        float Distance()
        {
            return Vector3.Distance(transform.position, player.Position);
        }

        bool IsRotationCover()
        {
            return targetRotationDotThreshold < Vector3.Dot(transform.forward, player.Position - transform.position);
        }

        IObservable<Unit> ShootState()
        {
            return Observable.Defer(() =>
            {
                fire.Value = true;
                moveDirection.Value = Vector2.zero;
                return Rotate();
            });
        }

        IObservable<Unit> LockState()
        {
            return Observable.Defer(() =>
            {
                fire.Value = false;
                moveDirection.Value = Vector2.zero;
                return Rotate();
            });
        }

        IObservable<Unit> Rotate()
        {
            return this.UpdateAsObservable()
                .Do(_ =>
                {
                    var rot = Vector3.RotateTowards(transform.forward, player.Position - transform.position,
                        rotateSpeed * Time.deltaTime, 0F);
                    targetDirection.Value = new Vector2(rot.x, rot.z);
                });
        }

        IObservable<Unit> MoveState()
        {
            return Observable.Defer(() =>
            {
                fire.Value = false;

                return this.UpdateAsObservable()
                    .Do(_ =>
                    {
                        var distance = Distance();

                        if (targetDistance.IsCover(distance))
                        {
                            return;
                        }

                        var diff = player.Position - transform.position;
                        if (distance < targetDistance.Start)
                        {
                            diff *= -1;
                        }
                        var direction = new Vector2(diff.x, diff.z);
                        moveDirection.Value = direction;
                        targetDirection.Value = direction;
                    });
            });
        }

        void Stop()
        {
            moveDirection.Value = Vector2.zero;
            fire.Value = false;
        }


        [Serializable]
        struct Range
        {
            [SerializeField]
            float start;
            [SerializeField]
            float length;

            public float Start => start;
            public float End => start + length;

            public bool IsCover(float value)
            {
                return start < value && value < End;
            }
        }
    }
}

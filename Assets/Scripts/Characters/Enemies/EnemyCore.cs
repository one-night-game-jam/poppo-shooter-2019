using System;
using Characters.Commons;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Characters.Enemies
{
    public class EnemyCore : MonoBehaviour, ICharacterCore
    {
        [SerializeField]
        EnemyAI enemyAi;

        [SerializeField]
        DamageApplicable damageApplicable;

        IObservable<Vector2> ICharacterCore.OnMoveAsObservable()
        {
            return this.UpdateAsObservable()
                .TakeUntil(OnDeadAsObservable())
                .WithLatestFrom(enemyAi.MoveDirection, (_, v) => v);
        }

        IObservable<Vector2> ICharacterCore.OnRotateAsObservable()
        {
            return enemyAi.TargetDirection
                .TakeUntil(OnDeadAsObservable());
        }

        IObservable<Vector2> ICharacterCore.OnFireAsObservable()
        {
            return this.UpdateAsObservable()
                .TakeUntil(OnDeadAsObservable())
                .WithLatestFrom(enemyAi.Fire, (_, b) => b)
                .Where(b => b)
                .WithLatestFrom(enemyAi.TargetDirection, (_, v) => v);
        }

        IObservable<Unit> ICharacterCore.OnReloadAsObservable()
        {
            return Observable.Empty<Unit>();
        }

        IObservable<Vector2> ICharacterCore.OnBoostAsObservable()
        {
            return Observable.Empty<Vector2>();
        }

        public IObservable<Unit> OnDeadAsObservable()
        {
            return damageApplicable.Dead;
        }
    }
}

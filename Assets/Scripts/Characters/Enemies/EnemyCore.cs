using System;
using Damages;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Characters.Enemies
{
    public class EnemyCore : MonoBehaviour, ICharacterCore
    {
        [SerializeField]
        EnemyAI enemyAi;

        IObservable<Vector2> ICharacterCore.OnMoveAsObservable()
        {
            return this.UpdateAsObservable()
                .WithLatestFrom(enemyAi.MoveDirection, (_, v) => v);
        }

        IObservable<Vector2> ICharacterCore.OnRotateAsObservable()
        {
            return enemyAi.TargetDirection;
        }

        IObservable<Vector2> ICharacterCore.OnFireAsObservable()
        {
            return this.UpdateAsObservable()
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
    }
}

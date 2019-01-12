using System;
using Inputs;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Characters.Commons;

namespace Characters.Players
{
    public class PlayerCore : MonoBehaviour, ICharacterCore, IReadOnlyPlayerCore
    {
        [Inject]
        IInputEventProvider inputEventProvider;

        [SerializeField]
        private DamageApplicable _damageApplicable;

        Vector3 IReadOnlyPlayerCore.Position => transform.position;
        public IObservable<Unit> Dead => _damageApplicable.Dead;

        IObservable<Vector2> ICharacterCore.OnMoveAsObservable()
        {
            return this.UpdateAsObservable()
                .TakeUntil(Dead)
                .WithLatestFrom(inputEventProvider.MoveDirection, (_, v) => v);
        }

        IObservable<Vector2> ICharacterCore.OnRotateAsObservable()
        {
            return inputEventProvider.TargetDirection
                .TakeUntil(Dead);
        }

        IObservable<Vector2> ICharacterCore.OnFireAsObservable()
        {
            return this.UpdateAsObservable()
                .TakeUntil(Dead)
                .WithLatestFrom(inputEventProvider.Fire, (_, b) => b)
                .Where(b => b)
                .WithLatestFrom(inputEventProvider.TargetDirection, (_, v) => v);
        }

        IObservable<Unit> ICharacterCore.OnReloadAsObservable()
        {
            return inputEventProvider.Reload
                .Where(b => b)
                .AsUnitObservable();
        }

        IObservable<Vector2> ICharacterCore.OnBoostAsObservable()
        {
            return inputEventProvider.Boost
                .Where(b => b)
                .WithLatestFrom(inputEventProvider.TargetDirection, (_, v) => v);
        }
    }
}

using System;
using Inputs;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Damages;

namespace Characters.Players
{
    public class PlayerCore : MonoBehaviour, ICharacterCore
    {
        [Inject]
        IInputEventProvider inputEventProvider;

        [SerializeField]
        private float _initialLife;
        private IObservable<float> _life;
        private ISubject<float> _damage = new Subject<float>();

        private void Start()
        {
            _life = _damage
                .StartWith(0)
                .Scan(0F, (x, y) => x + y)
                .Select(x => _initialLife - x)
                .Share();
            _life.Subscribe();
        }

        IObservable<Vector2> ICharacterCore.OnMoveAsObservable()
        {
            return this.UpdateAsObservable()
                .WithLatestFrom(inputEventProvider.MoveDirection, (_, v) => v);
        }

        IObservable<Vector2> ICharacterCore.OnRotateAsObservable()
        {
            return inputEventProvider.TargetDirection;
        }

        IObservable<Vector2> ICharacterCore.OnFireAsObservable()
        {
            return this.UpdateAsObservable()
                .WithLatestFrom(inputEventProvider.Fire, (_, b) => b)
                .Where(b => b)
                .WithLatestFrom(inputEventProvider.TargetDirection, (_, v) => v);
        }

        public IObservable<Unit> OnReloadAsObservable()
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

        public void ApplyDamage(Damage damage)
        {
            _damage.OnNext(damage.Value);
        }
    }
}

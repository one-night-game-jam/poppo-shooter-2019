using System;
using UnityEngine;
using UniRx;
using Zenject;
using Damages;

namespace Characters.Commons
{
    public class DamageApplicable : MonoBehaviour, IDamageApplicable
    {
        [SerializeField]
        private float _initialLife;
        private IObservable<float> _life;

        public IObservable<Unit> Dead => _dead;
        private IObservable<Unit> _dead;

        private ISubject<float> _damage = new Subject<float>();

        private void Start()
        {
            _life = _damage
                .StartWith(0)
                .Scan(0F, (x, y) => x + y)
                .Select(x => _initialLife - x)
                .Share();
            _life.Subscribe()
                .AddTo(this);

            _dead = _life
                .Where(x => x <= 0)
                .AsUnitObservable()
                .Share();
            _dead.Subscribe()
                .AddTo(this);
        }

        public void ApplyDamage(Damage damage)
        {
            _damage.OnNext(damage.Value);
        }
    }
}

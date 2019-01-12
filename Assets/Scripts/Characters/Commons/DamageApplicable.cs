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
        private Subject<Unit> _dead = new Subject<Unit>();

        private ISubject<float> _damage = new Subject<float>();

        private void Start()
        {
            _life = _damage
                .StartWith(0)
                .Scan(0F, (x, y) => x + y)
                .Select(x => _initialLife - x)
                .Share();
            _life.Where(x => x <= 0)
                .Subscribe(_ => _dead.OnNext(Unit.Default))
                .AddTo(this);
        }

        public void ApplyDamage(Damage damage)
        {
            _damage.OnNext(damage.Value);
        }
    }
}

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
        private readonly ISubject<Unit> _dead = new AsyncSubject<Unit>();

        private readonly ISubject<float> _damage = new Subject<float>();

        private void Start()
        {
            _life = _damage
                .StartWith(0)
                .Scan(0F, (x, y) => x + y)
                .Select(x => _initialLife - x)
                .Share();
            _life.Where(x => x <= 0)
                .Take(1)
                .Subscribe(_ =>
                {
                    _dead.OnNext(Unit.Default);
                    _dead.OnCompleted();
                })
                .AddTo(this);
        }

        public void ApplyDamage(Damage damage)
        {
            _damage.OnNext(damage.Value);
        }
    }
}

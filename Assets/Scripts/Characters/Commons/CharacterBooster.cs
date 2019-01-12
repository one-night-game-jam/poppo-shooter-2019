using System;
using Damages;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    public class CharacterBooster : MonoBehaviour
    {
        [SerializeField]
        float throttleMillis;

        ISubject<float> boostReload = new Subject<float>();
        public IObservable<float> BoostReload => boostReload;

        [SerializeField]
        float power;
        [SerializeField]
        float attackTimeMillis;
        [SerializeField]
        Damage damage;

        [SerializeField]
        Rigidbody _rigidbody;

        [SerializeField]
        ParticleSystem boost;

        [Inject]
        ICharacterCore characterCore;

        void Start()
        {
            var boostTrigger = characterCore.OnBoostAsObservable()
                .ThrottleFirst(TimeSpan.FromMilliseconds(throttleMillis))
                .Share();

            boostTrigger.Do(d => _rigidbody.AddForce(new Vector3(d.x, 0, d.y).normalized * power, ForceMode.VelocityChange))
                .Select(_ => this.OnTriggerEnterAsObservable()
                    .Do(ApplyDamage)
                    .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(attackTimeMillis)))
                )
                .Switch()
                .Subscribe()
                .AddTo(this);

            boostTrigger
                .Subscribe(_ => boost.Play())
                .AddTo(this);

            Observable.WithLatestFrom(
                this.UpdateAsObservable().Select(_ => Time.time),
                boostTrigger.Select(_ => Time.time),
                (x, y) => (x - y) * 1000 / this.throttleMillis
            ).Where(x => x <= 1).Subscribe(x =>
           {
               boostReload.OnNext(x);
           }).AddTo(this);
        }

        void ApplyDamage(Collider collider)
        {
            foreach (var applicable in collider.GetComponents<IDamageApplicable>())
            {
                applicable.ApplyDamage(damage);
            }
        }
    }
}

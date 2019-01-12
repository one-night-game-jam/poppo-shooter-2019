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
        double throttleMillis;
        [SerializeField]
        float power;
        [SerializeField]
        float attackTimeMillis;
        [SerializeField]
        Damage damage;

        [SerializeField]
        Rigidbody _rigidbody;

        [Inject]
        ICharacterCore characterCore;

        void Start()
        {
            characterCore.OnBoostAsObservable()
                .ThrottleFirst(TimeSpan.FromMilliseconds(throttleMillis))
                .Do(d => _rigidbody.AddForce(new Vector3(d.x, 0, d.y).normalized * power, ForceMode.VelocityChange))
                .Select(_ => this.OnTriggerEnterAsObservable()
                    .Do(ApplyDamage)
                    .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(attackTimeMillis)))
                )
                .Switch()
                .Subscribe()
                .AddTo(this);
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

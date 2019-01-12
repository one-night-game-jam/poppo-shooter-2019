using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterBooster : MonoBehaviour
    {
        [SerializeField]
        double throttleMillis;

        [SerializeField]
        float power;

        [SerializeField]
        Rigidbody _rigidbody;

        [Inject]
        ICharacterCore characterCore;

        void Start()
        {
            characterCore.OnBoostAsObservable()
                .ThrottleFirst(TimeSpan.FromMilliseconds(throttleMillis))
                .Subscribe(Boost)
                .AddTo(this);
        }

        void Boost(Vector2 direction)
        {
            _rigidbody.AddForce(new Vector3(direction.x, 0, direction.y).normalized * power, ForceMode.VelocityChange);
        }
    }
}

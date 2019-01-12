using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    public class CharacterWeapon : MonoBehaviour
    {
        [SerializeField]
        double throttleMillis;

        [Inject]
        ICharacterCore characterCore;

        void Start()
        {
            characterCore.OnFireAsObservable()
                .ThrottleFirst(TimeSpan.FromMilliseconds(throttleMillis))
                .Subscribe(Fire)
                .AddTo(this);

            characterCore.OnReloadAsObservable()
                .Subscribe(_ => Reload())
                .AddTo(this);
        }

        void Fire(Vector2 direction)
        {
            // TODO: Implement
            Debug.Log("Fire!");
        }

        void Reload()
        {
            // TODO: Implement
            Debug.Log("Reload!");
        }
    }
}

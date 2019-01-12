using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    public class CharacterWeapon : MonoBehaviour
    {
        [SerializeField]
        GameObject _bulletPrefab;

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
            var rotation = this.transform.rotation;
            rotation.SetLookRotation(new Vector3(direction.x, 0, direction.y));
            Instantiate(_bulletPrefab, this.transform.position, rotation);
        }

        void Reload()
        {
            // TODO: Implement
            Debug.Log("Reload!");
        }
    }
}

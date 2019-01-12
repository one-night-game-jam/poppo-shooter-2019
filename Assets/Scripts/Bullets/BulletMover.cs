using System;
using UnityEngine;
using UniRx;

namespace Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletMover : MonoBehaviour
    {
        [SerializeField]
        Rigidbody _rigidbody;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _lifeTimeMilli;

        private void Start()
        {
            Observable
                .Timer(TimeSpan.FromMilliseconds(_lifeTimeMilli))
                .Subscribe(_ => Destroy(this.gameObject))
                .AddTo(this);
        }

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position += _rigidbody.rotation * Vector3.forward * _speed * Time.deltaTime;
            _rigidbody.position = position;
        }
    }
}

using System;
using UnityEngine;

namespace Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletMover : MonoBehaviour
    {
        [SerializeField]
        Rigidbody _rigidbody;

        [SerializeField]
        private float _speed;

        private void FixedUpdate()
        {
            var position = _rigidbody.position;
            position += _rigidbody.rotation * Vector3.forward * _speed * Time.deltaTime;
            _rigidbody.position = position;
        }
    }
}

using UniRx;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMover : MonoBehaviour
    {
        [SerializeField]
        Rigidbody _rigidbody;
        [SerializeField]
        float speed;

        [Inject]
        ICharacterCore characterCore;

        void Start()
        {
            characterCore.OnMoveAsObservable()
                .ObserveOn(Scheduler.MainThreadFixedUpdate)
                .Select(v => v.normalized * speed * Time.deltaTime)
                .Subscribe(Move)
                .AddTo(this);

            characterCore.OnRotateAsObservable()
                .ObserveOn(Scheduler.MainThreadFixedUpdate)
                .Select(v => Quaternion.LookRotation(new Vector3(v.x, 0, v.y), Vector3.up))
                .Subscribe(Rotate)
                .AddTo(this);
        }

        void Move(Vector2 delta)
        {
            var position = _rigidbody.position;
            position.x += delta.x;
            position.z += delta.y;
            _rigidbody.position = position;
        }

        void Rotate(Quaternion rotation)
        {
            _rigidbody.MoveRotation(rotation);
        }
    }
}

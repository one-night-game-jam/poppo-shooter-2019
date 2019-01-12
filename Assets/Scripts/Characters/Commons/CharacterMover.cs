using UniRx;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMover : MonoBehaviour
    {
        [Inject]
        ICharacterCore characterCore;

        void Start()
        {
        }
    }
}

using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    public class CharacterBooster : MonoBehaviour
    {
        [SerializeField]
        double throttleMillis;

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
            // TODO: Implement
            Debug.Log("Boost!");
        }
    }
}

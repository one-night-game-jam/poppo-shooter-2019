using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Characters.Commons
{
    public class CharacterExploder : MonoBehaviour
    {
        [SerializeField]
        double playTimeMillis;

        [SerializeField]
        ParticleSystem explode;

        [Inject]
        ICharacterCore characterCore;

        private void Start()
        {
            characterCore.OnDeadAsObservable()
                .Subscribe(_ => Explode())
                .AddTo(this);

            characterCore.OnDeadAsObservable()
                .Delay(TimeSpan.FromMilliseconds(playTimeMillis))
                .Subscribe(_ => Destroy(this.gameObject))
                .AddTo(this);
        }

        private void Explode()
        {
            explode.Play();
        }
    }
}

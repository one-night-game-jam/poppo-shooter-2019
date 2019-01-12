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

        [SerializeField]
        DamageApplicable damageApplicable;

        private void Start()
        {
            damageApplicable.Dead
                .Subscribe(_ => Explode())
                .AddTo(this);

            damageApplicable.Dead
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

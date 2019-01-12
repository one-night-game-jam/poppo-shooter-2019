using UnityEngine;
using Zenject;
using Damages;

namespace Characters.Commons
{
    public class DamageApplicable : MonoBehaviour, IDamageApplicable
    {
        [Inject] ICharacterCore _characterCore;

        public void ApplyDamage(Damage damage)
        {
            _characterCore.ApplyDamage(damage);
        }
    }
}

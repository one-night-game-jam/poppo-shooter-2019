using UnityEngine;
using UnityEngine.Serialization;

namespace Damages
{
    public class DamageApplier : MonoBehaviour
    {
        [SerializeField]
        private Damage _damage;

        void OnTriggerEnter(Collider collider)
        {
            foreach (var applicable in collider.GetComponents<IDamageApplicable>())
            {
                applicable.ApplyDamage(_damage);
            }
        }
    }
}

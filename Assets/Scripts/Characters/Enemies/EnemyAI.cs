using UniRx;
using UnityEngine;

namespace Characters.Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        readonly ReactiveProperty<Vector2> moveDirection = new ReactiveProperty<Vector2>();
        readonly ReactiveProperty<Vector2> targetDirection = new ReactiveProperty<Vector2>();
        readonly ReactiveProperty<bool> fire = new ReactiveProperty<bool>();

        public IReadOnlyReactiveProperty<Vector2> MoveDirection => moveDirection;
        public IReadOnlyReactiveProperty<Vector2> TargetDirection => targetDirection;
        public IReadOnlyReactiveProperty<bool> Fire => fire;
    }
}

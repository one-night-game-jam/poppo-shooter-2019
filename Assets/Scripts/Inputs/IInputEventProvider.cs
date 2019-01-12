using UniRx;
using UnityEngine;

namespace Inputs
{
    interface IInputEventProvider
    {
        IReadOnlyReactiveProperty<Vector2> MoveDirection { get; }
        IReadOnlyReactiveProperty<Vector2> TargetDirection { get; }
        IReadOnlyReactiveProperty<bool> Fire { get; }
        IReadOnlyReactiveProperty<bool> Boost { get; }
        IReadOnlyReactiveProperty<bool> Reload { get; }
    }
}

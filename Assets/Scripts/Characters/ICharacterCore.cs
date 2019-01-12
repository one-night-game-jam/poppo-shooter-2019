using System;
using Vector2 = UnityEngine.Vector2;

namespace Characters
{
    interface ICharacterCore
    {
        IObservable<Vector2> OnMoveAsObservable();
        IObservable<Vector2> OnFireAsObservable();
        IObservable<Vector2> OnBoostAsObservable();
    }
}

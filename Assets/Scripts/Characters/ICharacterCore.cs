using System;
using UniRx;
using Vector2 = UnityEngine.Vector2;

namespace Characters
{
    interface ICharacterCore
    {
        IObservable<Vector2> OnMoveAsObservable();
        IObservable<Vector2> OnRotateAsObservable();
        IObservable<Vector2> OnFireAsObservable();
        IObservable<Unit> OnReloadAsObservable();
        IObservable<Vector2> OnBoostAsObservable();
    }
}

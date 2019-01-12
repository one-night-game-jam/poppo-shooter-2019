using System;
using UnityEngine;

namespace Characters.Players
{
    public class PlayerCore : MonoBehaviour, ICharacterCore
    {

        public IObservable<Vector2> OnMoveAsObservable()
        {
            throw new NotImplementedException();
        }

        public IObservable<Vector2> OnFireAsObservable()
        {
            throw new NotImplementedException();
        }

        public IObservable<Vector2> OnBoostAsObservable()
        {
            throw new NotImplementedException();
        }
    }
}

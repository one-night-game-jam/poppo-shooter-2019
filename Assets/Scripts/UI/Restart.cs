using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Characters;

namespace UI
{
    class Restart : MonoBehaviour
    {
        [Inject]
        IReadOnlyPlayerCore player;

        [SerializeField]
        GameObject button;

        private void Start()
        {
            player.Dead.Subscribe(_ => button.SetActive(true));
        }
    }
}

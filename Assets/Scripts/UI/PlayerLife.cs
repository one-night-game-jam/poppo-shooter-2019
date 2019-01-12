using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Characters;

namespace UI
{
    class PlayerLife : MonoBehaviour
    {
        [Inject]
        IReadOnlyPlayerCore player;

        [SerializeField]
        RectTransform bar;

        private void Start()
        {
            player.Life
                .TakeUntil(player.Dead)
                .Subscribe(UpdateBar, () => UpdateBar(0));
        }

        void UpdateBar(float value)
        {
            var scale = bar.transform.localScale;
            scale.x = value;
            bar.transform.localScale = scale;
        }
    }
}

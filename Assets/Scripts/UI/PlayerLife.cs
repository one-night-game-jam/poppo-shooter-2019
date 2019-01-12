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
                .Subscribe<float>(x =>
                {
                    var scale = bar.transform.localScale;
                    scale.x = x;
                    bar.transform.localScale = scale;
                });
        }
    }
}

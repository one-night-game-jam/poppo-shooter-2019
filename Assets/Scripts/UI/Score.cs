using Managers;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace UI
{
    public class Score : MonoBehaviour
    {
        [SerializeField]
        TMP_Text text;

        [SerializeField]
        string format;

        [Inject]
        EnemyContainer enemyContainer;

        void Start()
        {
            enemyContainer
                .ScoreChanged()
                .Subscribe(UpdateText)
                .AddTo(this);
        }

        void UpdateText(int value)
        {
            text.text = value.ToString(format);
        }
    }
}

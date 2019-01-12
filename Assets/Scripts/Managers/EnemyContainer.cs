using System;
using Characters.Enemies;
using UniRx;

namespace Managers
{
    public class EnemyContainer
    {
        readonly ReactiveCollection<EnemyCore> container = new ReactiveCollection<EnemyCore>();

        public void Add(EnemyCore enemyCore)
        {
            container.Add(enemyCore);
        }

        public IObservable<int> ScoreChanged()
        {
            return container.ObserveAdd()
                .SelectMany(x => x.Value.OnDeadAsObservable(), (_, u) => u)
                .Select(_ => 1)
                .StartWith(0)
                .Scan((a, b) => a + b);
        }
    }
}